module Recognizer

open FSharp.Configuration
open System
open System.IO

type Observation = { Label:string; Pixels:int[] }
type Prediction = { Prediction:string; Pixels:int[] }

let toObservation (csvData:string) =
    let columns = csvData.Split(',')
    let label = columns.[0]
    let pixels = columns.[1..] |> Array.map int
    { Label = label; Pixels = pixels }

let reader path = 
    let data = File.ReadAllLines path
    data.[1..]
    |> Array.map toObservation

let trainingFile = FSharp.Configuration.AppSettingsTypeProvider.getConfigValue("app.config", "trainingFile")
let trainingPath = AppDomain.CurrentDomain.BaseDirectory + trainingFile
let trainingData = reader trainingPath

type Distance = int[] * int[] * int -> int

let manhattanDistance (pixels1,pixels2:int[],target) =
    let mutable total = 0
    let len = pixels1 |> Array.length

    for i in 0 .. (len - 1) do
        total <- total + abs (pixels1.[i] - pixels2.[i])

    total
//    Array.zip pixels1 pixels2
//    |> Array.map (fun (x,y) -> abs (x-y))
//    |> Array.sum

let euclideanDistance (pixels1,pixels2:int[],target) =
    let mutable total = 0
    let len = pixels1 |> Array.length

    let diff i = pixels1.[i] - pixels2.[i]

    for i in 0 .. (len - 1) do
        total <- total + diff i * diff i

    total
//    Array.zip pixels1 pixels2
//    |> Array.map (fun (x,y) -> pown (x-y) 2)
//    |> Array.sum

let horizontalBlurArray (arr:int[]) =
    let res = Array.create arr.Length 0
    res.[0] <- (arr.[0] + arr.[1]) / 2
    res.[arr.Length-1] <- (arr.[arr.Length-2] + arr.[arr.Length-1]) /2
    for i in 1 .. arr.Length-2 do
        res.[i] <- (arr.[i-1] + arr.[i] + arr.[i+1]) / 3
    res

let intSqrt x = int (Math.Sqrt(float x))

let centerBlurArray (arr:int[]) =
    let res = Array.create arr.Length 0
    let rowLen = intSqrt res.Length
    for i in 0 .. rowLen-1 do
        for j in 1 .. rowLen-2 do
            let coord = i*rowLen + j
            let prev = i*(rowLen-1) + j
            let next = i*(rowLen+1) + j
            let prevRow = if (prev >= 0) then prev else coord
            let nextRow = if (next < res.Length) then next else coord
            res.[coord] <- (
                arr.[prevRow-1] + arr.[prevRow] + arr.[prevRow+1] +
                arr.[coord-1] + (arr.[coord] * 4) + arr.[coord+1] +
                arr.[nextRow-1] + arr.[nextRow] + arr.[nextRow+1]) / 12
    res

let blurDistance (pixels1,pixels2:int[],target) =
    let blur1 = centerBlurArray pixels1
    let blur2 = centerBlurArray pixels2
    manhattanDistance(blur1,blur2,target)

let train (trainingset:Observation[]) (dist:Distance) =
    let classify (pixels:int[]) =
        trainingset
        |> Array.minBy (fun x -> dist (x.Pixels, pixels, Int32.MaxValue))
        |> fun x -> x.Label
    classify

let classifier = train trainingData

let manhattanClassifier = train trainingData manhattanDistance
let euclideanClassifier = train trainingData euclideanDistance
let blurClassifier = train trainingData blurDistance

let evaluate (validationData:Observation[]) classifier =
    validationData
    |> Array.averageBy (fun x -> if classifier x.Pixels = x.Label then 1. else 0.)
    |> printfn "Correct: %.3f"

let predict (pixels:int[]) classifier =
    classifier pixels

let predictAll (predictionSet:int[][]) classifer =
    predictionSet
    |> Array.map(fun x -> { Prediction = classifer x; Pixels = x })

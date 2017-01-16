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
    data.[350..]
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

let train (trainingset:Observation[]) (dist:Distance) =
    let classify (pixels:int[]) =
        trainingset
        |> Array.minBy (fun x -> dist (x.Pixels, pixels, Int32.MaxValue))
        |> fun x -> x.Label
    classify

let classifier = train trainingData

let manhattanClassifier = train trainingData manhattanDistance
let euclideanClassifier = train trainingData euclideanDistance

let evaluate (validationData:Observation[]) classifier =
    validationData
    |> Array.averageBy (fun x -> if classifier x.Pixels = x.Label then 1. else 0.)
    |> printfn "Correct: %.3f"

let predict (pixels:int[]) classifier =
    classifier pixels

let predictAll (predictionSet:int[][]) classifer =
    predictionSet
    |> Array.map(fun x -> { Prediction = classifer x; Pixels = x })

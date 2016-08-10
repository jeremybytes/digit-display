open System.IO
open System

type Observation = { Label:string; Pixels:int[] }

let toObservation (csvData:string) =
    let columns = csvData.Split(',')
    let label = columns.[0]
    let pixels = columns.[1..] |> Array.map int
    { Label = label; Pixels = pixels }

let reader path = 
    let data = File.ReadAllLines path
    data.[1..]
    |> Array.map toObservation

let trainingPath = @"C:\Development\Articles\DigitDisplay-WithRecognizer\Data\trainingsample.csv"
let trainingData = reader trainingPath

type Distance = int[] * int[] -> int

let manhattanDistance (pixels1,pixels2) =
    Array.zip pixels1 pixels2
    |> Array.map (fun (x,y) -> abs (x-y))
    |> Array.sum

let euclideanDistance (pixels1,pixels2) =
    Array.zip pixels1 pixels2
    |> Array.map (fun (x,y) -> pown (x-y) 2)
    |> Array.sum

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

let blurDistance (pixels1,pixels2:int[]) =
    let blur1 = centerBlurArray pixels1
    let blur2 = centerBlurArray pixels2
    manhattanDistance(blur1,blur2)

let train (trainingset:Observation[]) (dist:Distance) =
    let classify (pixels:int[]) =
        trainingset
        |> Array.minBy (fun x -> dist (x.Pixels, pixels))
        |> fun x -> x.Label
    classify

let classifier = train trainingData

let validationPath = @"C:\Development\Articles\DigitDisplay-WithRecognizer\Data\validationsample.csv"
let validationData = reader validationPath

//validationData
//|> Array.averageBy (fun x -> if classifier x.Pixels = x.Label then 1. else 0.)
//|> printfn "Correct: %.3f"

let evaluate validationData classifier =
    validationData
    |> Array.averageBy (fun x -> if classifier x.Pixels = x.Label then 1. else 0.)
    |> printfn "Correct: %.3f"


let manhattanClassifier = train trainingData manhattanDistance
let euclideanClassifier = train trainingData euclideanDistance
let blurClassifier = train trainingData blurDistance

printfn "Manhattan"
evaluate validationData manhattanClassifier
printfn "BlurManhattan"
evaluate validationData blurClassifier
printfn "Euclidean"
evaluate validationData euclideanClassifier

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

let train (trainingset:Observation[]) (dist:Distance) =
    let classify (pixels:int[]) =
        trainingset
        |> Array.minBy (fun x -> dist (x.Pixels, pixels))
        |> fun x -> x.Label
    classify

let weightedTrain (trainingset:Observation[]) (dist:Distance) =
    let classify (pixels:int[]) =
        trainingset
        |> Array.map (fun x -> (dist (x.Pixels, pixels), x))
        |> Array.sortBy (fun (x,_) -> x)
        |> Array.take 5
        |> Array.countBy (fun (_,x) -> x.Label)
        |> Array.maxBy (fun (_,x) -> x)
        |> fst
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

let weightedManhattanClassifier = weightedTrain trainingData manhattanDistance
let weightedEuclideanClassifer = weightedTrain trainingData euclideanDistance

printfn "Manhattan"
evaluate validationData manhattanClassifier
printfn "Weighted Manhattan"
evaluate validationData weightedManhattanClassifier
printfn "Euclidean"
evaluate validationData euclideanClassifier
printfn "Weighted Euclidean"
evaluate validationData weightedEuclideanClassifer

let dist = manhattanDistance
let pixels = validationData.[12].Pixels
trainingData
|> Array.map (fun x -> (dist (x.Pixels, pixels), x))
|> Array.sortBy (fun (x,_) -> x)
|> Array.take 5
|> Array.countBy (fun (_,x) -> x.Label)
|> Array.maxBy (fun (_,x) -> x)
|> fst


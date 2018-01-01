using System;
using System.Linq;
using ObservationLoader;

namespace CSharp
{
    class Program
    {
        static void Main()
        {
            var distance = new ManhattanDistance();
            var classifier = new BasicClassifier(distance);

            var trainingPath = @"C:\Development\TestApps\MachineLearning\Chapter1\DigitRecognizer\Data\trainingsample.csv";
            var training = DataReader.ReadObservations(trainingPath);
            classifier.Train(training);
            Console.WriteLine("Total Training Records: {0}", training.Count());

            var validationPath = @"C:\Development\TestApps\MachineLearning\Chapter1\DigitRecognizer\Data\validationsample.csv";
            var validation = DataReader.ReadObservations(validationPath);
            Console.WriteLine("Total Validation Records: {0}", validation.Count());

            var correct = Evaluator.Correct(validation, classifier);
            Console.WriteLine("Correctly classified: {0:P2}", correct);

            Console.ReadLine();
        }
    }
}

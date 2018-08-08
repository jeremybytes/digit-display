using DigitLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitConsole
{
    class Program
    {
        private class Prediction
        {
            public string prediction;
            public string actual;
            public string imageString;
        }

        static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            var log = new List<Prediction>();

            string[] rawData = FileLoader.LoadDataStrings();

            var classifier = Recognizer.manhattanClassifier;

            var tasks = new List<Task<string>>();
            foreach (var imageString in rawData)
            {
                int actual = imageString.Split(',').Select(x => Convert.ToInt32(x)).First();
                int[] ints = imageString.Split(',').Select(x => Convert.ToInt32(x)).Skip(1).ToArray();
                var result = Recognizer.predict<string>(ints, classifier);

                var prediction = new Prediction { prediction = result, actual = actual.ToString(), imageString = imageString };

                Console.Clear();
                WriteOutput(prediction);

                if (result != actual.ToString())
                {
                    log = LogError(log, prediction);
                }
            }

            var endTime = DateTime.Now;

            Console.Clear();
            Console.WriteLine("Press ENTER to view errors");
            Console.ReadLine();

            foreach(var pred in log)
            {
                WriteOutput(pred);
                Console.WriteLine("-------------------------------------");
            }
            Console.WriteLine($"Total Errors: {log.Count}");
            Console.WriteLine($"Start Time: {startTime}");
            Console.WriteLine($"End Time: {endTime}");
            Console.WriteLine("\n\nEND END END END END END END END END");
            Console.ReadLine();
        }

        private static List<Prediction> LogError(List<Prediction> log, Prediction prediction)
        {
            var result = log.Append(prediction);
            return result.ToList();
        }

        private static void WriteOutput(Prediction prediction)
        {
            OutputImage(prediction.imageString);
            Console.WriteLine($"Prediction: {prediction.prediction} - Actual: {prediction.actual}");
        }

        private static void OutputImage(string imageString)
        {
            var rawData = imageString.Split(',');
            var integerData = rawData
                .Skip(1)
                .Select(x => Convert.ToInt32(x))
                .ToArray();

            for (int i = 0; i < integerData.Length; i++)
            {
                char outputChar;
                switch (integerData[i])
                {
                    case var low when low > 16 && low < 32:
                        outputChar = '.';
                        break;
                    case var mid when mid >= 32 && mid < 64:
                        outputChar = ':';
                        break;
                    case var high when high >= 64 && high < 192:
                        outputChar = 'o';
                        break;
                    case var reallyHigh when reallyHigh >= 192:
                        outputChar = 'O';
                        break;
                    default:
                        outputChar = ' ';
                        break;
                }

                Console.Write($"{outputChar}{outputChar}");

                if ((i+1) % 28 == 0)
                    Console.Write("\n");
            }
        }
    }
}

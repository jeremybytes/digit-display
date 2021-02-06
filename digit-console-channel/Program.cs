using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using static Recognizers;

namespace digit_console_channel
{
    class Program
    {
        private class Prediction
        {
            public string prediction;
            public string actual;
            public int[] image;
            public int[] closestMatch;
        }

        private static ParallelOptions options = new ParallelOptions()
            { MaxDegreeOfParallelism = 7 };

        private static async Task Listen(ChannelReader<Prediction> reader,
            List<Prediction> log, bool mini = false)
        {
            await foreach (Prediction prediction in reader.ReadAllAsync())
            {
                // Display the result
                Console.SetCursorPosition(0, 0);
                WriteOutput(prediction, mini);

                if (prediction.prediction != prediction.actual.ToString())
                {
                    log = LogError(log, prediction);
                }
            }
        }

        private static async Task Produce(ChannelWriter<Prediction> writer,
            string[] rawData, FSharpFunc<int[], Observation> classifier)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(rawData, options,
                  imageString =>
                  {
                      int actual = imageString.Split(',').Select(x => Convert.ToInt32(x)).First();
                      int[] ints = imageString.Split(',').Select(x => Convert.ToInt32(x)).Skip(1).ToArray();

                      var result = Recognizers.predict(ints, classifier);

                      var prediction = new Prediction
                      {
                          prediction = result.Label,
                          actual = actual.ToString(),
                          image = ints,
                          closestMatch = result.Pixels
                      };
                      writer.WriteAsync(prediction);
                  });
            });

            writer.Complete();
        }

        static async Task Main(string[] args)
        {
            bool mini = false;
            if (args.Length > 0)
            {
                mini = args.Contains("-m");
            }

            Console.Clear();
            Console.WriteLine("Loading training data...");

            var log = new List<Prediction>();

            string fileName = AppDomain.CurrentDomain.BaseDirectory + "train.csv";
            int offset = 6000;
            int recordCount = 400;

            string[] rawTrain = Loader.trainingReader(fileName, offset, recordCount);
            string[] rawValidation = Loader.validationReader(fileName, offset, recordCount);

            var classifier = Recognizers.euclideanClassifier(rawTrain);

            Console.Clear();
            var startTime = DateTime.Now;

            var channel = Channel.CreateUnbounded<Prediction>();

            var listener = Listen(channel.Reader, log, mini);
            var producer = Produce(channel.Writer, rawValidation, classifier);

            await producer;
            await listener;

            var endTime = DateTime.Now;

            Console.Clear();
            if (!mini)
            {
                Console.WriteLine("Press ENTER to view errors");
                Console.ReadLine();

                foreach (var pred in log)
                {
                    WriteOutput(pred, mini);
                    Console.WriteLine("-------------------------------------");
                }
            }
            Console.WriteLine($"Total Errors: {log.Count}");
            Console.WriteLine($"Start Time: {startTime}");
            Console.WriteLine($"End Time: {endTime}");
            Console.WriteLine($"Elapsed: {endTime - startTime:ss}");
            Console.WriteLine("\n\nEND END END END END END END END END");
            Console.ReadLine();
        }

        private static List<Prediction> LogError(List<Prediction> log, Prediction prediction)
        {
            log.Add(prediction);
            return log;
        }

        private static void WriteOutput(Prediction prediction, bool miniDisplay = false)
        {
            Console.WriteLine($"Actual: {prediction.actual} - Prediction: {prediction.prediction}");
            if (miniDisplay)
                Display.OutputImages(prediction.image, null);
            else
                Display.OutputImages(prediction.image, prediction.closestMatch);
        }
    }
}

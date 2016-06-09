using CSharp;
using DigitLoader;
using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DigitDisplay
{
    public partial class MainWindow : Window
    {
        //ManhattanDistance distance;
        //BasicClassifier classifier;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTrainingSet();

            var startTime = DateTimeOffset.Now;

            string[] rawData = FileLoader.LoadDataStrings(100);

            //int[][] rawInts = new int[rawData.Length][];
            //for (int i = 0; i < rawData.Length; i++)
            //{
            //    rawInts[i] = rawData[i].Split(',').Select(x => Convert.ToInt32(x)).ToArray();
            //}

            //var predictions = Recognizer.predictAll(rawInts, Recognizer.manhattanClassifier);


            //for (int i = 0; i < rawData.Length; i++)
            //{
            //    Bitmap image = DigitBitmap.GetBitmapFromRawData(rawData[i]);

            //    var imageControl = new System.Windows.Controls.Image();
            //    imageControl.Source = image.ToWpfBitmap();
            //    imageControl.Width = imageControl.Source.Width;
            //    imageControl.Height = imageControl.Source.Height;

            //    DigitsBox.Children.Add(imageControl);

            //    var textBlock = new TextBlock();
            //    textBlock.Height = imageControl.Height;
            //    textBlock.Width = imageControl.Width;
            //    textBlock.FontSize = 12;
            //    //textBlock.Text = "0";

            //    textBlock.Text = predictions[i];

            //    DigitsBox.Children.Add(textBlock);
            //}


            foreach (var imageString in rawData)
            {
                Bitmap image = DigitBitmap.GetBitmapFromRawData(imageString);

                var imageControl = new System.Windows.Controls.Image();
                imageControl.Source = image.ToWpfBitmap();
                imageControl.Width = imageControl.Source.Width;
                imageControl.Height = imageControl.Source.Height;


                var textBlock = new TextBlock();
                textBlock.Height = imageControl.Height;
                textBlock.Width = imageControl.Width;
                textBlock.FontSize = 12;
                //textBlock.Text = "0";

                int[] ints = imageString.Split(',').Select(x => Convert.ToInt32(x)).ToArray();

                Recognizer.Observation ob = new Recognizer.Observation("", ints);

                var predicted = Recognizer.predict<string>(ob.Pixels, Recognizer.manhattanClassifier);
                textBlock.Text = predicted;

                DigitsBox.Children.Add(imageControl);

                DigitsBox.Children.Add(textBlock);

            }

            var duration = DateTimeOffset.Now - startTime;
            TimingBlock.Text = string.Format("Duration: {0}", duration.ToString());
        }

        private void LoadTrainingSet()
        {
            //distance = new ManhattanDistance();
            //classifier = new BasicClassifier(distance);

            //string trainingFile = ConfigurationManager.AppSettings["trainingFile"];
            //string trainingPath = AppDomain.CurrentDomain.BaseDirectory + trainingFile;

            //var training = DataReader.ReadObservations(trainingPath);
            //classifier.Train(training);


        }
    }
}

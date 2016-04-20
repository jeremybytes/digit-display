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
        ManhattanDistance distance;
        BasicClassifier classifier;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTrainingSet();

            var startTime = DateTimeOffset.Now;

            string[] rawData = FileLoader.LoadDataStrings(1000);

            foreach (var imageString in rawData)
            {
                Bitmap image = DigitBitmap.GetBitmapFromRawData(imageString);

                var imageControl = new System.Windows.Controls.Image();
                imageControl.Source = image.ToWpfBitmap();
                imageControl.Width = imageControl.Source.Width;
                imageControl.Height = imageControl.Source.Height;

                DigitsBox.Children.Add(imageControl);

                var textBlock = new TextBlock();
                textBlock.Height = imageControl.Height;
                textBlock.Width = imageControl.Width;
                textBlock.FontSize = 12;
                //textBlock.Text = "0";

                int[] ints = imageString.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                var predicted = classifier.Predict(ints);
                textBlock.Text = predicted;

                DigitsBox.Children.Add(textBlock);

            }

            var duration = DateTimeOffset.Now - startTime;
            TimingBlock.Text = string.Format("Duration: {0}", duration.ToString());
        }

        private void LoadTrainingSet()
        {
            distance = new ManhattanDistance();
            classifier = new BasicClassifier(distance);

            string trainingFile = ConfigurationManager.AppSettings["trainingFile"];
            string trainingPath = AppDomain.CurrentDomain.BaseDirectory + trainingFile;

            var training = DataReader.ReadObservations(trainingPath);
            classifier.Train(training);
        }
    }
}

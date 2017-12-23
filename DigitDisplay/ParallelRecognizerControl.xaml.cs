using DigitLoader;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DigitDisplay
{
    public partial class ParallelRecognizerControl : UserControl
    {
        string classifierName;
        FSharpFunc<int[], string> classifier;
        string[] rawData;
        //ConcurrentQueue<PredictionData> predictions = new ConcurrentQueue<PredictionData>();

        DateTimeOffset startTime;
        SolidColorBrush redBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 150, 150));
        SolidColorBrush whiteBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        int errors = 0;

        public ParallelRecognizerControl(string classifierName, FSharpFunc<int[], string> classifier,
            string[] rawData)
        {
            InitializeComponent();
            this.classifierName = classifierName;
            this.classifier = classifier;
            this.rawData = rawData;
            Loaded += RecognizerControl_Loaded;
        }

        private void RecognizerControl_Loaded(object sender, RoutedEventArgs e)
        {

            ClassifierText.Text = classifierName;
            PopulatePanel(rawData);
        }

        private struct PredictionData
        {
            public string prediction;
            public string actual;
            public string imageData;
        }

        private void PopulatePanel(string[] rawData)
        {
            startTime = DateTime.Now;

            var options = new ParallelOptions();
            options.TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            var loopResult = Parallel.ForEach(rawData, s =>
            {
                int act = s.Split(',').Select(x => Convert.ToInt32(x)).First();
                int[] ints = s.Split(',').Select(x => Convert.ToInt32(x)).Skip(1).ToArray();
                var result = Recognizer.predict<string>(ints, classifier);

                //predictions.Enqueue(new PredictionData() { 
                //    prediction = result, actual = act.ToString(), imageData = s });

                Task.Factory.StartNew(() =>
                    CreateUIElements(result, act.ToString(), s, DigitsBox),
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    options.TaskScheduler
                );
            });

            //Parallel.Invoke(options, () =>
            //{
            //    PredictionData d;
            //    while (predictions.TryDequeue(out d))
            //    {
            //        if (d.imageData != null)
            //            CreateUIElements(d.prediction, d.actual, d.imageData, DigitsBox);
            //    }
            //});
        }

        private void CreateUIElements(string prediction, string actual, string imageData,
            Panel panel)
        {
            Bitmap image = DigitBitmap.GetBitmapFromRawData(imageData);

            var multiplier = 1.5;
            var imageControl = new System.Windows.Controls.Image();
            imageControl.Source = image.ToWpfBitmap();
            imageControl.Stretch = Stretch.UniformToFill;
            imageControl.Width = imageControl.Source.Width * multiplier;
            imageControl.Height = imageControl.Source.Height * multiplier;

            var textBlock = new TextBlock();
            textBlock.Height = imageControl.Height;
            textBlock.Width = imageControl.Width;
            textBlock.FontSize = 12; // * multiplier;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.Text = prediction;

            var button = new Button();
            var backgroundBrush = whiteBrush;
            button.Background = backgroundBrush;
            button.Click += ToggleCorrectness;

            var buttonContent = new StackPanel();
            buttonContent.Orientation = Orientation.Horizontal;
            button.Content = buttonContent;

            if (prediction != actual)
            {
                button.Background = redBrush;
                errors++;
                ErrorBlock.Text = $"Errors: {errors}";
            }

            buttonContent.Children.Add(imageControl);
            buttonContent.Children.Add(textBlock);

            panel.Children.Add(button);

            TimeSpan duration = DateTimeOffset.Now - startTime;
            TimingBlock.Text = $"Duration (seconds): {duration.TotalSeconds:0}";
        }

        private void ToggleCorrectness(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            if (button.Background == whiteBrush)
            {
                button.Background = redBrush;
                errors++;
            }
            else
            {
                button.Background = whiteBrush;
                errors--;
            }
            ErrorBlock.Text = $"Errors: {errors}";
        }
    }
}

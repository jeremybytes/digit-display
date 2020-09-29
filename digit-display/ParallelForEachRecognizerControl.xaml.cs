using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Recognizers;

namespace DigitDisplay
{
    public partial class ParallelForEachRecognizerControl : UserControl
    {
        #region Control Setup

        private class RecognizerResult
        {
            public string prediction { get; set; }
            public string actual { get; set; }
            public string imageString { get; set; }
        }

        string controlTitle;

        DateTimeOffset startTime;
        SolidColorBrush redBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 150, 150));
        SolidColorBrush whiteBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        int errors = 0;

        public ParallelForEachRecognizerControl(string controlTitle)
        {
            InitializeComponent();
            this.controlTitle = controlTitle + " (Parallel ForEach)";

            Loaded += RecognizerControl_Loaded;
        }

        private void RecognizerControl_Loaded(object sender, RoutedEventArgs e)
        {
            ClassifierText.Text = controlTitle;
        }

        #endregion

        private class Result
        {
            public string Prediction { get; }
            public string Actual { get; }
            public string ImageString { get; }

            public Result(string prediction, string actual, string imageString)
            {
                Prediction = prediction;
                Actual = actual;
                ImageString = imageString;
            }
        }

        private async Task Listen(ChannelReader<Result> reader)
        {
            while (await reader.WaitToReadAsync())
            {
                while (reader.TryRead(out Result result))
                {
                    CreateUIElements(result.Prediction, result.Actual, result.ImageString, DigitsBox);
                }
            }
        }

        private async Task Produce(ChannelWriter<Result> writer, string[] rawData,
            FSharpFunc<int[], Observation> classifier)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(rawData,
                  imageString =>
                  {
                      int actual = imageString.Split(',').Select(x => Convert.ToInt32(x)).First();
                      int[] ints = imageString.Split(',').Select(x => Convert.ToInt32(x)).Skip(1).ToArray();

                      var observation = Recognizers.predict<Observation>(ints, classifier);
                      var result = new Result(observation.Label, actual.ToString(), imageString);
                      writer.WriteAsync(result);
                  });
            });

            writer.Complete();
        }

        public async Task Start(string[] rawData, FSharpFunc<int[], Observation> classifier)
        {
            startTime = DateTime.Now;

            var channel = Channel.CreateUnbounded<Result>();

            var listener = Listen(channel.Reader);
            var producer = Produce(channel.Writer, rawData, classifier);

            await producer;
            await listener;
        }

        private void CreateUIElements(string prediction, string actual, string imageData,
            Panel panel)
        {
            Bitmap image = DigitBitmap.GetBitmapFromRawData(imageData);

            var multiplier = 1.0;
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

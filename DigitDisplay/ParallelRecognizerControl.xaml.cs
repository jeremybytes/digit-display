using DigitLoader;
using Microsoft.FSharp.Core;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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

        private async void RecognizerControl_Loaded(object sender, RoutedEventArgs e)
        {

            ClassifierText.Text = classifierName;
            await PopulatePanel(rawData);
        }

        private async Task PopulatePanel(string[] rawData)
        {
            startTime = DateTime.Now;

            var pipeline = BuildPipeline();

            foreach (var s in rawData)
            {
                pipeline.Post(s);
            }
            
            pipeline.Complete();
            await pipeline.Completion;
        }

        private TransformBlock<string, (string act, int[] ints, string s)> BuildPipeline()
        {
            var transform = new TransformBlock<string, (string act, int[] ints, string s)>(s =>
            {
                var split = s.Split(',');
                var act = split.First();
                var ints = split.Skip(1).Select(x => Convert.ToInt32(x)).ToArray();
                return (act: act, ints: ints, s: s);
            });

            var compute =
                new TransformBlock<(string act, int[] ints, string s), (string act, int[] ints, string s, string result)
                >(
                    tuple =>
                    {
                        var result = Recognizer.predict<string>(tuple.ints, classifier);
                        return (act: tuple.act, ints: tuple.ints, s: tuple.s, result: result);
                    },
                    new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = Math.Max(Environment.ProcessorCount - 1, 1)
                    });

            var output = new ActionBlock<(string act, int[] ints, string s, string result)>(tuple =>
                {
                    Dispatcher.Invoke(() => CreateUIElements(tuple.result, tuple.act, tuple.s, DigitsBox));
                });

            transform.LinkTo(compute);
            compute.LinkTo(output);

            return transform;
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

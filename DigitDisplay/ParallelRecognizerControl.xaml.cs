using DigitLoader;
using Microsoft.FSharp.Core;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DigitDisplay
{
    public partial class ParallelRecognizerControl : UserControl
    {
        readonly string classifierName;
        readonly FSharpFunc<int[], string> classifier;
        readonly string[] rawData;

        DateTimeOffset startTime;
        readonly SolidColorBrush redBrush = new SolidColorBrush(Color.FromRgb(255, 150, 150));
        readonly SolidColorBrush whiteBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        int errors;

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

        private void PopulatePanel(string[] input)
        {
            startTime = DateTimeOffset.Now;
            var uiContext = SynchronizationContext.Current;
            Task.Run(() => Parallel.ForEach(input, data =>
            {
                var stringInts = data.Split(',');
                var result = Recognizer.predict(StringArrayToIntArraySkippingFirstElement(stringInts), classifier);
                uiContext.Post(_ => CreateUIElements(result, stringInts[0], data, DigitsBox), null);
            }));
        }

        private static int[] StringArrayToIntArraySkippingFirstElement(string[] stringInts)
        {
            var result = new int[stringInts.Length - 1];
            for (int intIdx = 0, strIdx = 1; intIdx < result.Length; intIdx++, strIdx++)
            {
                result[intIdx] = int.Parse(stringInts[strIdx], NumberStyles.Integer);
            }
            return result;
        }

        private void CreateUIElements(string prediction, string actual, string imageData,
            Panel panel)
        {
            var imageSource = DigitBitmap.GetBitmapFromRawData(imageData).ToWpfBitmap();
            var scaledSize = new Size(imageSource.Width * 1.5, imageSource.Height * 1.5);
            var imageControl = new Image
            {
                Source = imageSource,
                Stretch = Stretch.UniformToFill,
                Width = scaledSize.Width,
                Height = scaledSize.Height
            };

            var textBlock = new TextBlock
            {
                Height = scaledSize.Height,
                Width = scaledSize.Width,
                FontSize = 12,
                TextAlignment = TextAlignment.Center,
                Text = prediction
            };

            var button = new Button();
            button.Click += ToggleCorrectness;
            if (prediction == actual)
            {
                button.Background = whiteBrush;
            }
            else
            {
                button.Background = redBrush;
                ChangeErrorsCount(1);
            }

            var buttonContent = new StackPanel {Orientation = Orientation.Horizontal};
            buttonContent.Children.Add(imageControl);
            buttonContent.Children.Add(textBlock);
            button.Content = buttonContent;
            panel.Children.Add(button);
            
            TimingBlock.Text = "Duration (seconds): " + (DateTimeOffset.Now - startTime).TotalSeconds.ToString("0");
        }

        private void ToggleCorrectness(object sender, RoutedEventArgs e)
        {
            switch (sender)
            {
                case Button whiteButton when ReferenceEquals(whiteButton.Background, whiteBrush):
                    whiteButton.Background = redBrush;
                    ChangeErrorsCount(1);
                    break;
                case Button redButton when ReferenceEquals(redButton.Background, redBrush):
                    redButton.Background = whiteBrush;
                    ChangeErrorsCount(-1);
                    break;
                default:
                    return;
            }
        }

        private void ChangeErrorsCount(int errorDiff)
        {
            errors += errorDiff;
            ErrorBlock.Text = "Errors: " + errors.ToString("0");
        }
    }
}
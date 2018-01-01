using DigitLoader;
using Microsoft.FSharp.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ObservationLoader;

namespace DigitDisplay
{
    public partial class ParallelRecognizerControl : UserControl
    {
        readonly string classifierName;
        readonly FSharpFunc<int[], string> classifier;
        readonly Observation[] rawData;

        DateTimeOffset startTime;
        int errors;

        public ParallelRecognizerControl(string classifierName, FSharpFunc<int[], string> classifier,
            Observation[] rawData)
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

        private void PopulatePanel(Observation[] input)
        {
            startTime = DateTimeOffset.Now;
            var uiContext = SynchronizationContext.Current;
            ThreadPool.QueueUserWorkItem(state => Parallel.ForEach(input, data =>
            {
                var result = Recognizer.predict(data.Pixels, classifier);
                var imageSource = DigitBitmap.GetBitmapFromRawData(data.Pixels).ToWpfBitmap();
                uiContext.Post(_ => CreateUIElements(result, data.Label, imageSource, DigitsBox), null);
            }));
        }

        private void CreateUIElements(string prediction, string actual, ImageSource imageSource, Panel panel)
        {
            var image = new Image
            {
                Source = imageSource,
                Stretch = Stretch.UniformToFill,
                Width = imageSource.Width * 1.5,
                Height = imageSource.Height * 1.5
            };
            var textBlock = new TextBlock
            {
                Height = image.Height,
                Width = image.Width,
                FontSize = 12,
                TextAlignment = TextAlignment.Center,
                Text = prediction
            };

            var button = new Button();
            button.Click += ToggleCorrectness;
            if (prediction == actual)
            {
                button.Background = MainWindow.WhiteBrush;
            }
            else
            {
                button.Background = MainWindow.RedBrush;
                ChangeErrorsCount(1);
            }

            var buttonContent = new StackPanel {Orientation = Orientation.Horizontal};
            buttonContent.Children.Add(image);
            buttonContent.Children.Add(textBlock);
            button.Content = buttonContent;
            panel.Children.Add(button);
            
            TimingBlock.Text = "Duration (seconds): " + (DateTimeOffset.Now - startTime).TotalSeconds.ToString("0");
        }

        private void ToggleCorrectness(object sender, RoutedEventArgs e)
        {
            switch (sender)
            {
                case Button whiteButton when ReferenceEquals(whiteButton.Background, MainWindow.WhiteBrush):
                    whiteButton.Background = MainWindow.RedBrush;
                    ChangeErrorsCount(1);
                    break;
                case Button redButton when ReferenceEquals(redButton.Background, MainWindow.RedBrush):
                    redButton.Background = MainWindow.WhiteBrush;
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
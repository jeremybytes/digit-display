using DigitLoader;
using Microsoft.FSharp.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ObservationLoader;

namespace DigitDisplay
{
    public partial class RecognizerControl : UserControl
    {
        string classifierName;
        FSharpFunc<int[], string> classifier;
        Observation[] rawData;

        DateTimeOffset startTime;
        int errors = 0;

        public RecognizerControl(string classifierName, FSharpFunc<int[], string> classifier,
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

        private void PopulatePanel(Observation[] rawData)
        {
            startTime = DateTimeOffset.Now;
            foreach (var observation in rawData)
            {
                var task = Task.Run(() => (Recognizer.predict(observation.Pixels, classifier), DigitBitmap.GetBitmapFromRawData(observation.Pixels).ToWpfBitmap()));
                task.ContinueWith(t =>
                    {
                        var (recognized, image) = t.Result;
                        CreateUIElements(recognized, observation.Label, image, DigitsBox);
                    },
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
            }
        }

        private void CreateUIElements(string prediction, string actual, BitmapSource image,
            Panel panel)
        {
            var multiplier = 1.5;
            var imageControl = new Image
            {
                Source = image,
                Stretch = Stretch.UniformToFill,
                Width = image.Width * multiplier,
                Height = image.Height * multiplier
            };

            var textBlock = new TextBlock
            {
                Height = imageControl.Height,
                Width = imageControl.Width,
                FontSize = 12,
                TextAlignment = TextAlignment.Center,
                Text = prediction
            };

            var button = new Button();
            var backgroundBrush = MainWindow.WhiteBrush;
            button.Background = backgroundBrush;
            button.Click += ToggleCorrectness;

            var buttonContent = new StackPanel {Orientation = Orientation.Horizontal};
            button.Content = buttonContent;

            if (prediction != actual)
            {
                button.Background = MainWindow.RedBrush;
                ChangeErrorsCount(1);
            }

            buttonContent.Children.Add(imageControl);
            buttonContent.Children.Add(textBlock);

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

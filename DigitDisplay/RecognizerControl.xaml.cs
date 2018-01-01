using DigitLoader;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            var tasks = new List<Task<string>>();
            foreach (var observation in rawData)
            {
                var task = Task.Run<string>(() => Recognizer.predict(observation.Pixels, classifier));
                tasks.Add(task);
                task.ContinueWith(t =>
                    {
                        CreateUIElements(t.Result, observation.Label, observation, DigitsBox);
                    },
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
            }
            Task.WhenAny(tasks).ContinueWith(t => startTime = DateTime.Now);
        }

        private void CreateUIElements(string prediction, string actual, Observation imageData,
            Panel panel)
        {
            Bitmap image = DigitBitmap.GetBitmapFromRawData(imageData.Pixels);

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
            var backgroundBrush = MainWindow.WhiteBrush;
            button.Background = backgroundBrush;
            button.Click += ToggleCorrectness;

            var buttonContent = new StackPanel();
            buttonContent.Orientation = Orientation.Horizontal;
            button.Content = buttonContent;

            if (prediction != actual)
            {
                button.Background = MainWindow.RedBrush;
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

            if (button.Background == MainWindow.WhiteBrush)
            {
                button.Background = MainWindow.RedBrush;
                errors++;
            }
            else
            {
                button.Background = MainWindow.WhiteBrush;
                errors--;
            }
            ErrorBlock.Text = $"Errors: {errors}";
        }
    }
}

﻿using DigitLoader;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ObservationLoader;

namespace DigitDisplay
{
    public partial class RecognizerUserControl
    {
        private static readonly SolidColorBrush RedBrush = new SolidColorBrush(Color.FromRgb(255, 150, 150));
        private static readonly SolidColorBrush WhiteBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        private DateTimeOffset _startTime;
        private int _errors;
        public RecognizerUserControl(string classifierName, Action<(Observation, Action<string>)[]> dispatcher,
            Observation[] rawData)
        {
            InitializeComponent();
            ClassifierText.Text = classifierName;
            
            Loaded += (_, __) => PopulatePanel(rawData, dispatcher) ;
        }

        private void PopulatePanel(Observation[] input, Action<(Observation, Action<string>)[]> dispatcher)
        {
            (Observation, Action<string>)[] toDispatch = input.Zip(input.Select(CreateButton), (observation, show) => (observation, show)).ToArray();
            _startTime = DateTimeOffset.Now;
            dispatcher(toDispatch);
        }

        private Action<string> CreateButton(Observation observation)
        {
            var imageSource = DigitBitmap.GetBitmapFromRawData(observation.Pixels).ToWpfBitmap();
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
                TextAlignment = TextAlignment.Center
            };

            var button = new Button();
            button.Click += ToggleCorrectness;
            button.Background = WhiteBrush;

            var buttonContent = new StackPanel { Orientation = Orientation.Horizontal };
            buttonContent.Children.Add(image);
            buttonContent.Children.Add(textBlock);
            button.Content = buttonContent;

            void ShowButton(string predicted)
            {
                textBlock.Text = predicted;
                if (predicted != observation.Label)
                {
                    button.Background = RedBrush;
                    ChangeErrorsCount(1);
                }
                DigitsBox.Children.Add(button);

                TimingBlock.Text = "Duration (seconds): " + (DateTimeOffset.Now - _startTime).TotalSeconds.ToString("0");
            }

            return ShowButton;
        }

        private void ToggleCorrectness(object sender, RoutedEventArgs e)
        {
            switch (sender)
            {
                case Button whiteButton when ReferenceEquals(whiteButton.Background, WhiteBrush):
                    whiteButton.Background = RedBrush;
                    ChangeErrorsCount(1);
                    break;
                case Button redButton when ReferenceEquals(redButton.Background, RedBrush):
                    redButton.Background = WhiteBrush;
                    ChangeErrorsCount(-1);
                    break;
                default:
                    return;
            }
        }

        private void ChangeErrorsCount(int errorDiff)
        {
            _errors += errorDiff;
            ErrorBlock.Text = "Errors: " + _errors.ToString("0");
        }
    }
}
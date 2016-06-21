using DigitLoader;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DigitDisplay
{
    public partial class MainWindow : Window
    {
        DateTimeOffset startTime = DateTimeOffset.Now;
        SolidColorBrush redBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 150, 150));
        SolidColorBrush whiteBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        int errors = 0;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string[] rawData = FileLoader.LoadDataStrings(1500);

            foreach (var imageString in rawData)
            {
                var task = Task.Run<string>(
                    () =>
                    {
                        int[] ints = imageString.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                        return Recognizer.predict<string>(ints, Recognizer.manhattanClassifier);
                    }
                );
                task.ContinueWith(t =>
                    {
                        CreateUIElements(t.Result, imageString);
                    },
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
            }
        }

        private void CreateUIElements(string prediction, string imageData)
        {
            Bitmap image = DigitBitmap.GetBitmapFromRawData(imageData);

            var multiplier = 1;
            var imageControl = new System.Windows.Controls.Image();
            imageControl.Source = image.ToWpfBitmap();
            imageControl.Stretch = Stretch.UniformToFill;
            imageControl.Width = imageControl.Source.Width * multiplier;
            imageControl.Height = imageControl.Source.Height * multiplier;

            var textBlock = new TextBlock();
            textBlock.Height = imageControl.Height;
            textBlock.Width = imageControl.Width;
            textBlock.FontSize = 12 * multiplier;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.Text = prediction;

            var button = new Button();
            var backgroundBrush = whiteBrush;
            button.Background = backgroundBrush;
            button.Click += ToggleCorrectness;

            var buttonContent = new StackPanel();
            buttonContent.Orientation = Orientation.Horizontal;
            button.Content = buttonContent;

            buttonContent.Children.Add(imageControl);
            buttonContent.Children.Add(textBlock);

            DigitsBox.Children.Add(button);

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

using CSharp;
using DigitLoader;
using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DigitDisplay
{
    public partial class MainWindow : Window
    {
        DateTimeOffset startTime = DateTimeOffset.Now;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string[] rawData = FileLoader.LoadDataStrings(1000);

            foreach (var imageString in rawData)
            {
                var task = Task.Run<string>(
                    () =>
                    {
                        int[] ints = imageString.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                        Recognizer.Observation ob = new Recognizer.Observation("", ints);
                        return Recognizer.predict<string>(ob.Pixels, Recognizer.manhattanClassifier);
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

        private void CreateUIElements(string prediction, string imageString)
        {
            Bitmap image = DigitBitmap.GetBitmapFromRawData(imageString);

            var imageControl = new System.Windows.Controls.Image();
            imageControl.Source = image.ToWpfBitmap();
            imageControl.Width = imageControl.Source.Width;
            imageControl.Height = imageControl.Source.Height;

            var textBlock = new TextBlock();
            textBlock.Height = imageControl.Height;
            textBlock.Width = imageControl.Width;
            textBlock.FontSize = 12;
            textBlock.Text = prediction;

            DigitsBox.Children.Add(imageControl);
            DigitsBox.Children.Add(textBlock);

            var duration = DateTimeOffset.Now - startTime;
            TimingBlock.Text = string.Format("Duration: {0}", duration.ToString());
        }
    }
}

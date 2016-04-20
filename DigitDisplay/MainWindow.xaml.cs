using DigitLoader;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace DigitDisplay
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var startTime = DateTimeOffset.Now;

            string[] rawData = FileLoader.LoadDataStrings(10000);

            foreach (var imageString in rawData)
            {
                Bitmap image = DigitBitmap.GetBitmapFromRawData(imageString);

                var imageControl = new System.Windows.Controls.Image();
                imageControl.Source = image.ToWpfBitmap();
                imageControl.Width = imageControl.Source.Width;
                imageControl.Height = imageControl.Source.Height;

                DigitsBox.Children.Add(imageControl);

                var textBlock = new TextBlock();
                textBlock.Height = imageControl.Height;
                textBlock.Width = imageControl.Width;
                textBlock.FontSize = 12;
                textBlock.Text = "0";

                DigitsBox.Children.Add(textBlock);

            }

            var duration = DateTimeOffset.Now - startTime;
            TimingBlock.Text = string.Format("Duration: {0}", duration.ToString());
        }
    }
}

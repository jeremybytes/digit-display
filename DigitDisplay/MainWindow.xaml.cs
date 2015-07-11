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

            //string[] rawData = FileLoader.LoadDataStrings(1);

            //int[,] digitArray = DigitBitmap.GenerateDigitArray(rawData[0]);
            //Bitmap image = DigitBitmap.GetBitmapFromDigitArray(digitArray);
            //TestImage.Source = image.ToWpfBitmap();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var startTime = DateTimeOffset.Now;

            string[] rawData = FileLoader.LoadDataStrings(10000);

            foreach (var imageString in rawData)
            {
                //int[,] digitArray = DigitBitmap.GenerateDigitArray(imageString);
                //Bitmap image = DigitBitmap.GetBitmapFromDigitArray(digitArray);
                Bitmap image = DigitBitmap.GetBitmapFromRawData(imageString);

                var imageControl = new System.Windows.Controls.Image();
                imageControl.Source = image.ToWpfBitmap();
                //imageControl.Width = 28;
                //imageControl.Height = 28;
                imageControl.Width = imageControl.Source.Width;
                imageControl.Height = imageControl.Source.Height;

                DigitsBox.Children.Add(imageControl);
            }

            var duration = DateTimeOffset.Now - startTime;
            TimingBlock.Text = string.Format("Duration: {0}", duration.ToString());
        }

    }
}

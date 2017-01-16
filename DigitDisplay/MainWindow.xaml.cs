using DigitLoader;
using Microsoft.FSharp.Core;
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
        SolidColorBrush redBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 150, 150));
        SolidColorBrush whiteBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPanel.Children.Clear();
            RightPanel.Children.Clear();

            int recordCount = int.Parse(RecordCount.Text);
            int offset = int.Parse(Offset.Text);

            string[] rawData = FileLoader.LoadDataStrings(recordCount, offset);

            var manhattanRecognizer = new RecognizerControl(
                "Manhattan Classifier", Recognizer.manhattanClassifier,
                rawData);
            LeftPanel.Children.Add(manhattanRecognizer);

            var euclideanRecognizer = new RecognizerControl(
                "Euclidean Classifier", Recognizer.euclideanClassifier,
                rawData);
            RightPanel.Children.Add(euclideanRecognizer);
        }
    }
}

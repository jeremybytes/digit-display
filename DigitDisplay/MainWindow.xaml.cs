using DigitLoader;
using Microsoft.FSharp.Core;
using System;
using System.Configuration;
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
            Offset.Text = 6000.ToString();
            RecordCount.Text = 459.ToString();
        }

        private async void GoButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPanel.Children.Clear();
            RightPanel.Children.Clear();

            string fileName = AppDomain.CurrentDomain.BaseDirectory + "train.csv";

            int offset = int.Parse(Offset.Text);
            int recordCount = int.Parse(RecordCount.Text);

            string[] rawTrain = await Task.Run(() => Loader.trainingReader(fileName, offset, recordCount));
            string[] rawValidation = await Task.Run(() => Loader.validationReader(fileName, offset, recordCount));

            var manhattanClassifier = Recognizers.manhattanClassifier(rawTrain);

            var manhattanRecognizer = new RecognizerControl(
                "Manhattan Classifier", manhattanClassifier,
                rawValidation);
            LeftPanel.Children.Add(manhattanRecognizer);

            var euclideanClassifier = Recognizers.euclideanClassifier(rawTrain);

            var euclideanRecognizer = new RecognizerControl(
                "Euclidean Classifier", euclideanClassifier,
                rawValidation);
            RightPanel.Children.Add(euclideanRecognizer);
        }
    }
}

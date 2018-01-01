using DigitLoader;
using System.Configuration;
using System.Windows;
using System.Windows.Media;

namespace DigitDisplay
{
    public partial class MainWindow : Window
    {
        public static readonly SolidColorBrush RedBrush = new SolidColorBrush(Color.FromRgb(255, 150, 150));
        public static readonly SolidColorBrush WhiteBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        public MainWindow()
        {
            InitializeComponent();
            Offset.Text = ConfigurationManager.AppSettings["offset"];
            RecordCount.Text = ConfigurationManager.AppSettings["recordCount"];
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPanel.Children.Clear();
            RightPanel.Children.Clear();

            string[] rawData = FileLoader.LoadDataStrings();

            var parallelManhattanRecognizer = new ParallelRecognizerControl(
                "Parallel Manhattan Classifier", Recognizer.manhattanClassifier,
                rawData);
            LeftPanel.Children.Add(parallelManhattanRecognizer);

            var manhattanRecognizer = new RecognizerControl(
                "Manhattan Classifier", Recognizer.manhattanClassifier,
                rawData);
            RightPanel.Children.Add(manhattanRecognizer);
        }
    }
}

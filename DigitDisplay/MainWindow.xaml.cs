using System;
using DigitLoader;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ObservationLoader;

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

            var rawData = FileLoader.LoadObservations();

            var parallelManhattanRecognizer = new RecognizerUserControl(
                "Parallel Manhattan Classifier", DispatchManhattanParallelForEach,
                rawData);
            LeftPanel.Children.Add(parallelManhattanRecognizer);

            var manhattanRecognizer = new RecognizerUserControl(
                "Manhattan Classifier", DispatchManhattanTasks,
                rawData);
            RightPanel.Children.Add(manhattanRecognizer);
        }

        private static void DispatchManhattanParallelForEach((Observation observation, Action<string> showResult)[] input)
        {
            var uiContext = SynchronizationContext.Current;
            ThreadPool.QueueUserWorkItem(state => Parallel.ForEach(input, current =>
            {
                var predicted = Recognizer.predict(current.observation.Pixels, Recognizer.manhattanClassifier);
                uiContext.Post(_ => current.showResult(predicted), null);
            }));
        }

        private static void DispatchManhattanTasks((Observation observation, Action<string> showResult)[] input)
        {
            foreach (var current in input)
            {
                Task
                    .Run(() => Recognizer.predict(current.observation.Pixels, Recognizer.manhattanClassifier))
                    .ContinueWith(t =>
                        {
                            current.showResult(t.Result);
                        },
                        TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}

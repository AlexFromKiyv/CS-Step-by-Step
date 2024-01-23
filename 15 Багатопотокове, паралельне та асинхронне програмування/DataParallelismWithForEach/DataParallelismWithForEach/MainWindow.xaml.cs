using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading;


namespace DataParallelismWithForEach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void cmdProcessWithoutParalell_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            ProcessFilesWithoutParalell(@"D:\Pictures", @$"D:\ModifiedPictures");
            watch.Stop();
            Title = $"Processing complite. Time: {watch.ElapsedMilliseconds}";
        }
        private void ProcessFilesWithoutParalell(string pictureDirectory, string outputDirectory)
        {
            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory,"*.jpg",SearchOption.AllDirectories);

            foreach (var currentFile in files)
            {
                string filename = Path.GetFileName(currentFile);
                int threadId = Environment.CurrentManagedThreadId;
                Title = $"Processing. Thread:{threadId}   File:{filename}";
 
                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(outputDirectory, filename));
            }
        }


        private void cmdProcessWithForEach_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            ProcessFilesWithForEach(@"D:\Pictures", @$"D:\ModifiedPictures");
            watch.Stop();
            Title = $"Processing complite. Time: {watch.ElapsedMilliseconds}";
        }
        private void ProcessFilesWithForEach(string pictureDirectory, string outputDirectory)
        {
            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);
            
            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);

            Parallel.ForEach(files, currentFile =>
            {
                string filename = Path.GetFileName(currentFile);
                //This code statement is now a problem 
                //Title = $"Processing {filename} on thread: {Environment.CurrentManagedThreadId}";
                int threadId = Environment.CurrentManagedThreadId;
                Debug.WriteLine($"Processing. Thread:{threadId}   File:{filename}");

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(outputDirectory, filename));
            });
        }

        private void ProcessWithTaskFactory_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(ProcessFilesWithForEachAndTask);
            //Or
            //Task.Factory.StartNew(() => ProcessFilesWithForEachAndTask());
        }
        private void ProcessFilesWithForEachAndTask()
        {
            string pictureDirectory = @"D:\Pictures";
            string outputDirectory = @"D:\ModifiedPictures";

            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);

            Parallel.ForEach(files, currentFile =>
            {
                string filename = Path.GetFileName(currentFile);
                
                //For title
                int threadId = Environment.CurrentManagedThreadId;
                Dispatcher?.Invoke(() =>
                {
                    Title = $"Processing. Thread:{threadId}   File:{filename}";
                });

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(outputDirectory, filename));
            });
            //For title
            Dispatcher?.Invoke(() => { Title = "Process complete"; });
        }


        private void ProcessWithCancellation_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(ProcessFilesWithCancellation);
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // This will be used to tell all the worker threads to stop!
            _cancellationTokenSource?.Cancel();
        }
        private void ProcessFilesWithCancellation()
        {
            _cancellationTokenSource = new();

            string pictureDirectory = @"D:\Pictures";
            string outputDirectory = @"D:\ModifiedPictures";

            // Use ParallelOptions instance to store the CancellationToken.
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.CancellationToken = _cancellationTokenSource.Token;
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;

            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);

            try
            {
                Parallel.ForEach(files, parallelOptions, currentFile =>
                {
                    parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                    
                        string filename = Path.GetFileName(currentFile);
                        int threadId = Environment.CurrentManagedThreadId;

                        Dispatcher?.Invoke(() =>
                        {
                            Title = $"Processing. Thread:{threadId}   File:{filename}";
                        });

                        using Bitmap bitmap = new(currentFile);
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        bitmap.Save(Path.Combine(outputDirectory, filename));
                    
                });
                Dispatcher?.Invoke(() => { Title = "Process complete"; });
            }
            catch(OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
            } 
        }
    }
}
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // This will be used to tell all the worker threads to stop!
            _cancellationTokenSource?.Cancel(); 
        }

        private void cmdProcess_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            ProcessFiles();
            watch.Stop();
            Title = $"Processing Complete. Time: {watch.ElapsedMilliseconds}";
        }

        private void ProcessFiles()
        {
            var basePath = @"D:\Temp";
            var pictureDirectory = System.IO.Path.Combine(basePath, "Pictures");
            var outputDirectory = System.IO.Path.Combine(basePath, "ModifiedPictures");

            //Clear out any existing files
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            // Process the image data in a blocking manner.
            string[] files = Directory.GetFiles(pictureDirectory,"*.jpg", SearchOption.AllDirectories);
            foreach (string currentFile in files)
            {
                string filename = System.IO.Path.GetFileName(currentFile);
                // Print out the ID of the thread processing the current image.
                Title = $"Processing on thread {Environment.CurrentManagedThreadId} {filename} ";

                using (Bitmap bitmap = new Bitmap(currentFile))
                {
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(System.IO.Path.Combine(outputDirectory, filename));
                }
            }
        }

        private void cmdProcessParrallel_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            ProcessFilesParallel();
            watch.Stop();
            Title = $"Processing Complete. Time: {watch.ElapsedMilliseconds}";
        }
        private void ProcessFilesParallel()
        {
            var basePath = @"D:\Temp";
            var pictureDirectory = System.IO.Path.Combine(basePath, "Pictures");
            var outputDirectory = System.IO.Path.Combine(basePath, "ModifiedPictures");

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
                string filename = System.IO.Path.GetFileName(currentFile);
                //This code statement is now a problem
                //Title = $"Processing {filename} on thread: {Environment.CurrentManagedThreadId}";
                Debug.WriteLine($"Processing on thread: {Environment.CurrentManagedThreadId} file: {filename} ");

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(System.IO.Path.Combine(outputDirectory, filename));
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
            var basePath = @"D:\Temp";
            var pictureDirectory = System.IO.Path.Combine(basePath, "Pictures");
            var outputDirectory = System.IO.Path.Combine(basePath, "ModifiedPictures");

            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);
            Dispatcher?.Invoke(() => { Title = "Starting..."; });
            var watch = Stopwatch.StartNew();

            Parallel.ForEach(files, currentFile =>
            {
                string filename = System.IO.Path.GetFileName(currentFile);

                //For title
                int threadId = Environment.CurrentManagedThreadId;
                Dispatcher?.Invoke(() =>
                {
                    Title = $"Processing. Thread:{threadId}   File:{filename}";
                });

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(System.IO.Path.Combine(outputDirectory, filename));
            });

            //For title
            watch.Stop();
            Dispatcher?.Invoke(() => { Title = $"Processing Complete. Time: {watch.ElapsedMilliseconds}"; });
        }

        private void ProcessWithCancellation_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(ProcessFilesWithCancellation);
        }

        private void ProcessFilesWithCancellation()
        {
            _cancellationTokenSource = new();

            var basePath = @"D:\Temp";
            var pictureDirectory = System.IO.Path.Combine(basePath, "Pictures");
            var outputDirectory = System.IO.Path.Combine(basePath, "ModifiedPictures");

            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            // Use ParallelOptions instance to store the CancellationToken.
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.CancellationToken = _cancellationTokenSource.Token;
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);
            Dispatcher?.Invoke(() => { Title = "Starting..."; });
            var watch = Stopwatch.StartNew();

            try
            {
                Parallel.ForEach(files, parallelOptions, currentFile =>
                {
                    parallelOptions.CancellationToken.ThrowIfCancellationRequested();

                    string filename = System.IO.Path.GetFileName(currentFile);
                    int threadId = Environment.CurrentManagedThreadId;

                    Dispatcher?.Invoke(() =>
                    {
                        Title = $"Processing. Thread:{threadId}   File:{filename}";
                    });

                    using Bitmap bitmap = new(currentFile);
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(System.IO.Path.Combine(outputDirectory, filename));

                });
                //For title
                watch.Stop();
                Dispatcher?.Invoke(() => { Title = $"Processing Complete. Time: {watch.ElapsedMilliseconds}"; });

            }
            catch (OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
            }
        }
    }
}
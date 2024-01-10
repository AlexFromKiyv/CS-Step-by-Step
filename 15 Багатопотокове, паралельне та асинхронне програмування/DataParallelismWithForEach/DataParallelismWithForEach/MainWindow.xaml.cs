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


namespace DataParallelismWithForEach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        private void cmdProcessWithForEach_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            ProcessFilesWithForEach(@"D:\Pictures", @$"D:\ModifiedPictures");
            watch.Stop();
            Title = $"Processing complite. Time: {watch.ElapsedMilliseconds}";
        }


        private void ProcessWithTaskFactory_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            Task.Factory.StartNew(ProcessFilesWithForEachAndTask);
            //Or
            //Task.Factory.StartNew(() => ProcessFilesWithForEachAndTask());
            watch.Stop();
            Title = $"Processing complite. Time: {watch.ElapsedMilliseconds}";
        }



        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

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
                Title = $"Processing {filename} on thread: {Environment.CurrentManagedThreadId}";
                Debug.WriteLine($"Processing {filename} on thread: {Environment.CurrentManagedThreadId}");

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(outputDirectory, filename));
            }
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
                Debug.WriteLine($"Processing {filename} on thread: {Environment.CurrentManagedThreadId}");

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(outputDirectory, filename));
            });
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
                Debug.WriteLine($"Processing {filename} on thread: {Environment.CurrentManagedThreadId}");

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(outputDirectory, filename));
            });
            Debug.WriteLine("Process complete.");
        }



    }
    
}
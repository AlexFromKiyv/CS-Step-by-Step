using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.IO;
using System.Drawing;

namespace PictureHandlerWithAsyncAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource? _cancellationTokenSource = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }


        private void DoWork(int interval)
        {
            Thread.Sleep(interval); // Emulation the long work
        }

        private async void cmdDoWork_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                Dispatcher?.Invoke(() => { Title = $"Start work in thread:{threadId} ...";});
                DoWork(10000);
                Dispatcher?.Invoke(() => { Title = $"Work completed!"; });
            });
        }

        private async void cmdProcess_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource = new();
            string pictureDirectory = @"D:\Temp\Pictures";
            string outputDirectory = @"D:\Temp\ModifiedPictures";

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
                foreach (var file in files)
                {
                    await ProcessFileAsync(file, outputDirectory, _cancellationTokenSource.Token);
                }
                Title = "Process complite";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            _cancellationTokenSource = null;
        }

        private async Task ProcessFileAsync(string currentFile, string outputDirectory, CancellationToken token)
        {
            string filename = Path.GetFileName(currentFile);
            using Bitmap bitmap = new(currentFile);
            try
            {
                await Task.Run(() =>
                {
                    int threadId = Thread.CurrentThread.ManagedThreadId;
                    Dispatcher?.Invoke(() => { Title = $"Processing in thread:{threadId} {filename}"; });
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(Path.Combine(outputDirectory, filename));
                }, token);
            }
            catch (OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async void cmdProcessWithForEachAsync_Click(object sender, RoutedEventArgs e)
        {
            await ProcessWithForEachAsync();
        }

        private async Task ProcessWithForEachAsync()
        {
            _cancellationTokenSource = new();

            string pictureDirectory = @"D:\Temp\Pictures";
            string outputDirectory = @"D:\Temp\ModifiedPictures";

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
                await Parallel.ForEachAsync(files, parallelOptions, async (currentFile, token) =>
                {
                    token.ThrowIfCancellationRequested();
                    string filename = Path.GetFileName(currentFile);

                    //For title
                    int threadId = Environment.CurrentManagedThreadId;
                    Dispatcher?.Invoke(() =>
                    {
                        Title = $"Processing in thread:{threadId}   File:{filename}";
                    });

                    using Bitmap bitmap = new Bitmap(currentFile);

                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(Path.Combine(@"D:\Temp\ModifiedPictures", filename));
                });
                Dispatcher?.Invoke(() => Title = "Process complite.");
            }
            catch (OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
            }
        }

    }
}
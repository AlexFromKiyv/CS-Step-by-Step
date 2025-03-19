using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace MyWordPad;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        SetF1CommandBinding();
    }

    private void SetF1CommandBinding()
    {
        CommandBinding helpBinding = new CommandBinding(ApplicationCommands.Help);

        helpBinding.CanExecute += CanHelpExecute;
        helpBinding.Executed += HelpExecute;

        CommandBindings.Add(helpBinding);
    }

    private void CanHelpExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void HelpExecute(object sender, ExecutedRoutedEventArgs e)
    {
        MessageBox.Show("Look, it is not that difficult. Just type something!", "Help");
    }


    private void FileExit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
    private void MouseEnterExitArea(object sender, MouseEventArgs e)
    {
        statBarText.Text = "Exit the Application";
    }

    private void MouseLeaveArea(object sender, MouseEventArgs e)
    {
        statBarText.Text = "Ready";
    }

    private void MouseEnterToolsHintsArea(object sender, MouseEventArgs e)
    {
        statBarText.Text = "Show Spelling Suggestions";
    }
    private void ToolsSpellingHints_Click(object sender, RoutedEventArgs e)
    {
        string spellingHints = string.Empty;

        // Try to get a spelling error at the current caret location.
        SpellingError error = txtData.GetSpellingError(txtData.CaretIndex);
        if (error != null)
        {
            // Build a string of spelling suggestions.
            foreach (string s in error.Suggestions)
            {
                spellingHints += $"{s}\n";
            }

            // Show suggestions and expand the expander.
            lblSpellingHints.Content = spellingHints;
            expanderSpelling.IsExpanded = true;
        }
    }

    private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void SaveCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }


    private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        // Create an open file dialog box and only show XAML files.
        var openDlg = new OpenFileDialog { Filter = "Text Files |*.txt" };

        // Did they click on the OK button?
        if (openDlg.ShowDialog() == true)
        {
            // Load all text of selected file.
            string dataFromFile = File.ReadAllText(openDlg.FileName);

            // Show string in TextBox.
            txtData.Text = dataFromFile;
        }
    }

    private void SaveCmdExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        var saveDlg = new SaveFileDialog { Filter = "Text Files |*.txt" };

        // Did they click on the OK button?
        if (true == saveDlg.ShowDialog())
        {
            // Save data in the TextBox to the named file.
            File.WriteAllText(saveDlg.FileName, txtData.Text);
        }
    }
}
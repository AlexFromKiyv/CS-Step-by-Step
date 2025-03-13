using System.Configuration;
using System.Data;
using System.Windows;

namespace WpfTesterApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.Properties["GodMode"] = false;
            // Check the incoming command-line arguments and see if they
            // specified a flag for /GODMODE.
            foreach (string arg in e.Args)
            {
                if (arg.Equals("/godmode",StringComparison.OrdinalIgnoreCase))
                {
                    Current.Properties["GodMode"] = true;
                    break;
                }
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }

}

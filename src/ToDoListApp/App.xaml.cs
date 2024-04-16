using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;
using ToDoListApp.Utils;

namespace ToDoListApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AllocConsole();
            TraceUtil.LogWelcomeMessage();
            TraceUtil.LogMethodCall(sender, e);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            FreeConsole();
        }

    }

}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;


namespace ParentalControlsUtils
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                cliMode(e.Args);
                Shutdown();
                return;
            }
            base.OnStartup(e);   
        }
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        private void cliMode(string[] args)
        {
            AllocConsole();
            //replace with logic from MainWindow.xaml.cs
            if (args.Contains("--test"))
            {
                Console.WriteLine("Works!");
                var test = Console.ReadLine();
                
            }       
        }
    }
}

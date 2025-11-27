using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
        private void cliMode(string[] args)
        {
            //replace with logic from MainWindow.xaml.cs
            if (args.Contains("--test"))
            {
                Console.WriteLine("Works!");
            }       
        }
    }
}

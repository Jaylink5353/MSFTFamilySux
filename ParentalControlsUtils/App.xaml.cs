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
            if (args.Contains("--test"))
            {
                Console.WriteLine("Works!");
                var test = Console.ReadLine();
            }
            if (args.Contains("--status"))
            {

            }
            if (args.Contains("--enable"))
            {

            }
            if (args.Contains("--disable"))
            {

            }
            if (args.Contains("--help"))
            {
                Console.WriteLine("Possible Arguments:");
                Console.WriteLine("--help      Prints this dialogue.");
                Console.WriteLine("--status    Gets the status of the WpcMon service");
                Console.WriteLine("--enable    Enables the WpcMon service, so Family Safety runs.");
                Console.WriteLine("--disable   Disables the WpcMon service, so Family Safety doesn't run");
            }
            if (args.Contains("--quiet"))
            {
                return;
            }
            if (!args.Contains("--quiet"))
            {
                Console.WriteLine("[Enter] to continue");
                var notUsed = Console.ReadLine();
            }
        }
    }
}

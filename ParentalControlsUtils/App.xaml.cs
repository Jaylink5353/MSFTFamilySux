using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.ServiceProcess;
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
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        public bool IsServiceRunning()
        {
            string serviceName = "WpcMonSvc";
            try
            {
                using (ServiceController service = new ServiceController(serviceName))
                {
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        Console.WriteLine($"Service '{serviceName}' is running.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Service '{serviceName}' is NOT running. Status: {service.Status}");
                        return false;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"Service '{serviceName}' not found.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        private bool DisableSvc()
        {
            string serviceName = "WpcMonSvc";
            using (ServiceController service = new ServiceController(serviceName))
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    try
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        Console.WriteLine("Successfully Disabled!");
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        if (ex.Message.Contains("The pipe has been ended"))
                        {
                            // Optionally log or inform the user, but continue execution
                            Console.WriteLine("Service stopped, but you may want to verify it actually stopped. Open Services and find 'Parental Controls', and under 'Status', make sure there is nothing.");
                            return true;
                        }
                        if (ex.Message.Contains("stop WpcMonSvc service on computer '.'"))
                        {
                            Console.WriteLine("Service Stopped, but check with --status.");
                            return false;
                        }
                        else
                        {
                            Console.WriteLine($"Service stop failed: {ex.Message}");
                            return true;
                        }
                    }
                    catch (Exception ex) //Removing this results in unhandeled exception
                    {
                        if (ex.Message.Contains("stop WpcMonSvc service on computer '.'"))
                        {
                            Console.WriteLine("Service may have stopped, please check with the --status arg.");
                            return false;
                        }
                        Console.WriteLine($"Service stop failed: {ex.Message}");
                        return true;
                    }
                }
            }
            try
            {
                using (ManagementObject service = new ManagementObject($"Win32_Service.Name='{serviceName}'"))
                {
                    // 4 = Disabled, 3 = Manual, 2 = Automatic
                    ManagementBaseObject inParams = service.GetMethodParameters("ChangeStartMode");
                    inParams["StartMode"] = "Disabled";
                    service.InvokeMethod("ChangeStartMode", inParams, null);
                    Console.WriteLine("Service startup type set to Disabled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to change startup type: {ex.Message}");
                return true;
            }
            return false;
        }
        private bool enableSvc()
        {
            string serviceName = "WpcMonSvc";

            try
            {
                using (ManagementObject service = new ManagementObject($"Win32_Service.Name='{serviceName}'"))
                {
                    // 4 = Disabled, 3 = Manual, 2 = Automatic
                    ManagementBaseObject inParams = service.GetMethodParameters("ChangeStartMode");
                    inParams["StartMode"] = "Manual";
                    service.InvokeMethod("ChangeStartMode", inParams, null);
                    Console.WriteLine("Service startup type set to Manual (Default).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to change startup type: {ex.Message}");
                return true;
            }

            using (ServiceController service = new ServiceController(serviceName))
            {
                // Check if the service is running
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    // Stop the service
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                    Console.WriteLine("Sucessfully Enabled!");
                   
                }
                return false;
            }
            
        }


        private void cliMode(string[] args)
        {
            if (!args.Contains("--quiet"))
            {
                AllocConsole();
            }
            if (args.Contains("--status"))
            {
                IsServiceRunning();
            }
            if (args.Contains("--enable"))
            {
                enableSvc();
            }
            if (args.Contains("--disable"))
            {
                var err = DisableSvc();
                if (err == false)
                {
                    Console.WriteLine("Service Disabled Sucessfully.");
                }
                if (err == true)
                {
                    Console.WriteLine("An Error Occurred.");
                }
            }
            if (args.Contains("--help"))
            {
                Console.WriteLine("Possible Arguments:");
                Console.WriteLine("--help      Prints this dialogue.");
                Console.WriteLine("--status    Gets the status of the WpcMon service");
                Console.WriteLine("--enable    Enables the WpcMon service, so Family Safety runs.");
                Console.WriteLine("--disable   Disables the WpcMon service, so Family Safety doesn't run");
                Console.WriteLine("--quiet     Doesn't init window, so you see no feedback");
            }
            Console.WriteLine("[Enter] to continue");
            var notUsed = Console.ReadLine();
        }
    }
}

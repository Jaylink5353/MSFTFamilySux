using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceProcess;
using System.Management; // Add this at the top

namespace ParentalControlsUtils
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FixText();
        }
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
                MessageBox.Show($"Service '{serviceName}' not found.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show($"An error has occurred: {ex.Message}.");
                return false;
            }
        }
        public void FixText()
        {
            var runningStat = IsServiceRunning();
            string text;
            if (runningStat)
            {
                text = $"Family Safety Service Status: Running";
            }
            else
            {
                text = $"Family Safety Service Status: Not Running";
            }
            servStat.Text = text;
        }
        private void Button_ClickP(object sender, RoutedEventArgs e)
        {
            // Change the service startup type to Disabled
            string serviceName = "WpcMonSvc";
            try
            {
                using (ManagementObject service = new ManagementObject($"Win32_Service.Name='{serviceName}'"))
                {
                    // 4 = Disabled, 3 = Manual, 2 = Automatic
                    ManagementBaseObject inParams = service.GetMethodParameters("ChangeStartMode");
                    inParams["StartMode"] = "Disabled";
                    service.InvokeMethod("ChangeStartMode", inParams, null);
                    MessageBox.Show("Service startup type set to Disabled.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to change startup type: {ex.Message}");
            }
            using (ServiceController service = new ServiceController(serviceName))
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    try
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        MessageBox.Show("Successfully Disabled!");
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        if (ex.Message.Contains("The pipe has been ended"))
                        {
                            // Optionally log or inform the user, but continue execution
                            MessageBox.Show("Service stopped, but you may want to verify it actually stopped. Open Services and find 'Parental Controls', and under 'Status', make sure there is nothing.");
                        }
                        else
                        {
                            MessageBox.Show($"Service stop failed: {ex.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Service stop failed: {ex.Message}");
                    }
                }
            }
        }

        private void Button_ClickR(object sender, RoutedEventArgs e)
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
                    MessageBox.Show("Service startup type set to Manual (Default).");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to change startup type: {ex.Message}");
            }

            using (ServiceController service = new ServiceController(serviceName))
            {
                // Check if the service is running
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    // Stop the service
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                    MessageBox.Show("Sucessfully Enabled!");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //RefreshButton
            FixText();
        }
    }
}

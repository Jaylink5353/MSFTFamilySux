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
using System.Net;
using Markdig;
namespace GainAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DisplayMarkdownFromUrl();
        }
        private void DisplayMarkdownFromUrl()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string url = "https://raw.githubusercontent.com/Jaylink5353/MSFTFamilySux/refs/heads/admingain/GainAdmin.md";
                    string markdown = client.DownloadString(url);
                    string html = Markdig.Markdown.ToHtml(markdown);
                    MarkdownBrowser.NavigateToString(html);
                }
            }
            catch (System.Net.WebException ex)
            {
                // Optionally log or display the error message
                MessageBox.Show("Check your internet connection. Failed to load markdown: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

    }

}

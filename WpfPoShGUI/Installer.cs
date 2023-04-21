using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using HttpClientProgress;

namespace WpfPoShGUI
{

    public partial class MainWindow : Window
    {
        // Initialize HTTPClient Class. Only needs to be done once!
        static readonly HttpClient client = new HttpClient();

        public async Task<bool> DownloadFile(string docUrl, string fileName, string startLocation, string installSwitch)
        {
            // for the sake of the example lets add a client definition here
            var filePath = Path.Combine(@"C:\Users\Public\Downloads", fileName);

            // Setup your progress reporter
            var progress = new Progress<float>();
            progress.ProgressChanged += Progress_ProgressChanged;

            // Use the provided extension method
            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                await client.DownloadDataAsync(docUrl, file, progress);

            ScriptOutput.AppendText("\nInstalling...");
            ProgressText.Text = "Installing...";

            var install = await Task<bool>.Run(() =>
            {
                // if ADW no need to install
                if (fileName == "ADWCleaner.exe" || fileName == "remote.msi") 
                {
                    Process.Start(startLocation, installSwitch);
                } else
                {
                    /// Install Silently
                    var process = Process.Start(filePath, installSwitch);
                    process.WaitForExit();

                    /// Run App
                    Process.Start(startLocation);
                }

                return true;
            });

            return true;
        }
    }
}

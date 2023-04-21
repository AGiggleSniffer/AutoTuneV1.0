using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace WpfPoShGUI
{
    public partial class MainWindow : Window
    {
        // Initialize Main Window & Call Asset
        public MainWindow()
        {
            InitializeComponent();

            /// Call Hardware Info
            WriteAsset();
        }

        // Put hardware string into the correct box
        public void WriteAsset()
        {
            AssetOutput.Text = asset;
        }

        // Make the top grid clickable for moving the window
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // Close window
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // If Update tools are selected allow run of tools, if not disable run of tools
        public void CB4_Click(object sender, RoutedEventArgs e)
        {
            if (CB4.IsChecked == false)
            {
                CB5.IsEnabled = false;
                CB5.IsChecked = false;

                CB6.IsEnabled = false;
                CB6.IsChecked = false;

                CB7.IsEnabled = false;
                CB7.IsChecked = false;

                CB8.IsEnabled = false;
                CB8.IsChecked = false;
            }
            else if (CB4.IsChecked == true)
            {
                CB5.IsEnabled = true;
                CB6.IsEnabled = true;
                CB7.IsEnabled = true;
                CB8.IsEnabled = true;
            }
        }
        
        // Start Main Commands
        public async void StartBtn_Click(object sender, EventArgs e)
        {
            // Disable start button
            StartBtn.IsEnabled = false;

            // Check and run Checkboxes
            int amountOfCB = 0;
            CheckBox[] checkboxes = new CheckBox[] { CB1, CB2, CB3, CB5, CB6, CB7, CB8, CB9 };
            foreach (CheckBox c in checkboxes)
            {
                if (c.IsChecked == true)
                {
                    // Keep track of how many check boxes are selected
                    amountOfCB += 1;
                }
            }

            // Find percentage of 100 from how many checkboxes are selected
            double progVal = 0;
            try
            {
                double val = 100 / amountOfCB;
                progVal = Math.Ceiling(val);
            }
            catch
            {
            }

            // Start Dism / SFC
            if (CB1.IsChecked == true)
            {
                ScriptOutput.AppendText("\nStarting Dism/SFC\n");
                ProgressText.Text = "Starting Dism/SFC...";

                FileChecker();

                ProgressBar1.Value += progVal;
            }
            // Start Install of Support Tool
            if (CB3.IsChecked == true)
            {
                ScriptOutput.AppendText("\nDownloading rescue.msi...");
                ProgressText.Text = "Downloading rescue.msi...";

                var docUrl = "https://s3-us-west-2.amazonaws.com/nerdtools/remote.msi";
                var fileName = "remote.msi";
                var startLocation = @"C:\Users\Public\Downloads\remote.msi";
                var installSwitch = "/qn";

                await DownloadFile(docUrl, fileName, startLocation, installSwitch);

                ScriptOutput.AppendText("\nCalling Card Downloaded!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            // Start Download of ADW
            if (CB5.IsChecked == true)
            {
                ScriptOutput.AppendText("\nDownloading ADWCleaner...");
                ProgressText.Text = "Downloading ADWCleaner...";

                var docUrl = "https://adwcleaner.malwarebytes.com/adwcleaner?channel=release";
                var fileName = "ADWCleaner.exe";
                var startLocation = @"cmd.exe";
                var installSwitch = @"/k C:\Users\Public\Downloads\ADWCleaner.exe /eula /clean /noreboot";

                await DownloadFile(docUrl, fileName, startLocation, installSwitch);

                ScriptOutput.AppendText("\nADWCleaner Downloaded!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            // Start Install of Malwarebytes
            if (CB6.IsChecked == true)
            {
                ScriptOutput.AppendText("\nDownloading Malwarebytes...");
                ProgressText.Text = "Downloading Malwarebytes...";

                var docUrl = "https://www.malwarebytes.com/api/downloads/mb-windows?filename=MBSetup.exe";
                var fileName = "MBSetup.exe";
                var startLocation = @"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe";
                var installSwitch = "/verysilent /noreboot";

                await DownloadFile(docUrl, fileName, startLocation, installSwitch);

                ScriptOutput.AppendText("\nMalwarebytes Updated!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            // Start Install of GlarySoft
            if (CB7.IsChecked == true)
            {
                ScriptOutput.AppendText("\nDownloading Glary Utilities...");
                ProgressText.Text = "Downloading Glary Utilities...";

                var docUrl = "https://www.glarysoft.com/aff/download.php?s=GU";
                var fileName = "GUSetup.exe";
                var startLocation = @"C:\Program Files (x86)\Glary Utilities 5\OneClickMaintenance.exe";
                var installSwitch = "/S";

                await DownloadFile(docUrl, fileName, startLocation, installSwitch);

                ScriptOutput.AppendText("\nGlary Utilities Updated!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            // Start Install of CCleaner
            if (CB8.IsChecked == true)
            {
                ScriptOutput.AppendText("\nDownloading CCleaner...");
                ProgressText.Text = "Downloading CCleaner...";

                var docUrl = "https://bits.avcdn.net/productfamily_CCLEANER/insttype_FREE/platform_WIN_PIR/installertype_ONLINE/build_RELEASE";
                var fileName = "CCSetup.exe";
                var startLocation = @"C:\Program Files\CCleaner\CCleaner64.exe";
                var installSwitch = "/S";

                await DownloadFile(docUrl, fileName, startLocation, installSwitch);

                ScriptOutput.AppendText("\nCCleaner Updated!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            // Write UBlock Origin to Edge and Chrome registry
            if (CB9.IsChecked == true)
            {
                ScriptOutput.AppendText("\nAdding Ublock Origin...");
                ProgressText.Text = "Adding Ublock Origin...";

                await InstallUB();

                ScriptOutput.AppendText("\nInstalled Ublock Origin to Google Chrome and Microsoft Edge\nOpen Chrome and Edge to Finish\n");
                ProgressBar1.Value += progVal;
            }
            // Make NOC folder last for shortcuts
            if (CB2.IsChecked == true)
            {
                ScriptOutput.AppendText("\nMaking Nerds on Call Security Folder...");
                ProgressText.Text = "Making Nerds on Call Security Folder...";

                await MakeNOC();

                ScriptOutput.AppendText("\nNerds on Call Security Folder Made!\n");
                ProgressBar1.Value += progVal;
            }

            // Delete MB shortcut from installer
            if (System.IO.File.Exists(@"C:\Users\Public\Desktop\Malwarebytes.lnk"))
            {
                System.IO.File.Delete(@"C:\Users\Public\Desktop\Malwarebytes.lnk");
            }

            // Delete CC shortcut from installer
            if (System.IO.File.Exists(@"C:\Users\Public\Desktop\CCleaner.lnk"))
            {
                System.IO.File.Delete(@"C:\Users\Public\Desktop\CCleaner.lnk");
            }

            ScriptOutput.AppendText("\nScript Complete.\n");

            StartBtn.IsEnabled = true;
        }

        public void Progress_ProgressChanged(object sender, float progress)
        {
            // Do something with your progress
            ProgressBar2.Value = progress;
        }
    }
}
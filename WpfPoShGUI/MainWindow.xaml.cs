﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using static MainWindow;

namespace WpfPoShGUI
{
    public partial class MainWindow : Window
    {
        // Initialize Main Window & Call Asset
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                // Call Windows 11 API to Round Corners
                IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
                var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
                var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
                DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
            }
            catch
            {
            }

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

        // Minimize Window
        private void Minimize_Click(object sender, EventArgs e) 
        {
            this.WindowState = WindowState.Minimized;
        }

        // Copy Button
        private void CopyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Clipboard.SetText(asset);
            }
            catch 
            {
                ProgressText.Text = "Stop Pressing the Copy Button!";
            }
        }

        // If Update tools are selected allow run of tools, if not disable run of tools
        public void CB4_Click(object sender, RoutedEventArgs e)
        {
            if (CB4.IsChecked == false)
            {
                CB5.IsEnabled = false;
                CB5.IsChecked = false;
                CB5.Opacity = .1;

                CB6.IsEnabled = false;
                CB6.IsChecked = false;
                CB6.Opacity = .1;

                CB7.IsEnabled = false;
                CB7.IsChecked = false;
                CB7.Opacity = .1;

                CB8.IsEnabled = false;
                CB8.IsChecked = false;
                CB8.Opacity = .1;

                CB10.IsEnabled = false;
                CB10.IsChecked = false;
                CB10.Opacity = .1;
            }
            else if (CB4.IsChecked == true)
            {
                CB5.IsEnabled = true;
                CB5.Opacity = 1;

                CB6.IsEnabled = true;
                CB6.Opacity = 1;

                CB7.IsEnabled = true;
                CB7.Opacity = 1;

                CB8.IsEnabled = true;
                CB8.Opacity = 1;

                CB10.IsEnabled = true;
                CB10.Opacity = 1;
            }
        }
        
        // Start Main Commands
        public async void StartBtn_Click(object sender, EventArgs e)
        {
            // Disable start button
            StartBtn.IsEnabled = false;

            // Reset Progress Bar
            ProgressBar1.Value = 0;
            ProgressBar2.Value = 0;

            // Check how many Checkboxes
            int amountOfCB = 0;
            System.Windows.Controls.CheckBox[] checkboxes = new System.Windows.Controls.CheckBox[] { CB1, CB2, CB3, CB5, CB6, CB7, CB8, CB9 };
            foreach (System.Windows.Controls.CheckBox c in checkboxes)
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
                ScriptOutput.AppendText("\nNo Checkboxes Selected.\n");
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
                var docUrl = "https://s3-us-west-2.amazonaws.com/nerdtools/remote.msi";
                var fileName = "remote.msi";
                var startLocation = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                var installSwitch = "/qn";

                var result = await DownloadFile(docUrl, fileName, startLocation, installSwitch);

                if (result == true)
                {
                    ScriptOutput.AppendText("\n" + fileName + " Updated!\nOpening...\n");
                }
                else
                {
                    ScriptOutput.AppendText("\nError During Install.\nCheck if Installers are Present.\n");
                }

                ProgressBar1.Value += progVal;
            }
            // Start Download of ADW
            if (CB5.IsChecked == true)
            {
                var docUrl = "https://adwcleaner.malwarebytes.com/adwcleaner?channel=release";
                var fileName = "ADWCleaner.exe";
                var startLocation = @"cmd.exe";
                var installSwitch = @"/k " + Path.Combine(Directory.GetCurrentDirectory(), fileName) + " /eula /clean /noreboot";

                var result = await DownloadFile(docUrl, fileName, startLocation, installSwitch);

                if (result == true)
                {
                    ScriptOutput.AppendText("\n" + fileName + " Updated!\nOpening...\n");
                }
                else
                {
                    ScriptOutput.AppendText("\nError During Install.\nCheck if Installers are Present.\n");
                }

                ProgressBar1.Value += progVal;
            }
            // Start Install of Malwarebytes
            if (CB6.IsChecked == true)
            {
                var docUrl = "https://www.malwarebytes.com/api/downloads/mb-windows?filename=MBSetup.exe";
                var fileName = "MBSetup.exe";
                var startLocation = @"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe";
                var installSwitch = "/verysilent /noreboot";

                var result = await DownloadFile(docUrl, fileName, startLocation, installSwitch);

                if (result == true)
                {
                    ScriptOutput.AppendText("\n" + fileName + " Updated!\nOpening...\n");
                }
                else
                {
                    ScriptOutput.AppendText("\nError During Install.\nCheck if Installers are Present.\n");
                }

                ProgressBar1.Value += progVal;
            }
            // Start Install of GlarySoft
            if (CB7.IsChecked == true)
            {
                var docUrl = "https://www.glarysoft.com/aff/download.php?s=GU";
                var fileName = "GUSetup.exe";
                var startLocation = @"C:\Program Files (x86)\Glary Utilities 5\OneClickMaintenance.exe";
                var installSwitch = "/S";

                var result = await DownloadFile(docUrl, fileName, startLocation, installSwitch);

                if (result == true)
                {
                    ScriptOutput.AppendText("\n" + fileName + " Updated!\nOpening...\n");
                }
                else
                {
                    ScriptOutput.AppendText("\nError During Install.\nCheck if Installers are Present.\n");
                }

                ProgressBar1.Value += progVal;
            }
            // Start Install of CCleaner
            if (CB8.IsChecked == true)
            {
                var docUrl = "https://bits.avcdn.net/productfamily_CCLEANER/insttype_FREE/platform_WIN_PIR/installertype_ONLINE/build_RELEASE";
                var fileName = "CCSetup.exe";
                var startLocation = @"C:\Program Files\CCleaner\CCleaner64.exe";
                var installSwitch = "/S";

                var result = await DownloadFile(docUrl, fileName, startLocation, installSwitch);

                if (result == true)
                {
                    ScriptOutput.AppendText("\n" + fileName + " Updated!\nOpening...\n");
                }
                else
                {
                    ScriptOutput.AppendText("\nError During Install.\nCheck if Installers are Present.\n");
                }
                
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
            
            ProgressText.Text = "Done!";
            ProgressBar1.Value = 100; 

            StartBtn.IsEnabled = true;
        }

        public void Progress_ProgressChanged(object sender, float progress)
        {
            // Do something with your progress
            ProgressBar2.Value = progress;
        }
    }
}
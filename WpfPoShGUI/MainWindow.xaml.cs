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

        // Start main functions on button press
        public async void StartBtn_Click(object sender, EventArgs e)
        {
            /// Disable start button 
            /// The way the checkboxes are written into a string cause errors on a second start, 
            /// but this prevents other issues as well
            StartBtn.IsEnabled = false;

            // Initialize progress bar value
            ProgressBar1.Value = 0;

            /// Check state of Checkboes and dump into string
            /// Only dump into string to assign values after checking state
            /// preventing a second for each statement to loop mass if statements,
            /// since progress bar values need to be assigned after checking how many checboxes are selected
            string selectedToppings = string.Empty;
            int amountOfCB = 0;
            CheckBox[] checkboxes = new CheckBox[] { CB1, CB2, CB3, CB5, CB6, CB7, CB8, CB9 };
            foreach (CheckBox c in checkboxes)
            {
                if (c.IsChecked == true)
                {
                    string Topping = (string)c.Name;
                    selectedToppings += Topping + " ";
                    /// Keep track of how many check boxes are selected
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

            // Start Processes based on what user selected and assign progressbar values
            if (selectedToppings.Contains("CB1"))
            {
                ScriptOutput.AppendText("\nStarting Dism/SFC\n");
                FileChecker();
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB3"))
            {
                ScriptOutput.AppendText("\nDownloading rescue.msi...");
                await RS();
                ScriptOutput.AppendText("\nCalling Card Repair Downloaded!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB5"))
            {
                ScriptOutput.AppendText("\nDownloading ADWCleaner...");
                await ADW();
                ScriptOutput.AppendText("\nADWCleaner Downloaded!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB6"))
            {
                ScriptOutput.AppendText("\nDownloading Malwarebytes...");
                await MB();
                ScriptOutput.AppendText("\nMalwarebytes Updated!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB7"))
            {
                ScriptOutput.AppendText("\nDownloading Glary Utilities...");
                await GU();
                ScriptOutput.AppendText("\nGlary Utilities Updated!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB8"))
            {
                ScriptOutput.AppendText("\nDownloading CCleaner...");
                await CC();
                ProgressBar1.Value += progVal;
                ScriptOutput.AppendText("\nCCleaner Updated!\nOpening...\n");
            }
            if (selectedToppings.Contains("CB9"))
            {
                ScriptOutput.AppendText("\nAdding Ublock Origin...");
                await InstallUB();
                ScriptOutput.AppendText("\nInstalled Ublock Origin to Google Chrome and Microsoft Edge\nOpen Chrome and Edge to Finish\n");
                ProgressBar1.Value += progVal;
            }
            /// Make NOC folder last for shortcuts
            if (selectedToppings.Contains("CB2"))
            {
                ScriptOutput.AppendText("\nMaking Nerds on Call Security Folder...");
                await MakeNOC();
                ScriptOutput.AppendText("\nNerds on Call Security Folder Made!\n");
                ProgressBar1.Value += progVal;
            }          

            // Start Progress Bar
            Duration duration = new Duration(TimeSpan.FromSeconds(60));
            DoubleAnimation doubleanimation = new DoubleAnimation(ProgressBar1.Value, duration);
            ProgressBar1.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);

            ScriptOutput.AppendText("\nScript Complete.\n");

            // Re enable start button and clear progress bar string
            selectedToppings = string.Empty;
            StartBtn.IsEnabled = true;
        }
    }
}
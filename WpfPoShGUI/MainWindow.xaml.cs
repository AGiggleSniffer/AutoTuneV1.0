using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading;
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
using System.Security.Cryptography;
using System.Windows.Media.Animation;

namespace WpfPoShGUI
{
    public partial class MainWindow : Window
    {
        /// Auto Tune BOI
        /// No one was ready for this
        /// Because no one asked for this
        /// but it saves me a lot of time, so: f the haters
        public MainWindow()
        {
            InitializeComponent();
            /// Call Hardware Info
            WriteAsset();
        }

        /// Make the top grid clickable for moving the window
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// Close window
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// Put hardware string into the correct box
        public void WriteAsset()
        { 
            AssetOutput.Text = asset; 
        }

        /// If Update tools are selected allow run of tools, if not disable run of tools
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
    }
    public partial class MainWindow : Window
    {
        /// Start main functions on button press
        public async void StartBtn_Click(object sender, EventArgs e)
        {

            /// Disable start button 
            /// The way the checkboxes are written into a string cause errors on a second start, 
            /// but this prevents other issues as well
            StartBtn.IsEnabled = false;

            /// Initialize progress bar value
            ProgressBar1.Value = 0;

            /// Check state of Checkboes and dump into string
            /// Only dump into string to assign values after checking state
            /// preventing a second for each statement to loop mass if statements
            /// since progress bar values need to be assigned after checking how many checboxes are selected
            string selectedToppings = string.Empty;
            int amountOfCB = 0;
            CheckBox[] checkboxes = new CheckBox[] { CB1, CB2, CB3, CB5, CB6, CB7, CB8, CB10 };
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

            /// Find percentage of 100 from how many checkboxes are selected
            int progVal = 0;
            try
            {
                progVal = 100 / amountOfCB;
            }
            catch
            {
            }    

            /// Start Processes based on what user selected and assign progressbar values
            if (selectedToppings.Contains("CB1"))
            {
                FileChecker();
                ScriptOutput.AppendText("Starting DISM/SFC ...");
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB2"))
            {
                MakeNOC();
                ScriptOutput.AppendText("Nerds on Call Security Folder Made!");
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB3"))
            {
                await RS();
                ScriptOutput.AppendText("\nCalling Card Repair Downloaded!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB5"))
            {
                await ADW();
                ScriptOutput.AppendText("\nADWCleaner Downloaded!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB6"))
            {
                await MB();
                ScriptOutput.AppendText("\nMalwarebytes Updated!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB7"))
            {
                await GU();
                ScriptOutput.AppendText("\nGlary Utilities Updated!\nOpening...\n");
                ProgressBar1.Value += progVal;
            }
            if (selectedToppings.Contains("CB8"))
            {
                await CC();
                ProgressBar1.Value += progVal;
                ScriptOutput.AppendText("\nCCleaner Updated!\nOpening...\n");
            }
            if (selectedToppings.Contains("CB10"))
            {
                InstallUB();
                ProgressBar1.Value += progVal;
            }

            /// Start Progress Bar
            Duration duration = new Duration(TimeSpan.FromSeconds(60));
            DoubleAnimation doubleanimation = new DoubleAnimation(ProgressBar1.Value, duration);
            ProgressBar1.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Reflection;

namespace WpfPoShGUI
{
    public partial class MainWindow : Window
    {
        static readonly HttpClient client = new HttpClient();

        /// Async Tasks to Download, Install, Then run target program
        public static async Task ADW()
        {
            var url = "https://adwcleaner.malwarebytes.com/adwcleaner?channel=release";
            var route = @"C:\Users\Public\Downloads";

            /// Start Download
            var stream = await client.GetStreamAsync(url);
            using (var fileStream = System.IO.File.Create(Path.Combine(route, "ADWCleaner.exe")))
            {
                stream.CopyTo(fileStream);
            }

            /// Run App
            /// Start cmd so itll stay open 
            /// (Temp until i can figure out redirecting output or just reading log file),
            /// scan, clean infection, do not reboot after
            Process.Start(@"cmd.exe", @"/k C:\Users\Public\Downloads\ADWCleaner.exe /eula /clean /noreboot");
        }
        public static async Task CC()
        {
            var url = "https://bits.avcdn.net/productfamily_CCLEANER/insttype_FREE/platform_WIN_PIR/installertype_ONLINE/build_RELEASE";
            var route = @"C:\Users\Public\Downloads";

            /// Download
            var stream = await client.GetStreamAsync(url);
            using (var fileStream = System.IO.File.Create(Path.Combine(route, "CCSetup.exe")))
            {
                stream.CopyTo(fileStream);
            }

            /// Install Silently
            var process = Process.Start(@"C:\Users\Public\Downloads\CCSetup.exe", "/S");
            process.WaitForExit();

            /// Run App
            Process.Start(@"C:\Program Files\CCleaner\CCleaner64.exe");
        }
        public static async Task MB()
        {
            var url = "https://ninite.com/malwarebytes/ninite.exe";
            var route = @"C:\Users\Public\Downloads";

            /// Download
            var stream = await client.GetStreamAsync(url);
            using (var fileStream = System.IO.File.Create(Path.Combine(route, "MBSetup.exe")))
            {
                stream.CopyTo(fileStream);
            }

            ///Install
            var process = Process.Start(@"C:\Users\Public\Downloads\MBSetup.exe", "/silent");
            process.WaitForExit();

            /// Run App
            Process.Start(@"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe");
        }
        public static async Task GU()
        {
            var url = "https://www.glarysoft.com/aff/download.php?s=GU";
            var route = @"C:\Users\Public\Downloads";

            /// Download
            var stream = await client.GetStreamAsync(url);
            using (var fileStream = System.IO.File.Create(Path.Combine(route, "GUSetup.exe")))
            {
                stream.CopyTo(fileStream);
            }

            /// Install
            var process = Process.Start(@"C:\Users\Public\Downloads\GUSetup.exe", "/S");
            process.WaitForExit();

            /// Run App
            Process.Start(@"C:\Program Files (x86)\Glary Utilities 5\OneClickMaintenance.exe");
        }
        public static async Task RS()
        {
            var url = "https://github.com/AGiggleSniffer/Installers-for-Autotune/archive/refs/heads/main.zip";
            var route = @"C:\Users\Public\Downloads";

            /// Download ZIP
            var stream = await client.GetStreamAsync(url);
            using (var fileStream = System.IO.File.Create(Path.Combine(route, "RSCallingCard.zip")))
            {
                stream.CopyTo(fileStream);
            }

            /// Extract ZIP
            string startPath = @"C:\Users\Public\Downloads";
            string zipPath = @"C:\Users\Public\Downloads\RSCallingCard.zip";
            string extractPath = @"C:\Users\Public\Downloads\RSCallingCard.exe";
            ZipFile.CreateFromDirectory(startPath, zipPath);
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            /// Run App
            Process.Start(@"C:\Users\Public\Downloads\RSCallingCard.exe");
        }

        /// Start DISM/SFC
        public static void FileChecker()
        {
            Process.Start(@"cmd.exe", @"/k dism /online /cleanup-image /restorehealth&sfc /scannow");
            /// Redirect output to ScriptOutput?
        }

        /// Make NOC Folder
        public void MakeNOC()
        {
            string dir = @"C:\Users\Public\Desktop\Nerds On Call 800-919NERD";
            // If directory does not exist, create it
            if (!Directory.Exists(dir))
            {
                DirectoryInfo folder = Directory.CreateDirectory(dir);


                using (StreamWriter sw = new StreamWriter(@"C:\Users\Public\Desktop\Nerds On Call 800-919NERD\desktop.ini"))
                {
                    sw.WriteLine("[.ShellClassInfo]");
                    sw.WriteLine("ConfirmFileOp=0");
                    sw.WriteLine("IconFile=nerd.ico");
                    sw.WriteLine("IconIndex=0");
                    sw.WriteLine("InfoTip=Contains the Nerds On Call Security Suite");
                    sw.Close();
                }

                folder.Attributes |= FileAttributes.System;
            }
        }

        /// Adds Reg keys so next time Chrome or Edge opens, it updates or asks to install UBlock Origin
        public void InstallUB()
        {
            /// Write to Google Chrome
            string key = @"HKLM:\Software\Wow6432Node\Google\Chrome\Extensions\cjpalhdlnbpafiamejdnhcphjbkeiagm";
            string valueName = "update_url"; // "(Default)" value
            string value = "https://clients2.google.com/service/update2/crx";
            Microsoft.Win32.Registry.SetValue(key, valueName, value, Microsoft.Win32.RegistryValueKind.String);
            AssetOutput.AppendText("\nGoogle Chrome Updated!\nOpen Chrome to Finish\n");

            /// Write to MS Edge
            key = @"HKLM:\Software\Wow6432Node\Google\Chrome\Extensions\cjpalhdlnbpafiamejdnhcphjbkeiagm";
            value = "https://clients2.google.com/service/update2/crx";
            Microsoft.Win32.Registry.SetValue(key, valueName, value, Microsoft.Win32.RegistryValueKind.String);
            AssetOutput.AppendText("\nMicrosoft Edge Updated!\nOpen Edge to Finish\n");
        }
    }
}

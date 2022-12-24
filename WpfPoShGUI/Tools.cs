using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using Microsoft.Win32;

namespace WpfPoShGUI
{
    public partial class MainWindow : Window
    {
        static readonly HttpClient client = new HttpClient();

        /// Async Tasks to Download, Install, Then run target program
        public static async Task<bool> ADW()
        {
            var route = @"C:\Users\Public\Downloads";
            var url = "https://adwcleaner.malwarebytes.com/adwcleaner?channel=release";

            /// Start Download
            var stream = await client.GetStreamAsync(url);
            using (var fileStream = File.Create(Path.Combine(route, "ADWCleaner.exe")))
            {
                stream.CopyTo(fileStream);
            }

            /// Run App
            /// Start cmd so itll stay open 
            /// (Temp until i can figure out redirecting output or just reading log file),
            /// scan, clean infection, do not reboot after
            Process.Start(@"cmd.exe", @"/k C:\Users\Public\Downloads\ADWCleaner.exe /eula /clean /noreboot");
           
            return true;
        }
        public static async Task<bool> CC()
        {
            var url = "https://bits.avcdn.net/productfamily_CCLEANER/insttype_FREE/platform_WIN_PIR/installertype_ONLINE/build_RELEASE";
            var route = @"C:\Users\Public\Downloads";

            /// Download
            var stream = await client.GetStreamAsync(url);
            using (var fileStream = File.Create(Path.Combine(route, "CCSetup.exe")))
            {
                stream.CopyTo(fileStream);
            }

            var install = await Task<bool>.Run(() => 
            {
                /// Install Silently
                var process = Process.Start(@"C:\Users\Public\Downloads\CCSetup.exe", "/S");
                process.WaitForExit();

                /// Run App
                Process.Start(@"C:\Program Files\CCleaner\CCleaner64.exe");

                return true;
            });
            return true;
        }
        public static async Task<bool> MB()
        {
            var url = "https://www.malwarebytes.com/api/downloads/mb-windows?filename=MBSetup.exe";
            var route = @"C:\Users\Public\Downloads";

            /// Download
            var stream = await client.GetStreamAsync(url);
            using (var fileStream = File.Create(Path.Combine(route, "MBSetup.exe")))
            {
                stream.CopyTo(fileStream);
            }
           
            var install = await Task<bool>.Run(() => 
            {
                /// Install Silently, dont reboot!
                var process = Process.Start(@"C:\Users\Public\Downloads\MBSetup.exe", "/verysilent /noreboot");
                process.WaitForExit();

                Process.Start(@"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe");

                return true;
            });
            return true;
        }
        public static async Task<bool> GU()
        {
            var url = "https://www.glarysoft.com/aff/download.php?s=GU";
            var route = @"C:\Users\Public\Downloads";

            /// Download
            var stream = await client.GetStreamAsync(url);
            using (var fileStream = File.Create(Path.Combine(route, "GUSetup.exe")))
            {
                stream.CopyTo(fileStream);
            }

            var install = await Task<bool>.Run(() => 
            {
                /// Install Silently
                var process = Process.Start(@"C:\Users\Public\Downloads\GUSetup.exe", "/S");
                process.WaitForExit();

                /// Run App
                Process.Start(@"C:\Program Files (x86)\Glary Utilities 5\OneClickMaintenance.exe");

                return true;
            });
            return true;
        }
        public static async Task<bool> RS()
        {
            /// Test if File Already Exists
            if (!Directory.Exists(@"C:\Users\Public\Downloads\Installers-for-Autotune-main"))
            {
                var url = "https://github.com/AGiggleSniffer/Installers-for-Autotune/archive/refs/heads/main.zip";
                var route = @"C:\Users\Public\Downloads";

                /// Download ZIP
                var stream = await client.GetStreamAsync(url);
                using (var fileStream = File.Create(Path.Combine(route, "RSCallingCard.zip")))
                {
                    stream.CopyTo(fileStream);
                }
            }

            var extract = await Task<bool>.Run(() => 
            {
                if (!Directory.Exists(@"C:\Users\Public\Downloads\Installers-for-Autotune-main"))
                {
                    /// Extract ZIP
                    string zipPath = @"C:\Users\Public\Downloads\RSCallingCard.zip";
                    string extractPath = @"C:\Users\Public\Downloads";
                    ZipFile.ExtractToDirectory(zipPath, extractPath);
                    File.Delete(zipPath);
                }

                return true;
            });
            return true;
        }

        /// Start DISM/SFC
        public static void FileChecker()
        {
            Process.Start(@"cmd.exe", @"/k dism /online /cleanup-image /restorehealth&sfc /scannow");
            /// Redirect output to ScriptOutput?
        }

        /// Make NOC Folder
        public async Task<bool> MakeNOC()
        {
            string dir = @"C:\Users\Public\Desktop\Nerds On Call 800-919NERD";
            /// If directory does not exist, create it
            if (!Directory.Exists(dir))
            {
                DirectoryInfo folder = Directory.CreateDirectory(dir);

                /// Create desktop.ini file
                string deskIni = @"C:\Users\Public\Desktop\Nerds On Call 800-919NERD\desktop.ini";
                using (StreamWriter sw = new StreamWriter(deskIni))
                {
                    sw.WriteLine("[.ShellClassInfo]");
                    sw.WriteLine("ConfirmFileOp=0");
                    sw.WriteLine("IconFile=nerd.ico");
                    sw.WriteLine("IconIndex=0");
                    sw.WriteLine("InfoTip=Contains the Nerds On Call Security Suite");
                    sw.Close();
                }

                /// Check to see if Nerds icon exists, If not then download it
                string nerdsIco = @"C:\Users\Public\Downloads\Installers-for-Autotune-main\Noc_Downloads\nerd.ico";
                string place = @"C:\Users\Public\Desktop\Nerds On Call 800-919NERD\nerd.ico";
                if ((!Directory.Exists(nerdsIco)))
                {
                    /// Download Resource folder
                    await RS();
                }

                /// Copy Nerds icon then set Attributes
                File.Copy(nerdsIco, place);
                File.SetAttributes(place, FileAttributes.Hidden);

                /// Hide icon and desktop.ini then set folder as a system folder
                File.SetAttributes(deskIni, FileAttributes.Hidden);
                folder.Attributes |= FileAttributes.System;
                folder.Attributes |= FileAttributes.ReadOnly;
                folder.Attributes |= FileAttributes.Directory;
            }

            return true;
        }

        /// Adds Registry keys so next time Chrome or Edge opens, it updates or asks to install UBlock Origin
        public static async Task<bool> InstallUB()
        {

            string valueName = "update_url";

            var GC = await Task.Run<bool>(() =>
            {
                /// Write to Google Chrome
                string value = "https://clients2.google.com/service/update2/crx";
                string key = @"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Google\Chrome\Extensions\cjpalhdlnbpafiamejdnhcphjbkeiagm";
                Registry.SetValue(key, valueName, value, RegistryValueKind.String);
                return true;
            });

            var edge = await Task.Run<bool>(() => 
            {
                /// Write to MS Edge
                string eValue = "https://edge.microsoft.com/extensionwebstorebase/v1/crx";
                string eKey = @"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Microsoft\Edge\Extensions\odfafepnkmbhccpbejgmiehpchacaeak";
                Registry.SetValue(eKey, valueName, eValue, RegistryValueKind.String);
                return true;
            });

            return true;
        }
    }
}

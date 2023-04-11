using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using Microsoft.Win32;
using System;
using IWshRuntimeLibrary;

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
            using (var fileStream = System.IO.File.Create(Path.Combine(route, "ADWCleaner.exe")))
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
            using (var fileStream = System.IO.File.Create(Path.Combine(route, "CCSetup.exe")))
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
            using (var fileStream = System.IO.File.Create(Path.Combine(route, "MBSetup.exe")))
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
            using (var fileStream = System.IO.File.Create(Path.Combine(route, "GUSetup.exe")))
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
            var url = "https://s3-us-west-2.amazonaws.com/nerdtools/remote.msi";
            var route = @"C:\Users\Public\Downloads";

                /// Download ZIP
            var stream = await client.GetStreamAsync(url);
            using (var fileStream = System.IO.File.Create(Path.Combine(route, "remote.msi")))
            {
                stream.CopyTo(fileStream);
            }

            var install = await Task<bool>.Run(() =>
            {
                /// Install Silently
                var process = Process.Start(@"C:\Users\Public\Downloads\remote.msi", "/qn");
                process.WaitForExit();

                /// Run App
                Process.Start(@"C:\Users\Public\Downloads\remote.msi");

                return true;
            });

            return true;
        }

        /// Download Resources
        public static async Task<bool> Rez()
        {
            /// Test if File Already Exists
            if (!Directory.Exists(@"C:\Users\Public\Downloads\rez.zip"))
            {
                var url = "https://github.com/AGiggleSniffer/Installers-for-Autotune/archive/refs/heads/main.zip";
                var route = @"C:\Users\Public\Downloads";

                /// Download ZIP
                var stream = await client.GetStreamAsync(url);
                using (var fileStream = System.IO.File.Create(Path.Combine(route, "rez.zip")))
                {
                    stream.CopyTo(fileStream);
                }

                var extract = await Task<bool>.Run(() =>
                {
                    if (!Directory.Exists(@"C:\Users\Public\Downloads\Installers-for-Autotune-main"))
                    {
                        /// Extract ZIP
                        string zipPath = @"C:\Users\Public\Downloads\rez.zip";
                        string extractPath = @"C:\Users\Public\Downloads";
                        ZipFile.ExtractToDirectory(zipPath, extractPath);
                        System.IO.File.Delete(zipPath);
                    }

                    return true;
                });
            }
            return true;
        }

        /// Start DISM/SFC
        public static void FileChecker()
        {
            
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/k dism /online /cleanup-image /restorehealth&sfc /scannow";
            process.StartInfo = startInfo;
            process.Start();
        }

        /// Create Shortcuts
        public static void Shortcut(string shortcutName, string targetFileLocation)
        {
            // Initialize shortcuts
            string shortcutLocation = Path.Combine(@"C:\Users\Public\Desktop\Nerds On Call 800-919NERD", shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.TargetPath = targetFileLocation;           // The path of the file that will launch when the shortcut is run
            shortcut.Save();
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
                    await Rez();
                }

                /// Copy Nerds icon then set Attributes
                System.IO.File.Copy(nerdsIco, place);
                System.IO.File.SetAttributes(place, FileAttributes.Hidden);

                /// Hide icon and desktop.ini then set folder as a system folder
                System.IO.File.SetAttributes(deskIni, FileAttributes.Hidden);
                folder.Attributes |= FileAttributes.System;
                folder.Attributes |= FileAttributes.ReadOnly;
                folder.Attributes |= FileAttributes.Directory;
            }

            // Create Shortcutsss

            // MB Shortcut
            if (System.IO.File.Exists(@"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe"))
            {
                Shortcut("Malwarebytes", @"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe");
            }
            // Delete MB shortcut from installer
            if (System.IO.File.Exists(@"C:\Users\Public\Desktop\Malwarebytes.lnk"))
            {
                System.IO.File.Delete(@"C:\Users\Public\Desktop\Malwarebytes.lnk");
            }

            // CC Shortcut
            if (System.IO.File.Exists(@"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe"))
            {
            Shortcut("CCleaner", @"C:\Program Files\CCleaner\CCleaner64.exe");
            }
            // Delete CC shortcut from installer
            if (System.IO.File.Exists(@"C:\Users\Public\Desktop\CCleaner.lnk"))
            {
                System.IO.File.Delete(@"C:\Users\Public\Desktop\CCleaner.lnk");
            }

            // GU Shortcut
            if (System.IO.File.Exists(@"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe"))
            {
                Shortcut("Glary Utilities", @"C:\Program Files (x86)\Glary Utilities 5\Integrator.exe");
            }


            // ADW Shortcut
            if (System.IO.File.Exists(@"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe"))
            {
                Shortcut("ADW Cleaner", @"C:\Users\Public\Downloads\ADWCleaner.exe");
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

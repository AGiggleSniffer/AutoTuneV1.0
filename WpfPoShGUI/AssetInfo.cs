using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Management;

namespace WpfPoShGUI
{
    public partial class MainWindow : Window
    {
        public static class HardwareInfo
        {
            ///
            /// Retrieving HDD Size & Model
            /// 
            /// 
            public static string GetDriveSize()
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");

                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {
                        return (Convert.ToInt64(wmi.GetPropertyValue("Size")) / 1073741824).ToString() + "GB" + 
                            "\n            Brand: " + (string)wmi["Model"];
                    }

                    catch { }

                }

                return "Size: Unknown";
            }
            ///
            /// Retrieving HDD Info
            /// 
            /// 
            public static string GetDriveInfo()
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk");

                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {
                        return (string)wmi["FileSystem"];
                    }

                    catch { }

                }

                return "Disk: Unknown";
            }
            ///
            /// Retrieving GPU Name
            /// 
            /// 
            public static string GetGPUInfo()
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");

                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {
                        return (string)wmi["Name"];
                    }

                    catch { }

                }

                return "GPU: Unknown";
            }
            ///
            /// Retrieving System MAC Address.
            /// 
            /// 
            public static string GetMACAddress()
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                string MACAddress = String.Empty;
                foreach (ManagementObject mo in moc)
                {
                    if (MACAddress == String.Empty)
                    {
                        if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                    }
                    mo.Dispose();
                }

                return MACAddress;
            }
            ///
            /// Retrieving Motherboard Manufacturer.
            /// 
            /// 
            public static string GetBoardMaker()
            {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {
                        return wmi.GetPropertyValue("Manufacturer").ToString();
                    }

                    catch { }

                }

                return "Board Maker: Unknown";

            }
            ///
            /// Retrieving Motherboard Product Id.
            /// 
            /// 
            public static string GetBoardProductId()
            {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {
                        return wmi.GetPropertyValue("Product").ToString();

                    }

                    catch { }

                }

                return "Product: Unknown";

            }
            ///
            /// Retrieving BIOS Maker.
            /// 
            /// 
            public static string GetBIOSmaker()
            {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {
                        return wmi.GetPropertyValue("Manufacturer").ToString();

                    }

                    catch { }

                }

                return "BIOS Maker: Unknown";

            }
            ///
            /// Retrieving BIOS Serial No.
            /// 
            /// 
            public static string GetBIOSserNo()
            {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {
                        return wmi.GetPropertyValue("SerialNumber").ToString();

                    }

                    catch { }

                }

                return "BIOS Serial Number: Unknown";

            }
            ///
            /// Retrieving BIOS Caption.
            /// 
            /// 
            public static string GetBIOScaption()
            {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {
                        return wmi.GetPropertyValue("Caption").ToString();

                    }
                    catch { }
                }
                return "BIOS Caption: Unknown";
            }
            ///
            /// Retrieving System Account Name.
            /// 
            /// 
            public static string GetAccountName()
            {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {

                        return wmi.GetPropertyValue("Name").ToString();
                    }
                    catch { }
                }
                return "User Account Name: Unknown";

            }
            ///
            /// Retrieving Physical Ram Memory.
            /// 
            /// 
            public static string GetPhysicalMemory()
            {
                ManagementScope oMs = new ManagementScope();
                ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
                ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
                ManagementObjectCollection oCollection = oSearcher.Get();

                long MemSize = 0;
                long mCap = 0;

                // In case more than one Memory sticks are installed
                foreach (ManagementObject obj in oCollection)
                {
                    mCap = Convert.ToInt64(obj["Capacity"]);
                    MemSize += mCap;
                }
                MemSize = (MemSize / 1073741824);
                return MemSize.ToString() + "GB";
            }
            ///
            /// Retrieving No of Ram Slot on Motherboard.
            /// 
            /// 
            public static string GetNoRamSlots()
            {

                int MemSlots = 0;
                ManagementScope oMs = new ManagementScope();
                ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
                ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
                ManagementObjectCollection oCollection2 = oSearcher2.Get();
                foreach (ManagementObject obj in oCollection2)
                {
                    MemSlots = Convert.ToInt32(obj["MemoryDevices"]);

                }
                return MemSlots.ToString();
            }
            ///
            /// method for retrieving the CPU Manufacturer
            /// using the WMI class
            /// 
            /// CPU Manufacturer
            public static string GetCPUManufacturer()
            {
                string cpuMan = String.Empty;
                //create an instance of the Managemnet class with the
                //Win32_Processor class
                ManagementClass mgmt = new ManagementClass("Win32_Processor");
                //create a ManagementObjectCollection to loop through
                ManagementObjectCollection objCol = mgmt.GetInstances();
                //start our loop for all processors found
                foreach (ManagementObject obj in objCol)
                {
                    if (cpuMan == String.Empty)
                    {
                        // only return manufacturer from first CPU
                        cpuMan = obj.Properties["Manufacturer"].Value.ToString();
                    }
                }
                return cpuMan;
            }
            ///
            /// method to retrieve the network adapters
            /// default IP gateway using WMI
            /// 
            /// adapters default IP gateway
            public static string GetDefaultIPGateway()
            {
                //create out management class object using the
                //Win32_NetworkAdapterConfiguration class to get the attributes
                //of the network adapter
                ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
                //create our ManagementObjectCollection to get the attributes with
                ManagementObjectCollection objCol = mgmt.GetInstances();
                string gateway = String.Empty;
                //loop through all the objects we find
                foreach (ManagementObject obj in objCol)
                {
                    if (gateway == String.Empty)  // only return MAC Address from first card
                    {
                        //grab the value from the first network adapter we find
                        //you can change the string to an array and get all
                        //network adapters found as well
                        //check to see if the adapter's IPEnabled
                        //equals true
                        if ((bool)obj["IPEnabled"] == true)
                        {
                            gateway = obj["DefaultIPGateway"].ToString();
                        }
                    }
                    //dispose of our object
                    obj.Dispose();
                }
                //replace the ":" with an empty space, this could also
                //be removed if you wish
                gateway = gateway.Replace(":", "");
                //return the mac address
                return gateway;
            }
            ///
            /// Retrieve CPU Speed.
            /// 
            /// 
            public static double? GetCpuSpeedInGHz()
            {
                double? GHz = null;
                using (ManagementClass mc = new ManagementClass("Win32_Processor"))
                {
                    foreach (ManagementObject mo in mc.GetInstances())
                    {
                        GHz = 0.001 * (UInt32)mo.Properties["CurrentClockSpeed"].Value;
                        break;
                    }
                }
                return GHz;
            }
            ///
            /// Retrieving Current Language.
            /// 
            /// 
            public static string GetOSInformation()
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {
                        return ((string)wmi["Caption"]).Trim() + "\n            " + (string)wmi["Version"] + "\n            " + (string)wmi["OSArchitecture"];
                    }
                    catch { }
                }
                return "OS Maker: Unknown";
            }
            ///
            /// Retrieving Processor Information.
            /// 
            /// 
            public static string GetProcessorInformation()
            {
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();
                String info = String.Empty;
                foreach (ManagementObject mo in moc)
                {
                    string name = (string)mo["Name"];
                    name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");

                    info = name;
                    //mo.Properties["Name"].Value.ToString();
                    //break;
                }
                return info;
            }
            ///
            /// Retrieving SystemSKU.
            /// 
            /// 
            public static string GetSystemSKU()
            {
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                String info = String.Empty;
                foreach (ManagementObject mo in moc)
                {
                    info = (string)mo["SystemSKUNumber"];
                    //mo.Properties["Name"].Value.ToString();
                    //break;
                }
                return info;
            }

        }

        /// Tidy up hardware info into a string
        public string asset = $@"
            Host Name
        --------------------------------------
            {Environment.MachineName}
            {HardwareInfo.GetAccountName()}

            
            Operating System
        --------------------------------------
            {HardwareInfo.GetOSInformation()}

            
            Bios/Make & Model
        --------------------------------------
            {HardwareInfo.GetBoardMaker()} 
            {HardwareInfo.GetBIOSmaker()}
            {HardwareInfo.GetBoardProductId()}
            {HardwareInfo.GetBIOScaption()}


            Serial Number
        --------------------------------------
            Bios: {HardwareInfo.GetBIOSserNo()}
            SKU: {HardwareInfo.GetSystemSKU()}

            
            Memory
        --------------------------------------
            Sticks: {HardwareInfo.GetNoRamSlots()}
            Total: {HardwareInfo.GetPhysicalMemory()}

            
            CPU
        --------------------------------------
            {HardwareInfo.GetCPUManufacturer()}
            {HardwareInfo.GetProcessorInformation()}
            Clock Speed: {HardwareInfo.GetCpuSpeedInGHz()}GHz


            GPU
        --------------------------------------
            {HardwareInfo.GetGPUInfo()}


            DiskInfo
        --------------------------------------
            Size: {HardwareInfo.GetDriveSize()}
            Format: {HardwareInfo.GetDriveInfo()}

           
            Mac Address
        --------------------------------------
            {HardwareInfo.GetMACAddress()}

        ";
    }
}
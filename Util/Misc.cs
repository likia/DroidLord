using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord.Util
{
    public class Misc
    {
        static public string HardwareID()
        {
            string HID = "";
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter");
            foreach (ManagementObject obj in Searcher.Get())
            {
                // PCI总线上的网卡的地址
                string MAC = (string)obj["MACAddress"];
                string PNPID = (string)obj["PNPDeviceID"];
                if (MAC != null && PNPID != null && PNPID.IndexOf("PCI") > -1)
                {
                    HID += "\"" + MAC + "\", ";
                }
            }

            ///* That's for DiskDrive*/
            //ManagementClass cimobject = new ManagementClass("Win32_DiskDrive");
            //ManagementObjectCollection moc = cimobject.GetInstances();
            //foreach (ManagementObject mo in moc)
            //{
            //    string HDid = (string)mo.Properties["Model"].Value;
            //    HID += HDid + "|";
            //}
            HID += "CPUID|" + GetCpuID();
            return Program.SHA256.Hash("HardwareID<" + HID + ",HASH = " + Program.MD5.Hash(HID) + ">");
        }
        public static String GetCpuID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return "";
            }
        }
    }
}
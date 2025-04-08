using Microsoft.Win32;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Steam_Authenticator.Internal
{
    internal static class Helper
    {
        public static string GetMachineGuid()
        {
            const string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography";
            const string value = "MachineGuid";
            var result = Registry.GetValue(key, value, string.Empty)?.ToString();

            return result;
        }

        public static string GetMachineUniqueId()
        {
            string result = $"{GetCpuId()}-{GetBiosId()}-{GetBaseId()}-{GetDiskId()}-{GetVideoId()}-{GetMacId()}";
            result = GetHash(result);
            return result;
        }

        public static string GetCpuId()
        {
            string retVal = Identifier("Win32_Processor", "UniqueId");
            if (retVal == "")
            {
                retVal = Identifier("Win32_Processor", "ProcessorId");
                if (retVal == "")
                {
                    retVal = Identifier("Win32_Processor", "Name");
                    if (retVal == "")
                    {
                        retVal = Identifier("Win32_Processor", "Manufacturer");
                    }
                    retVal += Identifier("Win32_Processor", "MaxClockSpeed");
                }
            }
            return retVal;
        }

        public static string GetBiosId()
        {
            return Identifier("Win32_BIOS", "Manufacturer")
            + Identifier("Win32_BIOS", "SMBIOSBIOSVersion")
            + Identifier("Win32_BIOS", "IdentificationCode")
            + Identifier("Win32_BIOS", "SerialNumber")
            + Identifier("Win32_BIOS", "ReleaseDate")
            + Identifier("Win32_BIOS", "Version");
        }

        public static string GetDiskId()
        {
            return Identifier("Win32_DiskDrive", "Model")
            + Identifier("Win32_DiskDrive", "Manufacturer")
            + Identifier("Win32_DiskDrive", "Signature")
            + Identifier("Win32_DiskDrive", "TotalHeads");
        }

        public static string GetBaseId()
        {
            return Identifier("Win32_BaseBoard", "Model")
            + Identifier("Win32_BaseBoard", "Manufacturer")
            + Identifier("Win32_BaseBoard", "Name")
            + Identifier("Win32_BaseBoard", "SerialNumber");
        }

        public static string GetVideoId()
        {
            return Identifier("Win32_VideoController", "DriverVersion")
            + Identifier("Win32_VideoController", "Name");
        }

        public static string GetMacId()
        {
            return Identifier("Win32_NetworkAdapterConfiguration",
                "MACAddress", "IPEnabled");
        }

        private static string GetHash(string s)
        {
            MD5 sec = new MD5CryptoServiceProvider();
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] bt = enc.GetBytes(s);
            return GetHexString(sec.ComputeHash(bt));
        }

        private static string GetHexString(byte[] bt)
        {
            string s = string.Empty;
            for (int i = 0; i < bt.Length; i++)
            {
                byte b = bt[i];
                int n, n1, n2;
                n = (int)b;
                n1 = n & 15;
                n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char)(n2 - 10 + (int)'A')).ToString();
                else
                    s += n2.ToString();
                if (n1 > 9)
                    s += ((char)(n1 - 10 + (int)'A')).ToString();
                else
                    s += n1.ToString();
                if ((i + 1) != bt.Length && (i + 1) % 2 == 0) s += "-";
            }
            return s;
        }

        private static string Identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            ManagementClass mc = new ManagementClass(wmiClass);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() == "True")
                {
                    //Only get the first one
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return result;
        }

        private static string Identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            ManagementClass mc = new ManagementClass(wmiClass);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                result = mo[wmiProperty]?.ToString();
                if (!string.IsNullOrWhiteSpace(result))
                {
                    break;
                }
            }
            return result;
        }
    }
}

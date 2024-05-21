using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security;

namespace OS_Informer
{
    public interface IWin32_Base
    {
        void Load(string name, object value);
        string ToString();
    }

    public class Win32_OperatingSystem : IWin32_Base
    {
        private const decimal _1024 = 1024 * 1024;

        public string BuildNumber;
        public string CSName;
        public string Caption;
        public UInt64 FreePhysicalMemory;
        public UInt64 FreeVirtualMemory;
        public string OSArchitecture;
        public string SystemDrive;
        public UInt64 TotalVirtualMemorySize;
        public UInt64 TotalVisibleMemorySize;
        public string Version;

        public decimal _FreePhysicalMemoryGB { get { return Math.Round((FreePhysicalMemory / _1024), 2); } }
        public decimal _FreeVirtualMemoryGB { get { return Math.Round((FreeVirtualMemory / _1024), 2); } }
        public decimal _TotalVirtualMemorySizeGB { get { return Math.Round((TotalVirtualMemorySize / _1024), 2); } }
        public decimal _TotalVisibleMemorySizeGB { get { return Math.Round((TotalVisibleMemorySize / _1024), 2); } }
        public string _OSVersion
        {
            get
            {
                return (String.IsNullOrEmpty(Version) || String.IsNullOrEmpty(BuildNumber)) ? String.Empty :
                    String.Format("{0} Build {1}", Version, BuildNumber);
            }
        }

        public Win32_OperatingSystem() { }

        public void Load(string name, object value)
        {
            try
            {
                this.GetType().GetField(name).SetValue(this, value);
            }
            catch (Exception ex)
            {

            }
        }

        public override string ToString()
        {
            var ser = JsonConvert.SerializeObject(this, Formatting.Indented);

            return ser;
        }
    }

    public class Win32_LogicalDisk : IWin32_Base
    {
        public enum enmDriveType { DEFAULT = -1, Unknown = 0, NoRootDirectory, RemovableDisk, LocalDisk, NetworkDrive, CompactDisc, RAMDisc }

        private const decimal _1024 = 1024 * 1024 * 1024;

        public string Caption;
        public string Description;
        public UInt64 FreeSpace;
        public UInt64 Size;
        public bool Compressed;
        public UInt32 DriveType;
        public string FileSystem;
        public string VolumeSerialNumber;

        public decimal _SizeGB { get { return Math.Round((Size / _1024), 2); } }
        public decimal _FreeSpaceGB { get { return Math.Round((FreeSpace / _1024), 2); } }
        public string _DriveType
        {
            get
            {
                enmDriveType tmpVal = enmDriveType.DEFAULT;

                if (Enum.TryParse(DriveType.ToString(), out tmpVal))
                    return tmpVal.ToString();

                return "N/A";
            }
        }

        public Win32_LogicalDisk() { }

        public void Load(string name, object value)
        {
            try
            {
                this.GetType().GetField(name).SetValue(this, value);
            }
            catch (Exception ex)
            {

            }
        }

        public override string ToString()
        {
            var ser = JsonConvert.SerializeObject(this, Formatting.Indented);

            return ser;
        }
    }

    public partial class TCimSession_Machine
    {
        private static HashSet<string> m_Props_Win32_OperatingSystem = new HashSet<string>() {
            "BuildNumber", "CSName", "Caption", "FreePhysicalMemory", "FreeVirtualMemory",
            "OSArchitecture", "SystemDrive", "TotalVirtualMemorySize", "TotalVisibleMemorySize", "Version" };

        private static HashSet<string> m_Props_Win32_LogicalDisk = new HashSet<string>() {
            "Caption", "Description", "FreeSpace", "Size", "Compressed",
            "DriveType", "FileSystem", "VolumeSerialNumber" };

        public void Get_REMOTE_SysDetails(int timeout = 30)
        {
            string computer = "172.16.0.203";
            string username = "adminavanza";
            string password = "uPgY1t50ar";

            string root_Machine_Path = String.Format(@"\\{0}\root\cimv2", computer);
            string wql_getOSDetail = @"select * from Win32_OperatingSystem";
            string wql_getDiskDetail = @"select * from Win32_LogicalDisk";

            DComSessionOptions DComOptions = new DComSessionOptions();
            DComOptions.Impersonation = ImpersonationType.Impersonate;
            DComOptions.PacketIntegrity = true;
            DComOptions.PacketPrivacy = true;
            DComOptions.Timeout = TimeSpan.FromSeconds(timeout);

            SecureString theSecureString = new NetworkCredential(username, password, computer).SecurePassword;
            CimCredential creden = new CimCredential(PasswordAuthenticationMechanism.Default, computer, username, theSecureString);
            DComOptions.AddDestinationCredentials(creden);

            Win32_OperatingSystem obj_OS = null;
            List<Win32_LogicalDisk> obj_Disks = null;

            using (var remoteSession = CimSession.Create(computer, DComOptions))
            {
                var results1 = remoteSession.QueryInstances(root_Machine_Path, "WQL", wql_getOSDetail);
                obj_OS = (Win32_OperatingSystem)getResult(results1);

                var results2 = remoteSession.QueryInstances(root_Machine_Path, "WQL", wql_getDiskDetail);
                obj_Disks = (List<Win32_LogicalDisk>)getResult(results2, true);
            }
        }

        public void Get_LOCAL_SysDetails()
        {
            string root_Machine_Path = @"root\cimv2";
            string wql_getOSDetail = @"select * from Win32_OperatingSystem";
            string wql_getDiskDetail = @"select * from Win32_LogicalDisk";


            Win32_OperatingSystem obj_OS = null;
            List<Win32_LogicalDisk> obj_Disks = null;

            using (var session = CimSession.Create(null))
            {
                var results1 = session.QueryInstances(root_Machine_Path, "WQL", wql_getOSDetail);
                obj_OS = (Win32_OperatingSystem)getResult(results1);


                var results2 = session.QueryInstances(root_Machine_Path, "WQL", wql_getDiskDetail);
                obj_Disks = (List<Win32_LogicalDisk>)getResult(results2, true);
            }
        }

        private object getResult(IEnumerable<CimInstance> results, bool isGettingDiskDetail = false)
        {
            List<Win32_LogicalDisk> tmpDisks = isGettingDiskDetail ? new List<Win32_LogicalDisk>() : null;
            IWin32_Base tmpObj = null;

            foreach (CimInstance result in results)
            {
                tmpObj = (isGettingDiskDetail ? (IWin32_Base)new Win32_LogicalDisk() : new Win32_OperatingSystem());
                setValues(tmpObj, tmpDisks, result, isGettingDiskDetail);
            }

            return isGettingDiskDetail ? (object)tmpDisks : tmpObj;
        }

        private void setValues(IWin32_Base tmpObj, List<Win32_LogicalDisk> tmpDisks, CimInstance result, bool isGettingDiskDetail = false)
        {
            Console.WriteLine("ClassName : [{0}]", result.CimSystemProperties.ClassName);
            var enmItm = result.CimInstanceProperties.GetEnumerator();

            while (enmItm.MoveNext())
            {
                if ((!isGettingDiskDetail && m_Props_Win32_OperatingSystem.Contains(enmItm.Current.Name)) || (isGettingDiskDetail && m_Props_Win32_LogicalDisk.Contains(enmItm.Current.Name)))
                {
                    tmpObj.Load(enmItm.Current.Name, enmItm.Current.Value);
                }
            }

            Console.WriteLine(tmpObj.ToString());

            if (isGettingDiskDetail)
            {
                tmpDisks.Add((Win32_LogicalDisk)tmpObj);
            }

            Console.WriteLine(String.Empty);
        }
    }
}

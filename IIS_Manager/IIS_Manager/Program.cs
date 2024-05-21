using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;

namespace IIS_Manager
{
    public enum eStates
    {
        Start = 2,
        Stop = 4,
        Pause = 6,
    }

    public partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Main Server"); Console.WriteLine(String.Empty);

            try
            {
                do
                {
                    TCimSession.Get_REMOTE_IIS_Ops("1");
                    TCimSession.Get_REMOTE_IIS_Ops("1");

                    continue;
                    TCimSession.Get_REMOTE_IIS();
                    TManagementScope.Get_REMOTE_IIS();
                    TManagementScope.Get_REMOTE_Details();
                    TCimSession.Get_LOCAL_IIS();
                } while (false);
            }
            catch (CimException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine(String.Empty);
            Console.WriteLine("Complete Main");
            Console.ReadLine();
        }

        static void IIS(string txtServer, string txtUserID, string txtPassword)
        {
            ConnectionOptions connectionOptions = new ConnectionOptions();
            eStates state = eStates.Start;
            string site = "1";


            if (txtUserID.Length > 0)
            {
                connectionOptions.Username = txtUserID;
                connectionOptions.Password = txtPassword;
            }
            else
            {
                connectionOptions.Impersonation = ImpersonationLevel.Impersonate;
            }

            ManagementScope managementScope =
                new ManagementScope(@"\\" + txtServer + @"\root\microsoftiisv2", connectionOptions);

            managementScope.Connect();
            if (managementScope.IsConnected == false)
            {
                //getMessage("Could not connect to WMI namespace " + managementScope.Path, "Connect Failed");
            }
            else
            {
                SelectQuery selectQuery =
                    new SelectQuery("Select * From IIsWebServer Where Name = 'W3SVC/" + site + "'");
                using (ManagementObjectSearcher managementObjectSearcher =
                        new ManagementObjectSearcher(managementScope, selectQuery))
                {
                    foreach (ManagementObject objMgmt in managementObjectSearcher.Get())
                        objMgmt.InvokeMethod(state.ToString(), new object[0]);
                }
            }
        }
    }

    public class TCimSession
    {
        public static void Get_REMOTE_IIS()
        {
            string computer = "172.16.0.203";
            string username = "adminavanza";
            string password = "uPgY1t50ar";

            string site = "1";
            string root_IIS_Path = String.Format(@"\\{0}\root\MicrosoftIISv2", computer);

            DComSessionOptions DComOptions = new DComSessionOptions();
            DComOptions.Impersonation = ImpersonationType.Impersonate;
            DComOptions.PacketIntegrity = true;
            DComOptions.PacketPrivacy = true;
            DComOptions.Timeout = TimeSpan.FromSeconds(10);

            SecureString theSecureString = new NetworkCredential(username, password, computer).SecurePassword;
            CimCredential creden = new CimCredential(PasswordAuthenticationMechanism.Default, computer, username, theSecureString);
            DComOptions.AddDestinationCredentials(creden);


            using (var remoteSession = CimSession.Create(computer, DComOptions))
            {

                // "winmgmts://MyMachine/root/MicrosoftIISv2").ExecQuery("select * from CIM_Setting" ) 

                //String.Format(@"select * from IIsWebServer Where Name = 'W3SVC/{0}'", site));

                var results = remoteSession.QueryInstances(root_IIS_Path,
                    "WQL",
                    @"select * from CIM_Setting");
                //String.Format(@"select Name,AppIsolated,ServerState from IIsWebServer Where Name = 'W3SVC/{0}'", site));
                /*
                 
  string Caption;
  string Description;
  string SettingID;

                using (ManagementObjectSearcher managementObjectSearcher =
                            new ManagementObjectSearcher(managementScope, selectQuery))
                    {
                        foreach (ManagementObject objMgmt in managementObjectSearcher.Get())
                            objMgmt.InvokeMethod(state.ToString(), new object[0]);
                    }
                 */

                StringBuilder sb = new StringBuilder();

                foreach (var result in results)
                {
                    sb.AppendLine(String.Format("CimInstanceProperties ClassName:{0}", result.CimSystemProperties.ClassName));

                    switch (result.CimSystemProperties.ClassName)
                    {
                        case "IIsWebServiceSetting":
                            break;

                        case "IIsWebServerSetting":

                            Console.WriteLine("IIsWebServerSetting Name : {0}", result.CimInstanceProperties["Name"].Value);
                            Console.WriteLine("IIsWebServerSetting ServerComment : {0}", result.CimInstanceProperties["ServerComment"].Value);
                            Console.WriteLine(String.Empty);
                            break;

                        case "IIsWebVirtualDirSetting":
                            break;
                    }
                }
            }
        }

        public static void Get_REMOTE_IIS_Ops(string siteId)
        {
            string computer = "172.16.0.203";
            string username = "adminavanza";
            string password = "uPgY1t50ar";

            string root_IIS_Path = String.Format(@"\\{0}\root\MicrosoftIISv2", computer);

            DComSessionOptions DComOptions = new DComSessionOptions();
            DComOptions.Impersonation = ImpersonationType.Impersonate;
            DComOptions.PacketIntegrity = true;
            DComOptions.PacketPrivacy = true;
            DComOptions.Timeout = TimeSpan.FromSeconds(10);

            SecureString theSecureString = new NetworkCredential(username, password, computer).SecurePassword;
            CimCredential creden = new CimCredential(PasswordAuthenticationMechanism.Default, computer, username, theSecureString);
            DComOptions.AddDestinationCredentials(creden);


            using (var remoteSession = CimSession.Create(computer, DComOptions))
            {
                var results = remoteSession.QueryInstances(root_IIS_Path,
                    "WQL",
                    //@"select * from IIsWebServer");
                    String.Format(@"select * from IIsWebServer Where Name = 'W3SVC/{0}'", siteId));

                //remoteSession.TestConnection();

                foreach (CimInstance result in results)
                {
                    string tmp_State = result.CimInstanceProperties["ServerState"].Value?.ToString();
                    eStates curr_state = (eStates)Enum.Parse(typeof(eStates), tmp_State);
                    eStates new_State = eStates.Start;

                    if (curr_state == eStates.Start)
                        new_State = eStates.Stop;

                    Console.WriteLine("IIsWebServer Name : {0}", result.CimInstanceProperties["Name"].Value);
                    Console.WriteLine("IIsWebServer ServerState : {0}", result.CimInstanceProperties["ServerState"].Value);
                    Console.WriteLine("IIsWebServer ServerState : {0}", curr_state);
                    Console.WriteLine("IIsWebServer Operation : {0} ...", new_State);

                    var innn = remoteSession.InvokeMethod(result, new_State.ToString(), null);

                    Console.WriteLine("IIsWebServer ServerState : {0}", new_State);
                }



                //-------------------

                //ConnectionOptions options = new ConnectionOptions("MS_409", username, theSecureString,
                //    "ntlmdomain:" + computer,
                //    ImpersonationLevel.Anonymous,
                //    AuthenticationLevel.Default, true,
                //    null, TimeSpan.FromSeconds(3));

                //ManagementScope scope = new ManagementScope(root_IIS_Path, options);
                //ObjectQuery query = new ObjectQuery("SELECT * FROM IIsWebServer");
                //ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                //ManagementObjectCollection queryCollection = searcher.Get();

                //foreach (ManagementObject objMgmt in searcher.Get())
                //{
                //    objMgmt.InvokeMethod(eStates.Stop.ToString(), new object[0]);
                //}
            }
        }

        public static void Get_LOCAL_IIS()
        {
            using (var session = CimSession.Create(null))
            {
                string site = "1";
                eStates state = eStates.Start;

                // "winmgmts://MyMachine/root/MicrosoftIISv2").ExecQuery("select * from CIM_Setting" ) 

                var results2 = session.QueryInstances(@"root\MicrosoftIISv2",
                    "WQL",
                    String.Format(@"select * from IIsWebServer Where Name = 'W3SVC/{0}'", site));

                foreach (var result in results2)
                {
                    Console.WriteLine("Process name: {0}",
                    result.CimInstanceProperties["Name"].Value);

                    /*
        ManagementScope managementScope =
            new ManagementScope(@"\\" + txtServer + @"\root\microsoftiisv2", connectionOptions);

                    SelectQuery selectQuery =
                        new SelectQuery("Select * From IIsWebServer Where Name = 'W3SVC/" + site + "'");
                    using (ManagementObjectSearcher managementObjectSearcher =
                            new ManagementObjectSearcher(managementScope, selectQuery))
                    {
                        foreach (ManagementObject objMgmt in managementObjectSearcher.Get())
                            objMgmt.InvokeMethod(state.ToString(), new object[0]);
                    }
                    */
                }

                //@"select * from CIM_Setting);
                ;
                string root_IIS_Path = @"root\MicrosoftIISv2";
                var results3 = session.QueryInstances(root_IIS_Path,
                    "WQL",
                    @"select * from CIM_Setting");

                StringBuilder sb = new StringBuilder();

                foreach (var result in results3)
                {
                    sb.AppendLine(String.Format("CimInstanceProperties ClassName:{0}", result.CimSystemProperties.ClassName));

                    switch (result.CimSystemProperties.ClassName)
                    {
                        case "IIsWebServiceSetting":
                            break;

                        case "IIsWebServerSetting":

                            Console.WriteLine("IIsWebServerSetting Name : {0}", result.CimInstanceProperties["Name"].Value);
                            Console.WriteLine("IIsWebServerSetting ServerComment : {0}", result.CimInstanceProperties["ServerComment"].Value);
                            Console.WriteLine(String.Empty);
                            break;

                        case "IIsWebVirtualDirSetting":
                            break;
                    }
                }

                var itmSB = sb.ToString();
            }
        }
    }

    public class TManagementScope
    {
        public static void Get_REMOTE_Details()
        {
            string siteID = "1";
            string computer = "172.16.0.203";
            string username = "adminavanza";
            string password = "uPgY1t50ar";

            string root_IIS_Path = String.Format(@"\\{0}\root\cimv2", computer);

            SecureString theSecureString = new NetworkCredential(username, password, computer).SecurePassword;
            ConnectionOptions options = new ConnectionOptions("MS_409", username, theSecureString,
                "ntlmdomain:" + computer,
                ImpersonationLevel.Impersonate,
                AuthenticationLevel.Default, true,
                null, TimeSpan.FromSeconds(3));


            ManagementScope scope = new ManagementScope(root_IIS_Path, options);

            Console.WriteLine("[{0}] ManagementScope Connecting...", DateTime.Now);
            try
            {
                scope.Connect();
                Console.WriteLine("Connected!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection Failed!");
            }
            finally
            {
                Console.WriteLine("[{0}] ManagementScope Finished !!!", DateTime.Now);
            }

            if (scope.IsConnected)
            {
                //SelectQuery selectQuery = new SelectQuery("Select * From IIsWebServer Where Name = 'W3SVC/" + siteID + "'");
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection queryCollection = searcher.Get();

                foreach (ManagementObject m in queryCollection)
                {
                    //m.InvokeMethod(eStates.Start.ToString(), new object[0]);

                    // Display the remote computer information
                    Console.WriteLine("Computer Name : {0}", m["csname"]);
                    Console.WriteLine("Windows Directory : {0}", m["WindowsDirectory"]);
                    Console.WriteLine("Operating System: {0}", m["Caption"]);
                    Console.WriteLine("Version: {0}", m["Version"]);
                    Console.WriteLine("Manufacturer : {0}", m["Manufacturer"]);
                }
            }
        }

        public static void Get_REMOTE_IIS()
        {
            string siteID = "1";
            string computer = "172.16.0.203";
            string username = "adminavanza";
            string password = "uPgY1t50ar";

            string root_IIS_Path = String.Format(@"\\{0}\root\MicrosoftIISv2", computer);

            SecureString theSecureString = new NetworkCredential(username, password, computer).SecurePassword;
            ConnectionOptions options = new ConnectionOptions("MS_409", username, theSecureString,
                "ntlmdomain:" + computer,
                ImpersonationLevel.Anonymous,
                AuthenticationLevel.Default, true,
                null, TimeSpan.FromSeconds(3));


            ManagementScope scope = new ManagementScope(root_IIS_Path, options);

            Console.WriteLine("[{0}] ManagementScope Connecting...", DateTime.Now);
            try
            {
                scope.Connect();
                Console.WriteLine("Connected!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection Failed!");
            }
            finally
            {
                Console.WriteLine("[{0}] ManagementScope Finished !!!", DateTime.Now);
            }

            if (scope.IsConnected)
            {
                //SelectQuery selectQuery = new SelectQuery("Select * From IIsWebServer Where Name = 'W3SVC/" + siteID + "'");
                ObjectQuery query = new ObjectQuery("SELECT * FROM IIsWebServer");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection queryCollection = searcher.Get();

                foreach (ManagementObject m in queryCollection)
                {
                    //m.InvokeMethod(eStates.Start.ToString(), new object[0]);

                    // Display the remote computer information
                    Console.WriteLine("Computer Name : {0}", m["csname"]);
                    Console.WriteLine("Windows Directory : {0}", m["WindowsDirectory"]);
                    Console.WriteLine("Operating System: {0}", m["Caption"]);
                    Console.WriteLine("Version: {0}", m["Version"]);
                    Console.WriteLine("Manufacturer : {0}", m["Manufacturer"]);
                }
            }
        }
    }
}

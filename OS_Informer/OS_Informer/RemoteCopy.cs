using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace OS_Informer
{
    public partial class RemoteNodeCopier : IDisposable
    {
        #region Members

        private static WindowsImpersonationContext impersonationContext;

        private static string DOMAIN;
        private static string DOMAINUSER;
        private static string DOMAINPASSWORD;
        private static string m_Src_Dir;
        private static string m_Des_Dir;

        private static string m_Installer = null;
        #endregion


        #region Constructor

        static RemoteNodeCopier()
        {
            setLocalDir();
            //setRemoteDir();
        }
        #endregion


        #region Properties

        public string Dest_Installer
        {
            get
            {
                if (m_Installer == null)
                {
                    FileInfo fi = new FileInfo(Path.Combine(m_Des_Dir, "Installer.bat"));

                    if (File.Exists(fi.FullName))
                        m_Installer = fi.FullName;
                }

                return m_Installer;
            }
        }
        #endregion


        #region Private Methods

        private static void setLocalDir()
        {
            DOMAIN = @"AS-PD-RAZA-S";
            DOMAINUSER = @"muhammad.raza";
            DOMAINPASSWORD = @"Google@012345";

            m_Src_Dir = String.Format(@"\\127.0.0.1\C$\Avanza\RemoteCopy.Testing\Src\");
            m_Des_Dir = String.Format(@"\\127.0.0.1\C$\Avanza.Remote\RemoteCopy.Testing\Des\");
        }

        private static void setRemoteDir()
        {
            DOMAIN = @"172.16.11.61";
            DOMAINUSER = @"adminavanza";
            DOMAINPASSWORD = @"uPgY1t50ar";

            m_Src_Dir = String.Format(@"\\127.0.0.1\C$\Avanza\RemoteCopy.Testing\Src\");
            m_Des_Dir = String.Format(@"\\{0}\C$\Avanza\RemoteCopy.Testing\Des\", DOMAIN);
        }

        private static void UndoImpersonation()
        {
            if (impersonationContext != null)
                impersonationContext.Undo();
        }

        private static bool ImpersonateValidUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUser(userName, domain, password, (int)dwLogonType.LOGON32_LOGON_NEW_CREDENTIALS, (int)dwLogonProvider.LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();

                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }

            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);

            return false;
        }

        private Process getProcess()
        {
            var process = new Process();

            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                FileName = "cmd.exe",
                Arguments = "/c \"\"" + Dest_Installer + "\"\"",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.StartInfo = startInfo;

            return process;
        }

        #endregion


        #region Public Methods

        public void CopyFiles()
        {
            if (ImpersonateValidUser(DOMAINUSER, DOMAIN, DOMAINPASSWORD))
            {
                try
                {
                    Console.WriteLine("Copying files...");

                    string[] src_Files = Directory.GetFiles(m_Src_Dir);

                    if (!Directory.Exists(m_Des_Dir))
                    {
                        Directory.CreateDirectory(m_Des_Dir);
                    }


                    Console.WriteLine("Source : [{0}]", m_Src_Dir);
                    Console.WriteLine("Target : [{0}]", m_Des_Dir);
                    Console.WriteLine(String.Empty);

                    FileInfo fi = null;
                    foreach (string src_File in src_Files)
                    {
                        fi = new FileInfo(src_File);
                        string des_File = Path.Combine(m_Des_Dir, fi.Name);


                        Console.Write("[{0}] copying... ", fi.Name);

                        File.Copy(src_File, des_File, true);

                        Console.WriteLine("success!");
                    }

                    Console.WriteLine("Copying files completed!");
                }
                catch (Exception se)
                {
                    int ret = Marshal.GetLastWin32Error();
                    Console.WriteLine(ret.ToString(), "Error code: " + ret.ToString());
                    Console.WriteLine(se.Message);
                }
                finally
                {
                    UndoImpersonation();
                }
            }
            else
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                //throw new Exception("Impersonation failed.");
            }
        }

        public void Run()
        {
            Process process = this.getProcess();

            if (process.Start())
            {
                Console.Clear();
                Console.WriteLine("###  SHOWING REMOTE CONSOLE OUTPUT  ###");

                string response;
                do
                {
                    response = process.StandardOutput.ReadLine();
                    Console.WriteLine(response);
                }
                while (!response.Equals("PRESS ENTER"));

                process.WaitForExit(30000);
            }
        }
        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            UndoImpersonation();
        }

        #endregion
    }
}
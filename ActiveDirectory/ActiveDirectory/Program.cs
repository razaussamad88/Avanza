
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace ActiveDirectory
{

    public static class HelperClass
    {
        public static string GetFullMessageForLog(this Exception ex)
        {
            return ex.InnerException == null
                 ? ex.Message
                 : ex.Message + " --> " + ex.InnerException.GetFullMessageForLog();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            string loginId = "rey0000";
            string pwd = "Bbk_1234";
            string domain = "KUWT.BBK.BH";
            //string domain = "bbkmaster";

            Console.Write("Do you want to change it to Bahrain Domain [y/n] n:");

            if (Console.ReadKey().KeyChar.ToString().ToLower()[0].Equals('y'))
            {
                domain = "bbkmaster";
            }

            Console.WriteLine();

            Console.WriteLine(String.Format("Domain: [{0}] | User: [{1}] | Pass: [{2}]", domain, loginId, pwd));

            //ProcessActiveDirectoryUserLogin("rey0000", "Bbk_1234", "bbkmaster");
            //ProcessActiveDirectoryUserLogin2(loginId, pwd, domain);


            UserValid(loginId, pwd, domain, false);

            //GetDomainUser(domain, loginId, pwd);

            //GetDomainUser2(domain, loginId, pwd);

            Console.WriteLine("Completed!!");

            Console.Read();
        }

        public static void GetFields(SearchResult sr)
        {
            foreach (System.Collections.IEnumerable itm in sr.Properties.PropertyNames)
            {
                Console.WriteLine(String.Format("Field: [{0}] ; Value: [{1}]", itm.ToString(), sr.Properties[itm.ToString()][0]));
            }
        }

        public static void RecordDetail(string DomainPath, string Login_ID)
        {
            DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);
            DirectorySearcher search = new DirectorySearcher(searchRoot);
            search.Filter = string.Format("(&(objectClass=user)(objectCategory=person)(samaccountname={0}))", Login_ID);
            search.PropertiesToLoad.Add("samaccountname");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("usergroup");
            search.PropertiesToLoad.Add("displayname");

            SearchResult result;
            SearchResultCollection resultCol = search.FindAll();

            if (resultCol != null)
            {
                for (int counter = 0; counter < resultCol.Count; counter++)
                {
                    string UserNameEmailString = string.Empty;
                    result = resultCol[counter];

                    if (result.Properties["mail"] != null && result.Properties["mail"].Count > 0)
                    {
                        Console.WriteLine(String.Format("{0}: {1}", "EMAIL_ADDRESS_TEMP", (String)result.Properties["mail"][0]));
                    }
                    if (result.Properties["samaccountname"] != null && result.Properties["samaccountname"].Count > 0)
                    {
                        Console.WriteLine(String.Format("{0}: {1}", "LOGIN_ID", (String)result.Properties["samaccountname"][0]));
                    }
                    if (result.Properties["displayname"] != null && result.Properties["displayname"].Count > 0)
                    {
                        Console.WriteLine(String.Format("{0}: {1}", "FULL_NAME", (String)result.Properties["displayname"][0]));
                    }
                }
            }
        }

        public static void ProcessActiveDirectoryUserLogin(string loginId, string password, string domain)
        {
            string DomainPath = "LDAP://" + domain;
            string msg = "";

            using (DirectoryEntry entry = new DirectoryEntry(DomainPath))
            {
                //entry.Username = loginId;
                entry.Username = domain + "\\" + loginId;
                entry.Password = password;

                DirectorySearcher searcher = new DirectorySearcher(entry);

                searcher.Filter = "(objectclass=user)";
                //searcher.Filter = "(sAMAccountName=user)";
                //searcher.PropertiesToLoad.Add("memberOf");

                try
                {
                    SearchResult sr = searcher.FindOne();

                    if (sr == null)
                        msg = "No record found";
                    else
                    {
                        msg = "Some record found";
                        //GetFields(sr);
                        RecordDetail(DomainPath, loginId);
                    }
                }
                catch (COMException ex)
                {
                    if (ex.ErrorCode == -2147023570)
                    {
                        msg = "AD User Id or password is incorrect";
                    }
                    else if (ex.ErrorCode == -2147016646)
                    {
                        msg = "AD server is not operational.";
                    }
                    else
                    {
                        msg = ex.Message;
                    }

                    Console.WriteLine(String.Format("{0} : {1}", MethodBase.GetCurrentMethod(), ex.GetFullMessageForLog()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("{0} : {1}", MethodBase.GetCurrentMethod(), ex.GetFullMessageForLog()));
                }
            }

            Console.WriteLine(msg);
        }


        public static void ProcessActiveDirectoryUserLogin2(string loginId, string password, string domain)
        {
            string DomainPath = "LDAP://" + domain;
            string msg = "";

            using (DirectoryEntry entry = new DirectoryEntry(DomainPath))
            {
                //entry.Username = loginId;
                entry.Username = domain + "\\" + loginId;
                entry.Password = password;

                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.ClientTimeout = TimeSpan.FromSeconds(20);
                searcher.ServerTimeLimit = TimeSpan.FromSeconds(20);
                searcher.CacheResults = true;

                searcher.Filter = "(objectclass=user)";
                //searcher.Filter = "(sAMAccountName=user)";
                //searcher.PropertiesToLoad.Add("memberOf");

                try
                {
                    Console.WriteLine(DateTime.Now.ToString() + " Finding...");
                    SearchResult sr = searcher.FindOne();

                    Console.WriteLine(DateTime.Now.ToString() + " Responding...");
                    if (sr == null)
                        msg = "No record found";
                    else
                    {
                        msg = "Some record found";
                        //GetFields(sr);
                        RecordDetail(DomainPath, loginId);
                    }
                }
                catch (COMException ex)
                {
                    if (ex.ErrorCode == -2147023570)
                    {
                        msg = "AD User Id or password is incorrect";
                    }
                    else if (ex.ErrorCode == -2147016646)
                    {
                        msg = "AD server is not operational.";
                    }
                    else
                    {
                        msg = ex.Message;
                    }

                    Console.WriteLine(DateTime.Now.ToString() + String.Format(" {0} : {1}", MethodBase.GetCurrentMethod(), ex.GetFullMessageForLog()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(DateTime.Now.ToString() + String.Format(" {0} : {1}", MethodBase.GetCurrentMethod(), ex.GetFullMessageForLog()));
                }
            }

            Console.WriteLine(msg);
        }



        public static bool UserValid(string username, string password, string DomainName, bool useSSL)
        {
            bool userAuthenticated = false;
            var domainName = DomainName;


            if (useSSL)
            {
                domainName = domainName + ":636";
            }

            try
            {
                int sec = 3;
                using (var ldap = new LdapConnection(domainName))
                {
                    var networkCredential = new NetworkCredential(username, password, DomainName); // Uses DomainName without the ":636" at all times, SSL or not.
                    ldap.SessionOptions.VerifyServerCertificate += VerifyServerCertificate;
                    ldap.SessionOptions.SecureSocketLayer = useSSL;
                    //ldap.AuthType = AuthType.Negotiate;

                    ldap.AuthType = AuthType.Ntlm;
                    ldap.SessionOptions.ProtocolVersion = 3;
                    ldap.Timeout = TimeSpan.FromSeconds(sec);
                    ldap.SessionOptions.SendTimeout = TimeSpan.FromSeconds(sec);
                    ldap.SessionOptions.TcpKeepAlive = false;
                    ldap.SessionOptions.PingWaitTimeout = TimeSpan.FromSeconds(sec);
                    ldap.SessionOptions.PingKeepAliveTimeout = TimeSpan.FromSeconds((sec + 2) >= 5 ? (sec + 2) : 5);
                    ldap.SessionOptions.AutoReconnect = false;

                    Console.WriteLine(DateTime.Now.ToString() + " Requesting...");
                    ldap.Bind(networkCredential);
                }

                // If the bind succeeds, we have a valid user/pass.
                userAuthenticated = true;
            }
            catch (LdapException ldapEx)
            {
                // Error Code 0x31 signifies invalid credentials, so return userAuthenticated as false.
                if (!ldapEx.ErrorCode.Equals(0x31))
                {
                    throw;
                }
            }
            finally
            {
                Console.WriteLine(DateTime.Now.ToString() + " Response: " + (userAuthenticated ? "Found" : "Not Found"));
            }

            return userAuthenticated;
        }

        private static bool VerifyServerCertificate(LdapConnection connection, X509Certificate certificate)
        {
            X509Certificate2 cert = new X509Certificate2(certificate);

            if (!cert.Verify())
            {
                // Could not validate potentially self-signed SSL certificate. Prompting user to install certificate themselves.
                X509Certificate2UI.DisplayCertificate(cert);

                // Try verifying again as the user may have allowed the certificate, and return the result.
                if (!cert.Verify())
                {
                    throw new SecurityException("Could not verify server certificate. Make sure this certificate comes from a trusted Certificate Authority.");
                }
            }

            return true;
        }



        public static void GetDomainUser(string domainName, string loginId, string pwd)
        {
            string msg = "";
            //string DomainPath = "LDAP://" + domainName;
            PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, domainName);

            try
            {
                Console.WriteLine("Finding...");

                UserPrincipal p = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, loginId);
                Console.WriteLine("Found!");

                Console.WriteLine("Finding...");
                if (domainContext.ValidateCredentials(loginId, pwd))
                {
                    Console.WriteLine("Found!");
                }
            }
            catch (COMException ex)
            {
                if (ex.ErrorCode == -2147023570)
                {
                    msg = "AD User Id or password is incorrect";
                }
                else if (ex.ErrorCode == -2147016646)
                {
                    msg = "AD server is not operational.";
                }
                else
                {
                    msg = ex.Message;
                }

                Console.WriteLine(String.Format("{0} : {1}", MethodBase.GetCurrentMethod(), ex.GetFullMessageForLog()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("{0} : {1}", MethodBase.GetCurrentMethod(), ex.GetFullMessageForLog()));
            }
        }




        public static void GetDomainUser2(string domainName, string loginId, string pwd)
        {
            string msg = "";
            //string DomainPath = "LDAP://" + domainName;

            //string DomainPath = String.Format("LDAP://cn={0},dc={1}", loginId, domainName);
            //string DomainPath = String.Format("LDAP://dc={1}/cn={0}", loginId, domainName);

            //string DomainPath = String.Format("LDAP://dc={1}/UID={0}", loginId, domainName);

            loginId = domainName + "\\" + loginId; //make sure user name has domain name.
            var context = new PrincipalContext(ContextType.Domain, "server_address", loginId, pwd);
            /* server_address = "192.168.15.36"; //don't include ldap in url */

            //string DomainPath = domainName;

            try
            {
                Console.WriteLine("Finding...");
                //using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, DomainPath, DomainPath, ContextOptions.SimpleBind))
                //{
                //    Console.WriteLine("Finding... sub...");

                //    // validate the credentials
                //    bool isValid = pc.ValidateCredentials(loginId, pwd);
                //    Console.WriteLine("Found!");
                //}
            }
            catch (COMException ex)
            {
                if (ex.ErrorCode == -2147023570)
                {
                    msg = "AD User Id or password is incorrect";
                }
                else if (ex.ErrorCode == -2147016646)
                {
                    msg = "AD server is not operational.";
                }
                else
                {
                    msg = ex.Message;
                }

                Console.WriteLine(String.Format("{0} : {1}", MethodBase.GetCurrentMethod(), ex.GetFullMessageForLog()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("{0} : {1}", MethodBase.GetCurrentMethod(), ex.GetFullMessageForLog()));
            }
        }
    }
}

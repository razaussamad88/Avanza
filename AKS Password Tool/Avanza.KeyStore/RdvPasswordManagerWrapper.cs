using Avanza.KeyStore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Configuration;

using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace Avanza.KeyStore
{
    public class RdvPasswordManagerWrapper
    {


        #region Members
        private static string _keyStoreServerIP = String.Empty;     /*   "172.16.1.101" */
        private static string _tlsCertificatePath = String.Empty;      /*   "C:\\SSL_CERT\\RdvCert.pem" */
        private static int _keystorePort = 5000;
        private static RdvPasswordManagerWrapper _Singleton;
        #endregion


        #region Properties
        public static RdvPasswordManagerWrapper Instance
        {
            get
            {
                if (_Singleton == null)
                {
                    throw new Exception("Object not created");
                }

                return _Singleton;
            }
        }
        #endregion


        #region Constructor
        public RdvPasswordManagerWrapper()
        {
            XmlConfigReader reader = (XmlConfigReader)System.Configuration.ConfigurationManager.GetSection("MachineAddresses");
            if (reader != null)
            {
                if (reader.RootSection.HasAttribute("AvanzaPasswordManagerRDV"))
                {
                    _keyStoreServerIP = reader.RootSection.GetTextValue("address");
                    _keystorePort = Convert.ToInt32(reader.RootSection.GetTextValue("port"));
                    _tlsCertificatePath = reader.RootSection.GetTextValue("tls-certificate-path");
                }
            }            
        }



        public RdvPasswordManagerWrapper(string KeystoreIP, int KeystorePort, string CertificatePath)
        {
            _keyStoreServerIP = KeystoreIP;
            _keystorePort = KeystorePort;
            _tlsCertificatePath = CertificatePath;
        }
        #endregion

        public static void Create()
        {
            if (_Singleton != null)
            {
                //throw new Exception("Object already created");
            }

            _Singleton = new RdvPasswordManagerWrapper();
        }

        public static void Initialize()
        {
            XmlConfigReader reader = (XmlConfigReader)System.Configuration.ConfigurationManager.GetSection("MachineAddresses");
            if (reader != null)
            {
                if (reader.RootSection.HasAttribute("AvanzaPasswordManagerRDV"))
                {
                    _keyStoreServerIP = reader.RootSection.GetTextValue("address");
                    _keystorePort = Convert.ToInt32(reader.RootSection.GetTextValue("port"));
                    _tlsCertificatePath = reader.RootSection.GetTextValue("tls-certificate-path");
                }
            }
        }

        public static void CreateAndInitialize()
        {
            if (_Singleton == null)
            {
                _Singleton = new RdvPasswordManagerWrapper();
            }            

            XmlConfigReader reader = (XmlConfigReader)System.Configuration.ConfigurationManager.GetSection("MachineAddresses");
            if (reader != null)
            {
                if (reader.RootSection.HasChild("AvanzaPasswordManagerRDV"))
                {
                    _keyStoreServerIP = reader.RootSection.GetChild("AvanzaPasswordManagerRDV").GetTextValue("address");
                    _keystorePort = Convert.ToInt32(reader.RootSection.GetChild("AvanzaPasswordManagerRDV").GetTextValue("port"));
                    _tlsCertificatePath = reader.RootSection.GetChild("AvanzaPasswordManagerRDV").GetTextValue("tls-certificate-path");
                }
            }
        }

        public static void Create(string KeystoreIP, int KeystorePort, string CertificatePath)
        {
            if (_Singleton != null)
            {
                throw new Exception("Object already created");
            }

            _Singleton = new RdvPasswordManagerWrapper(KeystoreIP, KeystorePort, CertificatePath);
        }

        public bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if ((int)sslPolicyErrors != 0)
                return true;

            return false;
        }

        private string CallPasswordManager(string sInputData)
        {
            string sKey = String.Empty;

            IPEndPoint keystoreIP = new IPEndPoint(IPAddress.Parse(_keyStoreServerIP), _keystorePort);
            TcpClient client = new TcpClient();

            client.Connect(keystoreIP);

            SslStream sslStream = new SslStream(client.GetStream(), false);
            sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            var clientCert = X509Certificate.CreateFromCertFile(_tlsCertificatePath);

            X509CertificateCollection certColl = new X509CertificateCollection();
            certColl.Add(clientCert);

            sslStream.AuthenticateAsClient(_keyStoreServerIP, null, SslProtocols.Tls12, false);

            if (client.Connected)
            {
                byte[] buffer = new byte[10240];
                buffer = Encoding.ASCII.GetBytes(sInputData);

                sslStream.Write(buffer, 0, buffer.Length);

                byte[] RcvBuffer = new byte[10240];
                int nData = sslStream.Read(RcvBuffer, 0, RcvBuffer.Length);
                sKey = Encoding.ASCII.GetString(RcvBuffer, 0, nData);
            }


            client.Close();

            return sKey;
        }


        public string ChangeConnString(object sDbType, object sDataSource, object sUserID, object sPassword, object sInitialCat)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(sDbType); sb.Append(",");
            sb.Append(sDataSource); sb.Append(",");
            sb.Append(sUserID); sb.Append(",");
            sb.Append(sPassword); sb.Append(",");
            sb.Append(sInitialCat);

            string sMsg = String.Format(WrapperHelper.PRODUCT_MESSAGE_FORMAT, ((int)PasswordManagerOpCode.CHANGE_CON_STR).ToString(), sb.ToString());

            return this.CallPasswordManager(sMsg);
        }

        public string ChangeSwitchConfigConnStr(object sDbType, object sDataSource, object sUserID, object sPassword, object sInitialCat)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(sDbType); sb.Append(",");
            sb.Append(sDataSource); sb.Append(",");
            sb.Append(sUserID); sb.Append(",");
            sb.Append(sPassword); sb.Append(",");
            sb.Append(sInitialCat);

            string sMsg = String.Format(WrapperHelper.PRODUCT_MESSAGE_FORMAT, WrapperHelper.GetString(PasswordManagerOpCode.CHANGE_SWITCH_CONFIG_CONN_STR), sb.ToString());

            return this.CallPasswordManager(sMsg);
        }

        public string ResetDEKnKEK()
        {
            string sMsg = String.Format(WrapperHelper.PRODUCT_MESSAGE_FORMAT, ((int)PasswordManagerOpCode.RESET_KEK_DEK).ToString(), String.Empty);

            return this.CallPasswordManager(sMsg);
        }

        public string ResetDEK()
        {
            string sMsg = String.Format(WrapperHelper.PRODUCT_MESSAGE_FORMAT, ((int)PasswordManagerOpCode.RESET_DEK).ToString(), String.Empty);

            return this.CallPasswordManager(sMsg);
        }


        public static string GetUiComConnectionString()
        {

            try
            {
                string connectionString = string.Empty;
                XmlConfigReader reader = (XmlConfigReader)System.Configuration.ConfigurationManager.GetSection("uicom");
                if (reader != null)
                {
                    if (reader.RootSection.HasAttribute("connection-string"))
                        connectionString = reader.RootSection.GetTextValue("connection-string");
                }
                if (!String.IsNullOrEmpty(connectionString))
                {
                    string fullconstr="";
                    AESCryptography.Instance.AESDecrypt(connectionString, ref fullconstr, KeyStoreBroker.ClearDEK);
                    return fullconstr;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        enum suspectDBUserIds { admin, sa, dba, sys, system, dbo,super };
        public static string GetConnectionString()
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AvanzaDBContext"].ConnectionString;
                if (!String.IsNullOrEmpty(connectionString))
                {
                    string decryptedpass = string.Empty;
                    string[] conarray = null;
                    string encryptedpass = string.Empty;
                    string endhalf = string.Empty;
                    bool isExist = false;
                    string[] providerconn = connectionString.Split(new string[] { "provider=" }, StringSplitOptions.None);
                    string providerName = providerconn[1].Substring(0, providerconn[1].IndexOf(';'));

                        if (providerName.ToUpper() == "System.Data.SqlClient".ToUpper())
                    {

                        string[] userIdconn = connectionString.Split(new string[] { "user id=" }, StringSplitOptions.None);
                        string userId = userIdconn[1].Substring(0, userIdconn[1].IndexOf(';'));
                        
                        if(Enum.IsDefined(typeof(suspectDBUserIds), userId))
                        {
                            throw new Exception(string.Format("Database User {0} has not rights to access the Database" , userId));

                        }


                        conarray = connectionString.Split(new string[] { "password=" }, StringSplitOptions.None);
                        encryptedpass = conarray[1].Substring(0, conarray[1].IndexOf(';'));
                        endhalf = conarray[1].Substring(conarray[1].IndexOf(';'));
                    }


                    AESCryptography.Instance.AESDecrypt(encryptedpass, ref decryptedpass, KeyStoreBroker.ClearDEK);

                    string fullconstr = conarray[0] + "password=" + decryptedpass + endhalf;
                    return fullconstr;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

       
    }
}

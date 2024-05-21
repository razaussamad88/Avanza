using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Configuration;

namespace Avanza.KeyStore
{
    public partial class KeyStoreBroker
    {
        #region Members
        private static string _keyStoreServerIP = String.Empty;     /*   "172.16.1.101" */
        private static string _tlsCertificatePath = String.Empty;      /*   "C:\\SSL_CERT\\RdvCert.pem" */
        private static int _keystorePort = 5000;
        private static KeyStoreBroker _Singleton;
        #endregion

        #region Properties
        public static KeyStoreBroker Instance
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
        public KeyStoreBroker()
        {
            XmlConfigReader xmlConfigReader = WebConfigurationManager.GetSection("MachineAddresses") as XmlConfigReader;
            IConfigSection configSection = xmlConfigReader.RootSection.GetChild("AvanzaKeyStore");

            _keyStoreServerIP = configSection.GetTextValue("address");
            _keystorePort = configSection.GetIntValue("port");
            _tlsCertificatePath = configSection.GetTextValue("tls-certificate-path");
        }

        public KeyStoreBroker(string KeystoreIP, int KeystorePort, string CertificatePath)
        {
            _keyStoreServerIP = KeystoreIP;
            _keystorePort = KeystorePort;
            _tlsCertificatePath = CertificatePath;
        }
        #endregion

        public static void Create()
        {
            if (_Singleton == null)
            {
                _Singleton = new KeyStoreBroker();
            }
        }

        public static void Create(string KeystoreIP, int KeystorePort, string CertificatePath)
        {
            if (_Singleton == null)
            {
                _Singleton = new KeyStoreBroker(KeystoreIP, KeystorePort, CertificatePath);
            }
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if ((int)sslPolicyErrors != 0)
                return true;

            return false;
        }

        private string SendRequest(KeyStoreOpCode opCode, ProductIndex productIndex, string data)
        {
            string message = String.Format(WrapperHelper.KEYSTORE_MESSAGE_FORMAT, (int)opCode, (int)productIndex, data);

            return GetKey(message);
        }

        private string SendRequest(KeyStoreOpCode opCode, ProductIndex productIndex)
        {
            return SendRequest(opCode, productIndex, String.Empty);
        }

        public string DECRYPT_DEK(ProductIndex productIndex, string encDEK)
        {
            return SendRequest(KeyStoreOpCode.DECRYPT_DEK, productIndex, encDEK);
        }

        public string DECRYPT_DEK(string productIndex, string encDEK)
        {
            return DECRYPT_DEK((ProductIndex)Enum.Parse(typeof(ProductIndex), productIndex), encDEK);
        }

        public string ENCRYPT_DEK(ProductIndex productIndex, string clearDEK)
        {
            return SendRequest(KeyStoreOpCode.ENCRYPT_DEK, productIndex, clearDEK);
        }

        public string ENCRYPT_DEK(string productIndex, string clearDEK)
        {
            return ENCRYPT_DEK((ProductIndex)Enum.Parse(typeof(ProductIndex), productIndex), clearDEK);
        }

        public string GENERATE_KEK_DEK(ProductIndex productIndex)
        {
            return SendRequest(KeyStoreOpCode.GENERATE_KEK_DEK, productIndex);
        }

        public string GENERATE_KEK_DEK(string productIndex)
        {
            return GENERATE_KEK_DEK((ProductIndex)Enum.Parse(typeof(ProductIndex), productIndex));
        }

        public string RESET_DEK(ProductIndex productIndex, string data)
        {
            return SendRequest(KeyStoreOpCode.RESET_DEK, productIndex, data);
        }

        public string RESET_DEK(string productIndex, string data)
        {
            return RESET_DEK((ProductIndex)Enum.Parse(typeof(ProductIndex), productIndex), data);
        }

        private string GetKey(string sInputData)
        {
            //logger.LogInfo("sInputData = " + sInputData);

            string sKey = String.Empty;

            IPEndPoint keystoreIP = new IPEndPoint(IPAddress.Parse(_keyStoreServerIP), _keystorePort);
            TcpClient client = new TcpClient();

            //logger.LogInfo("Connecting to Keystore Client : " + keystoreIP.Address + ":" + keystoreIP.Port);
            //ActivityLogger.SystemLog("Connecting to Keystore Client : " + keystoreIP.Address + ":" + keystoreIP.Port);

            client.Connect(keystoreIP);

            SslStream sslStream = new SslStream(client.GetStream(), false);
            sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            var clientCert = X509Certificate.CreateFromCertFile(_tlsCertificatePath);

            X509CertificateCollection certColl = new X509CertificateCollection();
            certColl.Add(clientCert);

            sslStream.AuthenticateAsClient(_keyStoreServerIP, null, SslProtocols.Tls12, false);

            if (client.Connected)
            {
                byte[] buffer = new byte[1024];
                buffer = Encoding.ASCII.GetBytes(sInputData);

                sslStream.Write(buffer, 0, buffer.Length);

                //while (sslStream.Length == 0)
                //{
                //    Thread.Sleep(100);
                //}

                byte[] RcvBuffer = new byte[1024];
                int nData = sslStream.Read(RcvBuffer, 0, RcvBuffer.Length);
                sKey = Encoding.ASCII.GetString(RcvBuffer, 0, nData);
            }

            //logger.LogInfo("sKey = " + sKey);

            client.Close();

            return sKey;
        }
    }
}
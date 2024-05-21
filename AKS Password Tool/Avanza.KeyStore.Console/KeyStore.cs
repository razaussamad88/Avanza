using Avanza.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Avanza.KeyStore.Console
{
    public class KeyStore
    {
        private static bool m_IsLoaded = false;

        static KeyStore()
        {
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;

            KeyStoreBroker.Create();
            KeyStoreBroker.LoadDEK();

            ActivityLogger.Init("AvanzaLogs");
        }

        public KeyStore()
        {
            if (!m_IsLoaded)
            {
                m_IsLoaded = true;
            }
        }

        public string Encrypt(string inString)
        {
            String output = null;

            try
            {
                string input = inString;

                KeyStoreBroker.AESEncrypt(input, ref output, KeyStoreBroker.ClearDEK);
            }
            catch (Exception ex)
            {
                ActivityLogger.Instance.Log(Constants.User, MethodBase.GetCurrentMethod(), ex);
            }

            return output;
        }

        public string Decrypt(string inString)
        {
            String output = null;

            try
            {
                string input = inString;

                KeyStoreBroker.AESDecrypt(input, ref output, KeyStoreBroker.ClearDEK);
            }
            catch (Exception ex)
            {
                ActivityLogger.Instance.Log(Constants.User, MethodBase.GetCurrentMethod(), ex);
            }

            return output;
        }
    }
}

using System;
using System.Configuration;
using System.IO;

namespace Avanza.KeyStore
{
    public partial class KeyStoreBroker
    {
        #region Members

        private static string m_DEKPath;
        private static string m_ClearDEK;
        private static string m_ClearDEK_PatternRDV;
        private static string m_EncryptedDEK;
        private static AESCryptography m_Crypto = new AESCryptography();
        private static AESCryptographyRdv m_CryptoForRdv = new AESCryptographyRdv();
        #endregion

        static KeyStoreBroker()
        {
            m_DEKPath = ConfigurationManager.AppSettings["DEKPath"];
            m_ClearDEK = ConfigurationManager.AppSettings["ClearDEK"];
        }

        #region Properties
        public static string ClearDEK
        {
            get
            {
                if (String.IsNullOrEmpty(m_ClearDEK))
                {
                    //m_ClearDEK = LoadDEK();
                    throw new Exception("Key not loaded. Hint: Call LoadDEK()");
                }

                return m_ClearDEK;
            }
        }

        public static string ClearDEK_PatternRDV
        {
            get
            {
                if (String.IsNullOrEmpty(m_ClearDEK_PatternRDV))
                {
                    //m_ClearDEK = LoadDEK();
                    throw new Exception("Key not loaded. Hint: Call LoadDEK()");
                }

                return m_ClearDEK_PatternRDV;
            }
        }

        public static string EncryptedDEK
        {
            get
            {
                if (String.IsNullOrEmpty(m_EncryptedDEK))
                {
                    throw new Exception("Key not loaded. Hint: Call LoadDEK()");
                }

                return m_EncryptedDEK;
            }
        }
        #endregion

        #region Methods
        public static void LoadDEK()
        {
            if (String.IsNullOrEmpty(m_ClearDEK))
            {
                try
                {
                    if (true)
                    {
                        //string encrypted_DEK = String.Empty;
                        //string filePath = String.Empty;

                        //filePath = ConfigurationManager.AppSettings["DEKPath"];
                        m_EncryptedDEK = File.ReadAllText(m_DEKPath).ToString().Trim();

                        //PA-DSS : Vision index is 4 for Keystore Utility
                        //string index = "4";
                        m_ClearDEK = Instance.DECRYPT_DEK(ProductIndex.BackOffice, m_EncryptedDEK); // _indentifier_***_

                        m_ClearDEK = m_ClearDEK.Replace("\0", String.Empty);
                        m_ClearDEK = m_ClearDEK.Replace("\n", String.Empty);

                        m_ClearDEK_PatternRDV = m_ClearDEK;
                        m_ClearDEK = m_ClearDEK.Replace(" ", String.Empty);
                    }
                }
                catch (Exception ex)
                {
                    //ActivityLogger.SystemLog("------- ** Symmetry SERVER (EncryptionkeyReader) EXCEPTION ** -------");
                    //ActivityLogger.SystemLog(ex.StackTrace);
                    //ActivityLogger.SystemLog(ex.Message);
                    //ActivityLogger.SystemLog("-------- Symmetry SERVER (EncryptionkeyReader) EXCEPTION END --------");
                }
            }
            if (String.IsNullOrEmpty(m_EncryptedDEK))
            {
                try
                {
                    m_EncryptedDEK = Instance.ENCRYPT_DEK(ProductIndex.BackOffice, m_ClearDEK);

                    var clr = "5154BB08518C4797B162C67F9F897092";
                    //clr += clr;     // 64-Bit (Chars)
                    //var clr = Guid.NewGuid().ToString("N").ToUpper();

                    var enc = Instance.ENCRYPT_DEK(ProductIndex.BackOffice, clr);

                    enc = "BUhYHH5RhagC8u6EWsmw/xJ1KieHXIVfsyoZPpgSXJz6e/M5UpmSjFFATsSAmL15";
                    var clr2 = Instance.DECRYPT_DEK(ProductIndex.BackOffice, enc);
                    clr2 = Instance.DECRYPT_DEK(ProductIndex.BackOffice, enc);


                    var kek = "ZAYCyp2TLTKMV4dnnI+lfm19Sxw1e/MSOeoQrtbfUxhMBzZcyR+2CUc2vPlWQ/4R";
                    var enc_dek = "BUhYHH5RhagC8u6EWsmw/xJ1KieHXIVfsyoZPpgSXJz6e/M5UpmSjFFATsSAmL15";

                    var res = Instance.DECRYPT_DEK(ProductIndex.BackOffice, kek);
                    res = Instance.DECRYPT_DEK(ProductIndex.BackOffice, enc_dek);


                    var aaaa = Instance.DECRYPT_DEK(ProductIndex.BackOffice, m_EncryptedDEK);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static bool AESDecrypt(string sEncrptTxt, ref string sClearTxt, string sKey)
        {
            return m_Crypto.AESDecrypt(sEncrptTxt, ref sClearTxt, sKey);
        }

        public static bool AESEncrypt(string sClearTxt, ref string sEncrptTxt, string sKey)
        {
            return m_Crypto.AESEncrypt(sClearTxt, ref sEncrptTxt, sKey);
        }

        public static bool AESDecryptForRdv(string sEncrptTxt, ref string sClearTxt, string sKey)
        {
            return m_CryptoForRdv.AESDecrypt(sEncrptTxt, ref sClearTxt, sKey);
        }

        public static bool AESEncryptForRdv(string sClearTxt, ref string sEncrptTxt, string sKey)
        {
            return m_CryptoForRdv.AESEncrypt(sClearTxt, ref sEncrptTxt, sKey);
        }
        #endregion
    }
}

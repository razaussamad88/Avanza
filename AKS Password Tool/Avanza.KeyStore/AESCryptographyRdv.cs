using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Avanza.KeyStore
{
    public class AESCryptographyRdv
    {
        private static AESCryptographyRdv m_Instance = null;

        public static AESCryptographyRdv Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new AESCryptographyRdv();
                }

                return m_Instance;
            }
        }

        public bool AESEncrypt(string sClearTxt, ref string sEncryptTxt, string sKey)
        {
            // Check arguments. 
            if (sClearTxt == null || sClearTxt.Length <= 0)
                throw new ArgumentNullException("Invalid or empty ClearTxt");
            if (sKey == null || sKey.Length <= 0)
                throw new ArgumentNullException("Invalid or empty Key");

            byte[] encrypted;
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (RijndaelManaged aesAlg = new RijndaelManaged())
            //using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.BlockSize = 128;
                aesAlg.Key = StringToByteArray(sKey);

                //Set initialization vector.
                //aesAlg.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                // aesAlg.Key = sKey;
                byte[] bIV = new byte[16];
                for (int i = 0; i < bIV.Length; i++)
                {
                    bIV[i] = 0x00;
                }
                aesAlg.IV = bIV;
                aesAlg.Padding = PaddingMode.Zeros;


                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(sClearTxt);
                        }
                        encrypted = msEncrypt.ToArray();
                        //sEncryptTxt = ASCIIEncoding.ASCII.GetString(encrypted);
                        sEncryptTxt = BitConverter.ToString(encrypted).Replace("-", " ");
                        //sEncryptTxt = encrypted.ToString();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream. 
            return true;
        }

        public bool AESDecrypt(string sEncrptTxt, ref string sClearTxt, string sKey)
        {
            // Check arguments. 
            if (sEncrptTxt == null || sEncrptTxt.Length <= 0 || sKey == null || sKey.Length <= 0)
                throw new ArgumentNullException("Invalid Arguments");

            byte[] encrypted = new byte[sEncrptTxt.Length];

            string plaintext = null;
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (RijndaelManaged aesAlg = new RijndaelManaged())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.BlockSize = 128;
                aesAlg.Key = StringToByteArray(sKey);
                EventLog.WriteEntry("RdvKeyStore", "Key found : " + sKey, EventLogEntryType.Error);
                // aesAlg.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                // aesAlg.Key = sKey;
                byte[] bIV = new byte[16];
                for (int i = 0; i < bIV.Length; i++)
                {
                    bIV[i] = 0x00;
                }
                aesAlg.IV = bIV;
                aesAlg.Padding = PaddingMode.Zeros;

                //encrypted = ASCIIEncoding.ASCII.GetBytes(sEncrptTxt);
                encrypted = StringToByteArray(sEncrptTxt);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                EventLog.WriteEntry("RdvKeyStore", "decryptor created", EventLogEntryType.Error);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(encrypted))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.ASCII))
                        //using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            //Write all data to the stream.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                        sClearTxt = plaintext.Trim(new char[] { '\0' });
                    }
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return true;
        }

        public byte[] StringToByteArray(string hex)
        {
            string[] strs = hex.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            byte[] arr = new byte[strs.Length];

            for (int i = 0; i < strs.Length; ++i)
            {
                arr[i] = (byte)Convert.ToInt32(strs[i], 16);
            }
            return arr;
        }

        public static string RdvDEK;

        public static string GetRdvDEK()
        {
            if (RdvDEK.Length != 0)
                return RdvDEK;

            return RdvDEK;
        }
    }
}


using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Avanza.Core.Utility
{
    // this AESCryptography only for x64-bit
    public class AESCryptography_x64
    {
        private static string licenseKey = "zlAhtcug/DQX4RRTSctuAnyGntq0Ng+e9N0YXgPb044=";

        public static string LicenseKey { get { return licenseKey; } }

        public bool AESEncrypt(string sClearTxt, ref string sEncryptTxt, string sKey)
        {
            byte[] b = AESEncryptForQueue(System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(sClearTxt), sKey);
            if (b == null)
                return false;
            sEncryptTxt = Convert.ToBase64String(b);
            return true;
        }

        public bool AESDecrypt(string sEncrptTxt, ref string sClearTxt, string sKey)
        {
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            byte[] b = AESDecryptForQueue(Convert.FromBase64String(sEncrptTxt), sKey);
            if (b == null)
                return false;
            sClearTxt = encoding.GetString(b);
            return true;
        }

        private static byte[] AESDecryptForQueue(byte[] sEncrptTxt, string sKey)
        {
            if (sEncrptTxt == null || sEncrptTxt.Length <= 0 || sKey == null || sKey.Length <= 0)
                throw new ArgumentNullException("Invalid Arguments");

            using (RijndaelManaged aesAlg = new RijndaelManaged())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.BlockSize = 128;
                aesAlg.Key = Convert.FromBase64String(sKey);

                byte[] bIV = new byte[16];
                for (int i = 0; i < bIV.Length; i++)
                {
                    bIV[i] = 0x00;
                }
                aesAlg.IV = bIV;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (MemoryStream msDecrypt = new MemoryStream())
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                        {
                            csDecrypt.Write(sEncrptTxt, 0, sEncrptTxt.Length);
                            csDecrypt.FlushFinalBlock();
                            return msDecrypt.ToArray();
                        }
                    }
                }
            }
        }

        private static byte[] AESEncryptForQueue(byte[] sClearTxt, string sKey)
        {
            if (sClearTxt == null || sClearTxt.Length <= 0)
                throw new ArgumentNullException("Invalid or empty ClearTxt");
            if (sKey == null || sKey.Length <= 0)
                throw new ArgumentNullException("Invalid or empty Key");

            using (RijndaelManaged aesAlg = new RijndaelManaged())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.BlockSize = 128;
                aesAlg.Key = Convert.FromBase64String(sKey);

                byte[] bIV = new byte[16];
                for (int i = 0; i < bIV.Length; i++)
                {
                    bIV[i] = 0x00;
                }
                aesAlg.IV = bIV;
                aesAlg.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            csEncrypt.Write(sClearTxt, 0, sClearTxt.Length);
                            csEncrypt.FlushFinalBlock();
                            return msEncrypt.ToArray();
                        }
                    }
                }
            }
        }
    }
}

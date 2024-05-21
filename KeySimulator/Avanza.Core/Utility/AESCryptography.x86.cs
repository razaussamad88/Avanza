using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Avanza.Core.Utility
{
    // this AESCryptography only for x86-bit
    public class AESCryptography_x86
    {
        private static string licenseKey = "f0fa9c8a95b7e35327c8277d54a7257e";

        public static string LicenseKey { get { return licenseKey; } }

        public bool AESEncrypt(string sClearTxt, ref string sEncryptTxt, string sKey)
        {

            // Check arguments. 
            if (sClearTxt == null || sClearTxt.Length <= 0)
                throw new ArgumentNullException("ClearTxt");
            if (sKey == null || sKey.Length <= 0)
                throw new ArgumentNullException("Key");

            byte[] encrypted;
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            //using (RijndaelManaged aesAlg = new RijndaelManaged())
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.BlockSize = 128;
                aesAlg.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

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
                        sEncryptTxt = BitConverter.ToString(encrypted).Replace("-", string.Empty);
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
            if (sEncrptTxt == null || sEncrptTxt.Length <= 0)
            {
                throw new ArgumentNullException("ClearTxt");
                return false;
            }
            if (sKey == null || sKey.Length <= 0)
            {
                throw new ArgumentNullException("Key");
                return false;
            }

            byte[] encrypted = new byte[sEncrptTxt.Length];

            string plaintext = null;
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            //using (RijndaelManaged aesAlg = new RijndaelManaged())
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.BlockSize = 128;
                aesAlg.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                //Set initialization vector.
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

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(encrypted))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            //Write all data to the stream.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                        sClearTxt = plaintext.Replace("\0", "");
                    }
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return true;
        }

        public byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}

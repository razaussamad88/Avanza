using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;

namespace RdvEncryption
{
    public static class RdvCryptography
    {
        private static string key = "2AF583D23BEC9C32";
        private static string ToString(byte[] value)
        {
            StringBuilder builder = new StringBuilder(value.Length * 2);

            for (int cnt = 0; cnt < value.Length; cnt++)
            {
                builder.Append(value[cnt].ToString("X2"));
            }
            return builder.ToString();
        }

        private static byte[] ToByteArray(string value)
        {
            if ((value.Length % 2) > 0)
                throw new ApplicationException("Invalid input string. Has to be multiple of 2");

            byte[] retVal = new byte[value.Length / 2];

            int cnt = 0;
            for (int index = 0; index < value.Length;)
            {
                string strTemp = value.Substring(index, 2);
                retVal[cnt] = (Byte.Parse(strTemp, NumberStyles.HexNumber));
                index = index + 2;
                cnt++;
            }
            return retVal;
        }

        public static string Encrypt(string data)
        {
            byte[] DesKey = new byte[8];
            byte[] temp = new byte[8];
            for (int i = 0; i < 8; i++)
                temp[i] = 0;

            string encryptedResult = String.Empty;

            if (key.Length != 16)
                throw new Exception("invalid key length given should be 16");
            while (data.Length % 8 != 0)
                data += '\0';
            if (string.IsNullOrEmpty(data) == true)
                throw new Exception("data is either null,empty");

            //getting hex form
            for (int i = 0, j = 0; i < key.Length; j++, i = i + 2)
                DesKey[j] = Convert.ToByte(key.Substring(i, 2), 16);

            byte[] dataBytes = Encoding.ASCII.GetBytes(data);

            using (System.IO.MemoryStream encryptStream = new MemoryStream(dataBytes.Length * 2))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.Zeros;
                des.KeySize = 64;

                ICryptoTransform encrypt = des.CreateEncryptor(DesKey, temp);

                using (CryptoStream encStream = new CryptoStream(encryptStream, encrypt, CryptoStreamMode.Write))
                {
                    encStream.Write(dataBytes, 0, dataBytes.Length);
                    encStream.Close();
                }

                encryptedResult = ToString(encryptStream.ToArray());
                encryptStream.Close();

            }

            return encryptedResult;
        }

        public static string Decrypt(string data)
        {
            byte[] DesKey = new byte[8];
            byte[] temp = new byte[8];

            for (int i = 0; i < 8; i++)
                temp[i] = 0;

            string decryptedResult = String.Empty;

            if (key.Length != 16)
                throw new Exception("invalid key length given should be 16");

            if (string.IsNullOrEmpty(data) == true || data.Length % 16 != 0)
                throw new Exception("data is either null,empty or not multiple of 16");

            //getting hex form
            for (int i = 0, j = 0; i < key.Length; j++, i = i + 2)
                DesKey[j] = Convert.ToByte(key.Substring(i, 2), 16);

            byte[] dataBytes = new byte[data.Length / 2];

            for (int i = 0, j = 0; i < data.Length; j++, i = i + 2)
                dataBytes[j] = Convert.ToByte(data.Substring(i, 2), 16);

            using (System.IO.MemoryStream decryptStream = new MemoryStream(dataBytes.Length))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.Zeros;
                des.KeySize = 64;

                ICryptoTransform decrypt = des.CreateDecryptor(DesKey, temp);

                using (CryptoStream decStream = new CryptoStream(decryptStream, decrypt, CryptoStreamMode.Write))
                {
                    decStream.Write(dataBytes, 0, dataBytes.Length);
                    decStream.Close();
                }

                decryptedResult = Encoding.ASCII.GetString(decryptStream.ToArray());
                decryptStream.Close();
            }

            return decryptedResult.TrimEnd('\0');
        }
    }
}

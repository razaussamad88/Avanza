//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Avanza.Core.Utility
{
    public class Cryptographer
    {
        private string[] HashSaltValues = new string[] { "Vision2.2", "RDVSM" };

        public enum HashSalt { Vision = 0, RDVSM };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const int IsoAnsiLatinI = 1252; // ISO ANSI LATIN I

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Text.Encoding _encoding = Encoding.GetEncoding(IsoAnsiLatinI);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] _key = { 0x6a, 0x68, 0x6b, 0x6b, 0x73, 0x6f, 0x61, 0x6b, 0x66, 0x73, 0x61, 0x7a, 0x61, 0x2d, 0x39, 0x32 };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] _customKey;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] _iv = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CipherMode _mode = CipherMode.CBC;

        public Encoding Encoding
        {
            get
            {
                return this._encoding;
            }
            set
            {
                Guard.CheckNull(value, "Cryptographer.Encoding");
                this._encoding = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public byte[] Key
        {
            set
            {
                Guard.CheckNull(value, "Cryptographer.Key");
                this._key = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public byte[] CustomKey
        {
            set
            {
                //   Guard.CheckNull(value, "Cryptographer.Key");
                this._customKey = value;

            }
        }

        public Cryptographer()
        { }

        public Cryptographer(byte[] key)
        {
            this.Key = key;
        }

        public string Encrypt(string input)
        {
            string retVal;
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            byte[] txtStream = this._encoding.GetBytes(input);
            using (System.IO.MemoryStream encryptStream = new MemoryStream(txtStream.Length * 2))
            {
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Mode = this._mode;
                ICryptoTransform encrypt = tdes.CreateEncryptor(this._key, this._iv);

                using (CryptoStream encStream = new CryptoStream(encryptStream, encrypt, CryptoStreamMode.Write))
                {
                    encStream.Write(txtStream, 0, txtStream.Length);
                    encStream.Close();
                }
                //retVal = Cryptographer.ToString(encryptStream.ToArray());
                retVal = ByteArrayUtility.ToHexString(encryptStream.ToArray());
                encryptStream.Close();
            }
            return retVal;
        }

        public string Decrypt(string input)
        {
            string retVal;
            //if ((input == null) || (input == ""))
            //    return "";
            //byte[] byteStream = Cryptographer.ToByteArray(input);

            if (string.IsNullOrEmpty(input))
                return string.Empty;

            byte[] byteStream = ByteArrayUtility.FromHexString(input);

            using (System.IO.MemoryStream decryptStream = new MemoryStream(byteStream.Length))
            {
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Mode = this._mode;
                ICryptoTransform decrypt = tdes.CreateDecryptor(this._key, this._iv);

                using (CryptoStream decStream = new CryptoStream(decryptStream, decrypt, CryptoStreamMode.Write))
                {
                    decStream.Write(byteStream, 0, byteStream.Length);
                    decStream.Close();
                }

                retVal = this._encoding.GetString(decryptStream.ToArray());
                decryptStream.Close();
            }

            return retVal;
        }

        public byte[] EncryptDES(byte[] data, byte[] key)
        {

            byte[] result = new byte[8];
            byte[] nullVector = { 0, 0, 0, 0, 0, 0, 0, 0 };

            if ((data == null))
                return nullVector;
            //byte[] txtStream = this._encoding.GetBytes(input);


            using (System.IO.MemoryStream encryptStream = new MemoryStream(result))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Mode = CipherMode.ECB;
                des.IV = nullVector;
                des.Padding = PaddingMode.None;
                des.Key = key;
                ICryptoTransform encrypt = des.CreateEncryptor(key, nullVector);

                using (CryptoStream encStream = new CryptoStream(encryptStream, encrypt, CryptoStreamMode.Write))
                {
                    encStream.Write(data, 0, 8);
                    encStream.Close();
                }
                //retVal = Cryptographer.ToString(encryptStream.ToArray());
                encryptStream.Close();
            }
            return result;
        }

        public byte[] DecryptDES(byte[] data, byte[] key)
        {
            byte[] result = new byte[8];
            byte[] nullVector = { 0, 0, 0, 0, 0, 0, 0, 0 };
            if ((data == null))
                return nullVector;
            // byte[] byteStream = Cryptographer.ToByteArray(data);

            using (System.IO.MemoryStream decryptStream = new MemoryStream(result))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Mode = CipherMode.ECB;
                des.IV = nullVector;
                des.Padding = PaddingMode.None;
                des.Key = key;
                ICryptoTransform decrypt = des.CreateDecryptor(key, nullVector);

                using (CryptoStream decStream = new CryptoStream(decryptStream, decrypt, CryptoStreamMode.Write))
                {
                    decStream.Write(data, 0, 8);
                    decStream.Close();
                }

                //retVal = this._encoding.GetString(decryptStream.ToArray());
                decryptStream.Close();
            }
            return result;
        }

        private static string ToString(byte[] value)
        {
            StringBuilder builder = new StringBuilder(100);

            for (int cnt = 0; cnt < value.Length; cnt++)
            {
                builder.Append(value[cnt].ToString("x2"));
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

        //PADSS
        public class x86
        {
            private readonly AESCryptography_x86 m_Crypto_x86 = new AESCryptography_x86();

            public bool AESEncrypt(string sClearTxt, ref string sEncryptTxt, string sKey)
            {
                return m_Crypto_x86.AESEncrypt(sClearTxt, ref sEncryptTxt, sKey);
            }

            public bool AESDecrypt(string sEncrptTxt, ref string sClearTxt, string sKey)
            {
                return m_Crypto_x86.AESDecrypt(sEncrptTxt, ref sClearTxt, sKey);
            }
        }

        public class x64
        {
            private readonly AESCryptography_x64 m_Crypto_x64 = new AESCryptography_x64();

            public bool AESEncrypt(string sClearTxt, ref string sEncryptTxt, string sKey)
            {
                return m_Crypto_x64.AESEncrypt(sClearTxt, ref sEncryptTxt, sKey);
            }

            public bool AESDecrypt(string sEncrptTxt, ref string sClearTxt, string sKey)
            {
                return m_Crypto_x64.AESDecrypt(sEncrptTxt, ref sClearTxt, sKey);
            }
        }

        public string ComputeHash(string input)
        {
            return ComputeHash(input, HashSalt.Vision);
        }

        public string ComputeHash(string input, HashSalt hsalt)
        {
            string hashSalt = HashSaltValues[(int)hsalt];

            input = string.Concat(input, hashSalt);
            byte[] byteData = Encoding.GetBytes(input);
            byte[] result;
            string hashedResult = string.Empty;
            SHA512 hashCalculator = new SHA512Managed();
            result = hashCalculator.ComputeHash(byteData);

            hashedResult = Encoding.GetString(result);
            if (hashedResult.Contains("'"))
                hashedResult = hashedResult.Replace("'", "");
            if (hashedResult.Contains("\0"))
                hashedResult = hashedResult.Replace("\0", "");
            hashedResult = Regex.Escape(hashedResult);
            return hashedResult;
        }
    }
}
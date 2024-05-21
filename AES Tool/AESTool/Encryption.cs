using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using Vision.Common;

namespace Vision.Web.Common
{
    public class Cryptographer
    {
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
        {
        }

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
                retVal = ByteArrayUtility.ToHexString(encryptStream.ToArray());
                encryptStream.Close();
            }
            return retVal;
        }

        public string Decrypt(string input)
        {
            string retVal;
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

                decryptStream.Close();
            }
            return result;
        }


        public byte[] ComputeHash(string input, string hashSalt)
        {
            //string hashSalt = "VISION";
            input = string.Concat(input, hashSalt);
            byte[] byteData = Encoding.GetBytes(input);
            byte[] result;
            string hashedResult = string.Empty;
            SHA512 hashCalculator = new SHA512Managed();
            result = hashCalculator.ComputeHash(byteData);

            //hashedResult = Encoding.GetString(result);
            //if (hashedResult.Contains("'"))
            //    hashedResult = hashedResult.Replace("'", "");
            //if (hashedResult.Contains("\0"))
            //    hashedResult = hashedResult.Replace("\0", "");
            //hashedResult = Regex.Escape(hashedResult);
            //return hashedResult;
            return result;
        }




        #region PADSS

        public bool AESEncrypt(string sClearTxt, ref string sEncryptTxt, string sKey)
        {
            AESCryptography AESCrypto = new AESCryptography();
            return AESCrypto.AESEncrypt(sClearTxt, ref sEncryptTxt, sKey);
        }


        public bool AESDecrypt(string sEncrptTxt, ref string sClearTxt, string sKey)
        {

            AESCryptography AESCrypto = new AESCryptography();
            return AESCrypto.AESDecrypt(sEncrptTxt, ref sClearTxt, sKey);

        }

        #endregion
    }
    public static class Guard
    {

        /// <summary>
        /// Checks an argument to ensure it isn't null
        /// </summary>
        /// <param name="argValue">The argument value to check.</param>
        /// <param name="argName">The name of the argument.</param>
        public static void CheckNull(object argVal, string argName)
        {
            if (argVal == null)
                throw new ArgumentNullException(argName);
        }

        /// <summary>
        /// Checks a string argument to ensure it isn't null or empty
        /// </summary>
        /// <param name="argValue">The argument value to check.</param>
        /// <param name="argName">The name of the argument.</param>
        public static string CheckNullOrEmpty(string argVal, string argName)
        {
            Guard.CheckNull(argVal, argName);
            Guard.CheckEmpty(argVal, argName);

            return argVal;
        }

        public static string CheckEmpty(string argVal, string argName)
        {
            if (argVal.Length == 0)
                throw new ArgumentException(string.Format("Argument[{0}] can't be empty", argName));

            return argVal;
        }

        /// <summary>
        /// Checks an Enum argument to ensure that its value is defined by the specified Enum type.
        /// </summary>
        /// <param name="enumType">The Enum type the value should correspond to.</param>
        /// <param name="value">The value to check for.</param>
        /// <param name="argName">The name of the argument holding the value.</param>
        public static void CheckEnumValue(Type enumType, object value, string argName)
        {
            if (Enum.IsDefined(enumType, value) == false)
                throw new ArgumentException(String.Format("Argument[{0}] contain invalid value for Enum[{1}]", argName, enumType.ToString()));
        }

    }
    public static class ByteArrayUtility
    {
        private const string hexEmpty = "0x";
        private const byte highMask = 0xf0;
        private const byte lowMask = 0x0f;

        public static bool Compare(byte[] arg1, byte[] arg2)
        {
            if (arg1 == null || arg2 == null)
            {
                return false;
            }

            if (arg1.Length != arg2.Length)
            {
                return false;
            }

            bool retVal = true;
            for (int index = 0; index < arg1.Length; index++)
            {
                if (arg1[index] != arg2[index])
                {
                    retVal = false;
                    break;
                }
            }

            return retVal;
        }

        public static byte[] FromHexString(string hexValue)
        {
            if ((hexValue.Length % 2) > 0)
                throw new ApplicationException("Invalid input string. Has to be multiple of 2");

            byte[] retVal = new byte[hexValue.Length / 2];

            int count = 0;
            for (int index = 0; index < hexValue.Length;)
            {
                string strTemp = hexValue.Substring(index, 2);
                retVal[count] = (Byte.Parse(strTemp, System.Globalization.NumberStyles.HexNumber));
                index = index + 2;
                count++;
            }

            return retVal;
        }

        public static string ToHexString(byte[] value)
        {
            if ((value == null) || (value.Length == 0))
            {
                return hexEmpty;
            }

            StringBuilder builder = new StringBuilder(hexEmpty, (value.Length * 2) + hexEmpty.Length);

            for (int count = 0; count < value.Length; count++)
            {
                builder.Append(value[count].ToString("x2"));
            }
            return builder.ToString().Substring(2);
        }

        public static byte GetLowNibble(byte arg)
        {
            return (byte)(arg & lowMask);
        }

        public static byte GetHighNibble(byte arg)
        {
            return (byte)((arg & highMask) >> 4);
        }

        private static char ToHexChar(byte nibble)
        {
            char retVal;

            if (nibble < 10)
                retVal = (char)('0' + nibble);
            else
                retVal = (char)('a' + (nibble - 10));

            return retVal;
        }
    }
}

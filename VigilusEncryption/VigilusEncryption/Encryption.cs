using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace VigilusEncryption
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


        public string ComputeHash(string input)
        {
            string hashSalt = "Vigilus1.0";
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
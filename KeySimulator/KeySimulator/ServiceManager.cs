using Avanza.Core.AvanzaKeyStore;
using Avanza.Core.Logging;
using Avanza.Core.Utility;
using System;
using System.Configuration;
using System.IO;

namespace EncryptionSimulator
{
    public static class ServiceManager
    {
        public static bool? Is64Bit { get; private set; }

        private static string _RdvSMClearDEK = String.Empty;
        private static string _VisionClearDEK = String.Empty;

        private static string _GenericClearDEK = String.Empty;

        public static void Log(string msg)
        {
            StreamWriter fs = new StreamWriter(ConfigurationManager.AppSettings["RdvSMEncryption"], true);
            fs.WriteLine(msg);
            fs.Close();
        }

        public static void Init()
        {
            Is64Bit = null;

            Log("Init...");
            KeyStoreWrapper.Create();
            Log("Created!");


            var dek = KeyStoreWrapper.Instance.GENERATE_KEK_DEK(ProductIndex.Vision);

            if (dek.Length == 64)
            {
                // x64
                Is64Bit = true;
            }
            else
            {
                // x86
                Is64Bit = false;
            }


            string filePath = String.Empty, encrypted_DEK = String.Empty;

            Log("--- RDVSM ---");

            try
            {
                filePath = ConfigurationManager.AppSettings["RDVSMDEKStorePath"];
                Log("FilePath: " + filePath);

                encrypted_DEK = System.IO.File.ReadAllText(filePath).ToString().Trim();
                Log("encrypted_DEK: " + encrypted_DEK);

                _RdvSMClearDEK = KeyStoreWrapper.Instance.DECRYPT_DEK(ProductIndex.RDVSM, encrypted_DEK);
                Log("_RdvSMClearDEK: " + _RdvSMClearDEK);
            }
            catch (Exception ex)
            { Log(ex.Message); }


            Log("--- Vision ---");

            try
            {
                filePath = ConfigurationManager.AppSettings["VisionDEKStorePath"];
                Log("FilePath: " + filePath);

                encrypted_DEK = System.IO.File.ReadAllText(filePath).ToString().Trim();
                Log("encrypted_DEK: " + encrypted_DEK);

                _VisionClearDEK = KeyStoreWrapper.Instance.DECRYPT_DEK(ProductIndex.Vision, encrypted_DEK);
                Log("_VisionClearDEK: " + _VisionClearDEK);
            }
            catch (Exception ex)
            { Log(ex.Message); }
        }


        private static bool is_x64_DEK()
        {
            bool isValid = false;

            try
            {
                // x64
                var clearDEK = @"mjKQiDhmftasgDkD0K+ZAOm8YntMmLY3C85AewGCMks=";
                var encDEK = KeyStoreWrapper.Instance.ENCRYPT_DEK(ProductIndex.Vision, clearDEK);

                if (clearDEK.Length == 44 && encDEK.Length == 64)
                    isValid = true;
                else
                    isValid = false;
            }
            catch (Exception ex)
            {
            }

            return isValid;
        }

        private static bool is_x86_DEK()
        {
            bool isValid = false;

            try
            {
                // x86

                /*
                var dek122 = KeyStoreWrapper.Instance.GENERATE_KEK_DEK(ProductIndex.Vision);
                var dek1221 = KeyStoreWrapper.Instance.DECRYPT_DEK(ProductIndex.Vision, dek122);

                dek1221 = dek1221.Replace(" ", String.Empty);
                dek1221 = dek1221.Replace("\0", String.Empty);
                var dek1222 = KeyStoreWrapper.Instance.ENCRYPT_DEK(ProductIndex.Vision, dek1221);
                */


                var clearDEK = @"d893a9b6722baed414b6be325ba83dfb";
                var encDEK = KeyStoreWrapper.Instance.ENCRYPT_DEK(ProductIndex.Vision, clearDEK);


                if (clearDEK.Length == 32 && encDEK.Length == 144)
                    isValid = true;
                else
                    isValid = false;
            }
            catch (Exception ex)
            {
            }

            return isValid;
        }

        public static void Init_PADSS()
        {
            try
            {
                Log("Init...");
                KeyStoreWrapper.Create();
                Log("Created!");


                if (is_x64_DEK())
                    Is64Bit = true;
                else if (is_x86_DEK())
                    Is64Bit = false;
            }
            catch (Exception ex)
            { Log(ex.Message); }
        }

        public static void LoadDEK(string encrypted_DEK, ProductIndex ind)
        {
            //Log("Init...");
            //KeyStoreWrapper.Create();
            //Log("Created!");

            try
            {
                _GenericClearDEK = KeyStoreWrapper.Instance.DECRYPT_DEK(ind, encrypted_DEK);
                Log("_GenericClearDEK: " + _GenericClearDEK);
            }
            catch (Exception ex)
            { Log(ex.Message); }
        }

        public static string ComputeHash(string userId, Cryptographer.HashSalt productIndex)
        {
            Cryptographer ObjCrypto = new Cryptographer();
            return ObjCrypto.ComputeHash(userId + userId, productIndex);
        }

        public static string AESEncrypt(string clearText)
        {
            string encryptText = String.Empty;

            if (Is64Bit.Value)
            {
                Cryptographer.x64 objCrypto = new Cryptographer.x64();
                objCrypto.AESEncrypt(clearText, ref encryptText, Util.ClearDecryptionKeyServer);
            }
            else
            {
                Cryptographer.x86 objCrypto = new Cryptographer.x86();
                objCrypto.AESEncrypt(clearText, ref encryptText, Util.ClearDecryptionKeyServer);
            }

            return encryptText;
        }

        public static string AESDecrypt(string encryptText)
        {
            string clearText = String.Empty;

            if (Is64Bit.Value)
            {
                Cryptographer.x64 objCrypto = new Cryptographer.x64();
                objCrypto.AESDecrypt(encryptText, ref clearText, Util.ClearDecryptionKeyServer);
            }
            else
            {
                Cryptographer.x86 objCrypto = new Cryptographer.x86();
                objCrypto.AESDecrypt(encryptText, ref clearText, Util.ClearDecryptionKeyServer);
            }

            return clearText;
        }

        public static void SetDEK(ProductIndex productIndex)
        {
            switch (productIndex)
            {
                case ProductIndex.RDVSM:
                    Util.ClearDecryptionKeyServer = _RdvSMClearDEK;
                    break;

                case ProductIndex.Vision:
                    Util.ClearDecryptionKeyServer = _VisionClearDEK;
                    break;
            }
        }

        public static void SetGenericDEK()
        {
            Util.ClearDecryptionKeyServer = _GenericClearDEK;
        }
    }
}
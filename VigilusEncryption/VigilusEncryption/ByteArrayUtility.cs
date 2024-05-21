using System;
using System.Text;
using System.Globalization;

namespace VigilusEncryption
{
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
            for (int index = 0; index < hexValue.Length; )
            {
                string strTemp = hexValue.Substring(index, 2);
                retVal[count] = (Byte.Parse(strTemp, NumberStyles.HexNumber));
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

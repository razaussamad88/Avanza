//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;
using System.Text;

namespace Avanza.Core.Utility
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

        // TO DO: Implement
        public static byte[] FromHexString(string hexValue)
        {
            return null;
        }

        // TO DO: Implement
        public static string ToHexString(byte[] arg)
        {
            if ((arg == null) || (arg.Length == 0))
            {
                return hexEmpty;
            }

            System.Text.StringBuilder builder = new StringBuilder(hexEmpty, (arg.Length * 2)+ hexEmpty.Length);

            for (int index = 0; index < arg.Length; index++)
            {
                builder.Append(ToHexChar(ByteArrayUtility.GetHighNibble(arg[index])) );
                builder.Append(ToHexChar(ByteArrayUtility.GetHighNibble(arg[index])) );
            }

            return builder.ToString();
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

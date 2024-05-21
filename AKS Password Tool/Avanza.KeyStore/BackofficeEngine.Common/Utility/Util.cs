//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using Microsoft.Win32;
using System;
using System.Text;
using System.Xml;

namespace Avanza.KeyStore
{
    public static class Util
    {
        public const string DateFormat = "yyyy-MM-dd HH:mm:ss";
        private const string KeyStore = "AvanzaKeyStore";
        public static string fileName = "";
        public static string remoteFilePath = "";

        #region Properties
        #endregion

        public static System.Type ToType(TypeCode code)
        {
            System.Type retVal;

            switch (code)
            {
                case TypeCode.Boolean:
                    retVal = typeof(bool);
                    break;

                case TypeCode.Byte:
                    retVal = typeof(byte);
                    break;

                case TypeCode.Char:
                    retVal = typeof(char);
                    break;

                case TypeCode.DateTime:
                    retVal = typeof(DateTime);
                    break;

                case TypeCode.Decimal:
                    retVal = typeof(decimal);
                    break;

                case TypeCode.Double:
                    retVal = typeof(double);
                    break;

                case TypeCode.Int16:
                    retVal = typeof(short);
                    break;

                case TypeCode.Int32:
                    retVal = typeof(int);
                    break;

                case TypeCode.Int64:
                    retVal = typeof(long);
                    break;

                case TypeCode.SByte:
                    retVal = typeof(sbyte);
                    break;

                case TypeCode.Single:
                    retVal = typeof(Single);
                    break;

                case TypeCode.String:
                    retVal = typeof(string);
                    break;

                case TypeCode.UInt16:
                    retVal = typeof(ushort);
                    break;

                case TypeCode.UInt32:
                    retVal = typeof(uint);
                    break;

                case TypeCode.UInt64:
                    retVal = typeof(ulong);
                    break;

                default:
                    throw new ArgumentException(string.Format("Unrecognized Type {0}", code));
            }

            return retVal;
        }

        public static TypeCode ToTypeCode(string value)
        {
            TypeCode retVal;
            value = value.ToLowerInvariant();

            if (value.CompareTo("sbyte") == 0)
                retVal = TypeCode.SByte;
            else if (value.CompareTo("short") == 0)
                retVal = TypeCode.Int16;
            else if (value.CompareTo("ushort") == 0)
                retVal = TypeCode.UInt16;
            else if (value.CompareTo("int") == 0)
                retVal = TypeCode.Int32;
            else if (value.CompareTo("uint") == 0)
                retVal = TypeCode.UInt32;
            else if (value.CompareTo("long") == 0)
                retVal = TypeCode.Int64;
            else if (value.CompareTo("ulong") == 0)
                retVal = TypeCode.UInt64;
            else if (value.CompareTo("bool") == 0)
                retVal = TypeCode.Boolean;
            else
                retVal = (TypeCode)Enum.Parse(typeof(System.TypeCode), value, true);

            return retVal;
        }

        public static object ParseTo(string value, System.Type type)
        {
            TypeCode typeCode = System.Type.GetTypeCode(type);

            if ((typeCode != TypeCode.String) && (value.Length == 0))
                return default(Type);

            object retVal = value;

            if (typeCode == TypeCode.String)
                retVal = value;
            else if (typeCode == TypeCode.Boolean)
                retVal = Util.GetBoolValue(value);
            else if (typeCode == TypeCode.Byte)
                retVal = Convert.ToByte(value);
            else if (typeCode == TypeCode.Char)
                retVal = Convert.ToChar(value);
            else if (typeCode == TypeCode.DateTime)
            {
                if (!value.ToUpper().Contains("SYSDATE") && !value.ToUpper().Contains("GETDATE()"))
                    retVal = XmlConvert.ToDateTime(value, Util.DateFormat);
            }
            else if (typeCode == TypeCode.Decimal)
                retVal = Convert.ToDecimal(value);
            else if (typeCode == TypeCode.Double)
                retVal = Convert.ToDouble(value);
            else if (typeCode == TypeCode.Int16)
                retVal = Convert.ToInt16(value);
            else if (typeCode == TypeCode.Int32)
                retVal = Convert.ToInt32(value);
            else if (typeCode == TypeCode.Int64)
                retVal = Convert.ToInt64(value);
            else if (typeCode == TypeCode.SByte)
                retVal = Convert.ToSByte(value);
            else if (typeCode == TypeCode.Single)
                retVal = Convert.ToSingle(value);
            else if (typeCode == TypeCode.UInt16)
                retVal = Convert.ToUInt16(value);
            else if (typeCode == TypeCode.UInt32)
                retVal = Convert.ToUInt32(value);
            else if (typeCode == TypeCode.UInt64)
                retVal = Convert.ToUInt64(value);
            else if (typeCode == TypeCode.Empty)
                retVal = null;
            else
                throw new ArgumentException(string.Format("Conversion for Type {0} is not supported. Data: {1}",
                                                    type, value), (Exception)null);

            return retVal;
        }


        public static string ToString(object value)
        {
            string retVal = string.Empty;
            if (value is DateTime)
            {
                DateTime dtvalue = (DateTime)value;
                retVal = dtvalue.ToString(Util.DateFormat);
            }
            else if (value != null)
                retVal = value.ToString();
            else
                retVal = "null";

            return retVal;
        }

        public static object ConvertTo(object value, System.TypeCode code)
        {
            return Util.ConvertTo(value, Util.ToType(code));
        }

        public static object ConvertTo(object value, System.Type type)
        {
            if (value == null)
                return value;

            if (value.GetType() == type)
                return value;

            object retVal;
            TypeCode typeCode = System.Type.GetTypeCode(type);

            if (typeCode == TypeCode.Object)
                return value;

            if (value is string)
            {
                string temp = (string)value;
                if (temp.Length == 0)
                    return default(Type);
            }

            if (value is System.DBNull)
                retVal = value;
            else if (typeCode == TypeCode.Boolean)
                retVal = Convert.ToBoolean(value);
            else if (typeCode == TypeCode.Byte)
                retVal = Convert.ToByte(value);
            else if (typeCode == TypeCode.Char)
                retVal = Convert.ToChar(value);
            else if (typeCode == TypeCode.DateTime)
                retVal = Convert.ToDateTime(value);
            else if (typeCode == TypeCode.Decimal)
                retVal = Convert.ToDecimal(value);
            else if (typeCode == TypeCode.Double)
                retVal = Convert.ToDouble(value);
            else if (typeCode == TypeCode.Int16)
                retVal = Convert.ToInt16(value);
            else if (typeCode == TypeCode.Int32)
                retVal = Convert.ToInt32(value);
            else if (typeCode == TypeCode.Int64)
                retVal = Convert.ToInt64(value);
            else if (typeCode == TypeCode.SByte)
                retVal = Convert.ToSByte(value);
            else if (typeCode == TypeCode.Single)
                retVal = Convert.ToSingle(value);
            else if (typeCode == TypeCode.String)
                retVal = value.ToString();
            else if (typeCode == TypeCode.UInt16)
                retVal = Convert.ToUInt16(value);
            else if (typeCode == TypeCode.UInt32)
                retVal = Convert.ToUInt32(value);
            else if (typeCode == TypeCode.UInt64)
                retVal = Convert.ToUInt64(value);
            else if (typeCode == TypeCode.Object)
                retVal = value;
            else if (typeCode == TypeCode.Empty)
                retVal = null;
            else
                throw new ArgumentException(string.Format("Conversion for Type {0} is not supported. Data: {1}",
                                                    type, value));

            return retVal;
        }

        public static string BuildExceptionString(Exception exception)
        {
            System.Text.StringBuilder builder = new StringBuilder(2 * 1024);

            builder.AppendLine(exception.Message).AppendLine(exception.StackTrace);

            while (exception.InnerException != null)
            {
                BuildInnerExceptionString(exception.InnerException, builder);
                exception = exception.InnerException;
            }
            int length = 500;
            if (builder.ToString().Length < length)
                length = builder.ToString().Length;
            return builder.ToString().Substring(0, length);
        }

        private static void BuildInnerExceptionString(Exception innerExcep, StringBuilder builder)
        {
            builder.AppendLine(string.Empty);
            builder.Append("[InnerException]: ");
            builder.AppendLine(innerExcep.Message);
            builder.Append(innerExcep.StackTrace).AppendLine(Environment.NewLine);
        }

        public static bool GetBoolValue(string value)
        {
            string temp = value.ToUpper();
            bool retVal;

            if ((temp.CompareTo("Y") == 0) || (temp.CompareTo("1") == 0) ||
                (temp.CompareTo("TRUE") == 0) || (temp.CompareTo("YES") == 0))
            {
                retVal = true;
            }
            else if ((temp.CompareTo("N") == 0) || (temp.CompareTo("0") == 0) ||
                     (temp.CompareTo("FALSE") == 0) || (temp.CompareTo("NO") == 0))
            {
                retVal = false;
            }
            else
                throw new ArgumentException(string.Format("{0} is not recognized as boolean value", value));

            return retVal;
        }

        public static string ReadRegistryText(string url, string key)
        {
            Guard.CheckNullOrEmpty(url, "Avanza.Utility.Util.ReadRegistryText");
            Guard.CheckNullOrEmpty(key, "Avanza.Utility.Util.ReadRegistryText");

            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(url);

            if (regKey == null)
                return null;

            return (string)regKey.GetValue(key);
        }
    }
}
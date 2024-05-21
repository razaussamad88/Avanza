//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================
// Modification history
// DH(01) : Fix review finding
//===============================================================================

using System;
using System.Xml;
using System.Globalization;

//namespace Avanza.Core.Utility
namespace Avanza.Common.Utility
{
    public class XmlUtil
    {
        public const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string XML_TRUE = "Y";
        public const string XML_FALSE = "N";

        private System.Xml.XmlReader _reader;

        public XmlUtil(System.Xml.XmlReader reader)
        {
            this._reader = reader;
        }

        public XmlReader Reader
        {
            get
            {
                return this._reader;
            }
        }
        public string Name
        {
            get { return this._reader.Name; }
        }

        public void ReadNextNode()
        {
            this._reader.Read();
        }

        public string GetTextValue(string name)
        {
            string result;
            try
            {
                result = this._reader.GetAttribute(name).Trim();
            }
            catch (Exception e)
            {
                throw new XmlDataException(e, string.Format("Attribute: {0} not found. Node: {1}",
                                           name, this._reader.Name));
            }

            return result;
        }

        // TO DO: correct below functionality
        public string GetValue(string name, string defVal)
        {
            try
            {
                string temp = this._reader.GetAttribute(name);
                if (temp != null)
                {
                    if (temp.Length != 1)
                        defVal = temp.Trim();
                    else
                        defVal = temp;
                }
            }
            catch (Exception)
            { }

            return defVal;
        }

        public bool GetValue(string name, bool defVal)
        {
            try
            {
                defVal = this.GetBoolValue(name);
            }
            catch
            { }

            return defVal;
        }

        public int GetValue(string name, int defVal)
        {
            try
            {
                string temp = this._reader.GetAttribute(name);
                if (temp != null)
                    defVal = XmlConvert.ToInt32(temp.Trim());

            }
            catch
            { }

            return defVal;
        }

        public long GetValue(string name, long defVal)
        {
            try
            {
                string temp = this._reader.GetAttribute(name);
                if (temp != null)
                    defVal = XmlConvert.ToInt64(temp.Trim());

            }
            catch
            { }

            return defVal;
        }

        public T GetEnumValue<T>(string name, T defVal)
        {
            try
            {
                string temp = this._reader.GetAttribute(name);
                if (temp != null)
                    defVal = (T)Enum.Parse(typeof(T), temp.Trim(), true);
            }
            catch
            { }

            return defVal;

        }

        public T GetEnumValue<T>(string name)
        {
            string temp = string.Empty;
            try
            {
                temp = this._reader.GetAttribute(name).Trim();
                return (T)Enum.Parse(typeof(T), temp, true);

            }
            catch (Exception ex)
            {
                throw new XmlDataException(ex, "Failed to parse Field [{0}] as Enum {1}. Value={2}",
                                           name, typeof(T).FullName, temp);
            }
        }

        public string GetTextValue(string name, int size)
        {
            string retVal = this.GetTextValue(name);
            if (retVal.Length != size)
                throw new XmlDataException(string.Format("{0} = {1}, size expected {2}", name, retVal, size));

            return retVal;
        }

        public string GetTextValue(string name, int minSize, int maxSize)
        {
            string retVal = this.GetTextValue(name);
            int len = retVal.Length;
            if ((len > maxSize) || (len < minSize))
                throw new XmlDataException(string.Format("\"{0}\" contains value with {1} characters. Size expected {2} - {3} characters",
                                                     name, len, minSize, maxSize));

            return retVal;
        }

        public bool GetBoolValue(string name)
        {
            string temp = this.GetTextValue(name).ToUpper();
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
                throw new XmlDataException(string.Format("{0} is not recognized as boolean value",
                                           this.GetTextValue(name)));

            return retVal;
        }

        public int GetIntValue(string name)
        {
            string temp = this.GetTextValue(name);
            int retVal;
            try
            {
                retVal = int.Parse(temp);
            }
            catch (Exception excep)
            {
                throw new XmlDataException(string.Format("Failed to parse {0} as integer",
                                           temp), excep);
            }

            return retVal;
        }

        public int GetIntValue(string name, int minVal, int maxVal)
        {
            int val = this.GetIntValue(name);
            if ((val < minVal) || (val > maxVal))
                throw new XmlDataException(string.Format(" Value {0} must be in range {1} - {2}",
                                            val, minVal, maxVal));
            return val;
        }

        public short GetShortValue(string rName)
        {
            return (short)this.GetIntValue(rName);
        }

        public short GetShortValue(string rName, short rMinVal, short rMaxVal)
        {
            return (short)this.GetIntValue(rName, rMinVal, rMaxVal);
        }

        public DateTime GetDateTimeValue(string rName, string dateTimeFormat)
        {
            string Temp = this.GetTextValue(rName);
            DateTime RetVal;

            try
            {
                RetVal = DateTime.ParseExact(Temp, dateTimeFormat,
                                  System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                throw new XmlDataException(string.Format("{0} is not recognized as date time. Format used {1}",
                                           Temp, DATE_FORMAT));
            }

            return RetVal;
        }

        public DateTime GetDateTimeValue(string rName)
        {
            return GetDateTimeValue(rName, XmlUtil.DATE_FORMAT);
        }

        public double GetDoubleValue(string rName)
        {
            string Temp = this.GetTextValue(rName);
            double RetVal;
            try
            {
                RetVal = double.Parse(Temp);
            }
            catch (Exception)
            {
                throw new XmlDataException(string.Format("Failed to parse {0} as real number.", Temp));

            }

            return RetVal;
        }

        public double GetDoubleValue(string rName, double rMinVal, double rMaxVal)
        {
            double RetVal = this.GetDoubleValue(rName);

            if ((RetVal < rMinVal) || (RetVal > rMaxVal))
                throw new XmlDataException(string.Format(" Value {0} must be in range {1} - {2}",
                    RetVal, rMinVal, rMaxVal));

            return RetVal;
        }

        public long GetLongValue(string rName)
        {
            string Temp = this.GetTextValue(rName);
            long RetVal;

            try
            {
                RetVal = long.Parse(Temp);
            }
            catch (Exception)
            {
                throw new XmlDataException(string.Format("Failed to parse {0} as long value.", Temp));

            }

            return RetVal;
        }

        public long GetLongValue(string rName, long rMinVal, long rMaxVal)
        {
            long RetVal = this.GetLongValue(rName);

            if ((RetVal < rMinVal) || (RetVal > rMaxVal))
                throw new XmlDataException(string.Format("Value {0} must be in range {1} - {2}",
                                           RetVal, rMinVal, rMaxVal));

            return RetVal;
        }
    }
}

//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;
using System.Collections;
using System.Xml;

namespace Avanza.KeyStore
{
    public class XmlConfigSection : IConfigSection
    {
        private XmlElement _xmlNode;

        public XmlConfigSection(XmlNode node)
        {
            this._xmlNode = (XmlElement)node;
        }

        private XmlConfigSection()
        { }

        string IConfigSection.Name
        {
            get { return this._xmlNode.Name; }
        }

        string IConfigSection.Value
        {
            get { return this._xmlNode.InnerText; }
        }

        bool IConfigSection.HasChildSections
        {
            get { return this._xmlNode.HasChildNodes; }
        }

        bool IConfigSection.HasChild(string name)
        {
            return (this._xmlNode.SelectSingleNode(name) != null);
        }

        bool IConfigSection.HasAttribute(string name)
        {
            return this._xmlNode.HasAttribute(name);
        }

        IConfigSection IConfigSection.GetChild(string name)
        {
            XmlNode node = this._xmlNode.SelectSingleNode(name);

            if (null == node)
                throw new ConfigurationException("Node: {0} not found. Parent Node: {1}",
                                                  name, this._xmlNode.Name);

            return new XmlConfigSection(node);
        }

        bool IConfigSection.GetBoolValue(string name)
        {
            string value = this.GetAttribValue(name).ToLowerInvariant();

            return this.ParseBoolean(value, name);
        }

        string IConfigSection.GetTextValue(string name)
        {
            return this.GetAttribValue(name);
        }

        short IConfigSection.GetShortValue(string name)
        {
            string value = this.GetAttribValue(name);
            try
            {
                return XmlConvert.ToInt16(value);
            }
            catch (Exception excep)
            {
                throw new ConfigurationException(excep, "Failed to parse value {0} as short. Attribute: {1}; Section: {2}",
                                             value, name, this.Name);
            }
        }

        int IConfigSection.GetIntValue(string name)
        {
            string value = this.GetAttribValue(name);
            try
            {
                return XmlConvert.ToInt32(value);
            }
            catch (Exception excep)
            {
                throw new ConfigurationException(excep, "Failed to parse value {0} as int. Attribute: {1}; Section: {2}",
                                             value, name, this.Name);
            }
        }

        long IConfigSection.GetLongValue(string name)
        {
            string value = this.GetAttribValue(name);
            try
            {
                return XmlConvert.ToInt64(value);
            }
            catch (Exception excep)
            {
                throw new ConfigurationException(excep, "Failed to parse value {0} as long. Attribute: {1}; Section: {2}",
                                             value, name, this.Name);
            }
        }

        double IConfigSection.GetDoubleValue(string name)
        {
            string strVal = this.GetAttribValue(name);
            try
            {
                return XmlConvert.ToDouble(strVal);
            }
            catch (Exception excep)
            {
                throw new ConfigurationException(excep, "Failed parse value {0} as double. Attribute: {1}; Section: {2}",
                                             strVal, name, this.Name);
            }
        }

        decimal IConfigSection.GetDecimalValue(string name)
        {
            string strVal = this.GetAttribValue(name);
            try
            {
                return XmlConvert.ToDecimal(strVal);
            }
            catch (Exception excep)
            {
                throw new ConfigurationException(excep, "Failed to parse value {0} as decimal. Attribute: {1}; Section: {2}",
                                             strVal, name, this.Name);
            }
        }

        /// <summary>
        /// Returns Datetime value
        /// </summary>
        /// <param name="name">name of Attribute</param>
        /// <returns></returns>
        /// <remarks>Value should be in universal format</remarks>
        DateTime IConfigSection.GetDateTimeValue(string name)
        {
            string strVal = this.GetAttribValue(name);
            try
            {
                return Convert.ToDateTime(strVal);
            }
            catch (Exception excep)
            {
                throw new ConfigurationException(excep, "Failed to parse value {0} as datetime. Attribute: {1}; Section: {2}",
                                             strVal, name, this.Name);
            }
        }

        public T GetEnumValue<T>(string name, T defVal)
        {
            try
            {
                string temp = this.GetAttribValue(name).Trim();
                defVal = (T)Enum.Parse(typeof(T), temp, true);
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
                temp = this.GetAttribValue(name).Trim();
                return (T)Enum.Parse(typeof(T), temp, true);
            }
            catch (Exception ex)
            {
                throw new ConfigurationException(ex, "Failed to parse Field [{0}] as Enum {1}. Value={2}",
                                                 name, typeof(T).FullName, temp);
            }
        }

        char IConfigSection.GetCharValue(string name)
        {
            string strVal = this.GetAttribValue(name);
            try
            {
                return XmlConvert.ToChar(strVal);
            }
            catch (Exception excep)
            {
                throw new ConfigurationException(excep, "Failed to parse value {0} as decimal. Attribute: {1}; Section: {2}",
                                             strVal, name, this.Name);
            }
        }

        bool IConfigSection.GetValue(string name, bool defVal)
        {
            if (this._xmlNode.HasAttribute(name))
            {
                try
                {
                    defVal = this.ParseBoolean(this._xmlNode.GetAttribute(name), name);
                }
                catch (Exception)
                { }
            }

            return defVal;
        }

        string IConfigSection.GetValue(string name, string defVal)
        {
            if (this._xmlNode.HasAttribute(name))
                defVal = this._xmlNode.GetAttribute(name);

            return defVal;
        }

        short IConfigSection.GetValue(string name, short defVal)
        {
            if (this._xmlNode.HasAttribute(name))
            {
                try
                {
                    defVal = XmlConvert.ToInt16(this._xmlNode.GetAttribute(name));
                }
                catch (Exception)
                { }
            }

            return defVal;
        }

        int IConfigSection.GetValue(string name, int defVal)
        {
            if (this._xmlNode.HasAttribute(name))
            {
                try
                {
                    defVal = XmlConvert.ToInt32(this._xmlNode.GetAttribute(name));
                }
                catch (Exception)
                { }
            }

            return defVal;
        }

        long IConfigSection.GetValue(string name, long defVal)
        {
            if (this._xmlNode.HasAttribute(name))
            {
                try
                {
                    defVal = XmlConvert.ToInt64(this._xmlNode.GetAttribute(name));
                }
                catch (Exception)
                { }
            }

            return defVal;
        }

        double IConfigSection.GetValue(string name, double defVal)
        {
            if (this._xmlNode.HasAttribute(name))
            {
                try
                {
                    defVal = XmlConvert.ToDouble(this._xmlNode.GetAttribute(name));
                }
                catch (Exception)
                { }
            }

            return defVal;
        }

        decimal IConfigSection.GetValue(string name, decimal defVal)
        {
            if (this._xmlNode.HasAttribute(name))
            {
                try
                {
                    defVal = XmlConvert.ToDecimal(this._xmlNode.GetAttribute(name));
                }
                catch (Exception)
                { }
            }

            return defVal;
        }

        char IConfigSection.GetValue(string name, char defVal)
        {
            if (this._xmlNode.HasAttribute(name))
            {
                try
                {
                    defVal = XmlConvert.ToChar(this._xmlNode.GetAttribute(name));
                }
                catch (Exception)
                { }
            }

            return defVal;
        }

        /// <summary>
        /// Returns Datetime value
        /// </summary>
        /// <param name="name">name of Attribute</param>
        /// <returns></returns>
        /// <remarks>Value should be in universal format</remarks>
        DateTime IConfigSection.GetValue(string name, DateTime defVal)
        {
            if (this._xmlNode.HasAttribute(name))
            {
                try
                {
                    defVal = Convert.ToDateTime(this._xmlNode.GetAttribute(name));
                }
                catch (Exception)
                { }
            }

            return defVal;
        }

        IConfigSection[] IConfigSection.ChildSections
        {
            get
            {
                if (this._xmlNode == null)
                    return null;

                if (!this._xmlNode.HasChildNodes)
                    return null;

                XmlConfigSection temp = null;
                ArrayList childList = new ArrayList(this._xmlNode.ChildNodes.Count);
                foreach (XmlNode node in this._xmlNode.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        temp = new XmlConfigSection(node);
                        childList.Add(temp);
                    }
                }

                if (childList.Count == 0)
                    return null;

                return (XmlConfigSection[])childList.ToArray(temp.GetType());
            }
        }

        public override string ToString()
        {
            return this._xmlNode.OuterXml;
        }

        IConfigSection[] IConfigSection.GetChildSections(string Xpath)
        {
            IConfigSection[] retVal = null;
            try
            {
                XmlNodeList nodelist = this._xmlNode.SelectNodes(Xpath);
                if (nodelist != null)
                {
                    System.Collections.Generic.List<IConfigSection> ConfigSections = new System.Collections.Generic.List<IConfigSection>(nodelist.Count);
                    foreach (System.Xml.XmlNode node in nodelist)
                    {
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            ConfigSections.Add(new XmlConfigSection(node));
                        }
                    }

                    retVal = ConfigSections.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new ConfigurationException(ex, "Failed to find child section. Name \"{0}\" in Section {1}", Xpath, this.Name);
            }

            return retVal;
        }

        internal XmlElement XmlNode
        {
            get
            {
                return this._xmlNode;
            }
            set
            {
                if (this._xmlNode == null)
                    throw new System.NullReferenceException("The value null was found where an instance of an XmlElement was required");

                this._xmlNode = value;
            }
        }

        private string GetAttribValue(string name)
        {
            if (!this._xmlNode.HasAttribute(name))
                throw new ConfigurationException("Attribute: {0} not found. Section: {1}",
                                                 name, this._xmlNode.Name);

            return this._xmlNode.GetAttribute(name);
        }

        private bool ParseBoolean(string value, string name)
        {
            bool retVal;
            value = value.ToLowerInvariant();

            if ((value.CompareTo("1") == 0) || (value.CompareTo("true") == 0) ||
                (value.CompareTo("y") == 0) || (value.CompareTo("yes") == 0))
            {
                retVal = true;
            }
            else if ((value.CompareTo("0") == 0) || (value.CompareTo("false") == 0) ||
                     (value.CompareTo("n") == 0) || (value.CompareTo("no") == 0))
            {
                retVal = false;
            }
            else
                throw new ConfigurationException("Failed to parse value {0} as bool. Attribute: {1}; Section: {2}",
                                                 value, name, this.Name);
            return retVal;
        }

        private string Name
        {
            get { return this._xmlNode.Name; }
        }
    }
}
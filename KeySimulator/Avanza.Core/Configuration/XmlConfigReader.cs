//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Configuration;
using Avanza.Core.Utility;

namespace Avanza.Core.Configuration
{
	public class XmlConfigReader : System.Configuration.ConfigurationSection
	{
        private const string msgInvalidFile = "Invalid Configuration file. Make sure it should be proper XML document";
        private const string msgInvalidUrl = "Invalid File Name {0}";

        private IConfigSection _rootSection;
        		
		public XmlConfigReader()
		{}

        public IConfigSection Load(string fileUrl)
        {
            System.IO.Stream fileStream = null;
            fileUrl = IoUtil.GetCompleteUrl(fileUrl);
            try
            {
                fileStream = new FileStream(fileUrl, FileMode.Open);
                return this.Load(fileStream);
            }
            catch (Exception e)
            {
                throw new ConfigurationException(e, string.Format(msgInvalidUrl, fileUrl));
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }           
		}

        public IConfigSection Load(System.IO.Stream fileStream)
        {
            using (System.IO.TextReader reader = new StreamReader(fileStream))
            {
                return this.Load(reader);
            }
        }

        public IConfigSection Load(System.IO.TextReader reader)
        {
            using (System.Xml.XmlTextReader txtReader = new XmlTextReader(reader))
            {
                return this.Load(txtReader);
            }
        }

        public IConfigSection Load(System.Xml.XmlReader reader)
        {
            XmlDocument oDoc = new XmlDocument();
            try
            {
                oDoc.Load(reader);
            }
            catch(XmlException excep)
            {
                throw new ConfigurationException(excep, msgInvalidFile);
            }

            this._rootSection = new XmlConfigSection(oDoc.DocumentElement);
            return this._rootSection;
        }

		public IConfigSection LoadData(string confData)
		{
			XmlDocument oDoc = new XmlDocument();
			try
			{
				oDoc.LoadXml(confData);
			}
			catch(XmlException excep)
			{
                throw new ConfigurationException(excep, msgInvalidFile);
			}

            this._rootSection = new XmlConfigSection(oDoc.DocumentElement);
			return this._rootSection;
		}

        protected override void DeserializeSection(XmlReader reader)
        {
            if (reader.Read())
            {
                XmlDocument _xmldoc = new XmlDocument();
                _xmldoc.LoadXml(reader.ReadOuterXml());
                this._rootSection = new XmlConfigSection(_xmldoc.DocumentElement);
            }
        }

        public IConfigSection RootSection
        {
            get 
            { 
                return this._rootSection;
            }
        }
    }
}
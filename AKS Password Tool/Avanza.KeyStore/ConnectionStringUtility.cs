using Avanza.KeyStore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Web.Configuration;

namespace Avanza.KeyStore
{
    public static class ConnectionStringUtility
    {
        public static volatile string uicom = null;
        public static volatile string Avanza = null;
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string GetUiComConnectionString()
        {
            try
            {
                if (uicom == null)
                {
                    string connectionString = string.Empty;
                    XmlConfigReader reader = (XmlConfigReader)System.Configuration.ConfigurationManager.GetSection("uicom");
                    if (reader != null)
                    {
                        if (reader.RootSection.HasAttribute("connection-string"))
                            connectionString = reader.RootSection.GetTextValue("connection-string");
                    }
                    if (!String.IsNullOrEmpty(connectionString))
                    {
                        string fullconstr = "";
                        AESCryptography.Instance.AESDecrypt(connectionString, ref fullconstr, KeyStoreBroker.ClearDEK);
                        uicom = fullconstr;
                        return fullconstr;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return uicom;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        enum suspectDBUserIds { admin, sa, dba, sys, system, dbo, super };
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string GetConnectionString()
        {

            try
            {
                if (Avanza == null)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["AvanzaDBContext"].ConnectionString;
                    if (!String.IsNullOrEmpty(connectionString))
                    {
                        string decryptedpass = string.Empty;
                        string[] conarray = null;
                        string encryptedpass = string.Empty;
                        string endhalf = string.Empty;
                        string[] providerconn = connectionString.Split(new string[] { "provider=" }, StringSplitOptions.None);
                        string providerName = providerconn[1].Substring(0, providerconn[1].IndexOf(';'));

                        if (providerName.ToUpper() == "System.Data.SqlClient".ToUpper())
                        {

                            string[] userIdconn = connectionString.Split(new string[] { "user id=" }, StringSplitOptions.None);
                            string userId = userIdconn[1].Substring(0, userIdconn[1].IndexOf(';'));

                            if (Enum.IsDefined(typeof(suspectDBUserIds), userId))
                            {
                                throw new Exception(string.Format("Database User {0} has not rights to access the Database", userId));

                            }

                            conarray = connectionString.Split(new string[] { "password=" }, StringSplitOptions.None);
                            encryptedpass = conarray[1].Substring(0, conarray[1].IndexOf(';'));
                            endhalf = conarray[1].Substring(conarray[1].IndexOf(';'));
                        }
                        else if (providerName.ToUpper() == "Oracle.ManagedDataAccess.Client".ToUpper())
                        {

                            string[] userIdconn = connectionString.Split(new string[] { "USER ID=" }, StringSplitOptions.None);
                            string userId = userIdconn[1].Substring(0, userIdconn[1].IndexOf('"'));

                            if (Enum.IsDefined(typeof(suspectDBUserIds), userId))
                            {
                                throw new Exception(string.Format("Database User {0} has not rights to access the Database", userId));

                            }

                            conarray = connectionString.Split(new string[] { "PASSWORD=" }, StringSplitOptions.None);
                            encryptedpass = conarray[1].Substring(0, conarray[1].IndexOf(';'));
                            endhalf = conarray[1].Substring(conarray[1].IndexOf(';'));
                        }


                        AESCryptography.Instance.AESDecrypt(encryptedpass, ref decryptedpass, KeyStoreBroker.ClearDEK);

                        string fullconstr = conarray[0] + "PASSWORD=" + decryptedpass + endhalf;


                        Avanza = fullconstr;
                        return fullconstr;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return Avanza;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static Configuration config;
        private static void SetConfigSectionXml(string sectionXmlValue, string connectionString)
        {


            System.Configuration.Configuration Config1 = WebConfigurationManager.OpenWebConfiguration("~");
            ConnectionStringsSection conSetting = (ConnectionStringsSection)Config1.GetSection("connectionStrings");
            ConnectionStringSettings StringSettings = new ConnectionStringSettings("AvanzaDBContext", sectionXmlValue);
            conSetting.ConnectionStrings.Remove("connectionStrings");
            conSetting.ConnectionStrings.Add(StringSettings);

            Config1.Save(ConfigurationSaveMode.Modified);



        }

        private static string GetConfigSectionXml(string sectionName)
        {
            return WebConfig.Sections[sectionName].SectionInformation.GetRawXml();
        }

        public static Configuration WebConfig
        {
            get
            {
                if (config == null)
                    config = WebConfigurationManager.OpenWebConfiguration("~");
                return config;
            }
        }

    }
}

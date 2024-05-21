using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CrudDB
{
    class MsSqlDBClass : BaseDBClass
    {
        private static string m_SqlDBConnectionString = String.Empty;

        static MsSqlDBClass()
        {
            m_SqlDBConnectionString = ConfigurationManager.AppSettings[AppConfig.RDV_SQL_CONNSTR];
        }

        public override bool RequestPreRequisite(out string queueName, out string networkId)
        {
            SqlCommand cmd;
            bool isSuccess = false;

            queueName = networkId = String.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(m_SqlDBConnectionString))
                {
                    conn.Open();

                    using (cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = String.Format("SELECT LISTENING_QUEUE FROM MODULE WHERE lower(MODULE_NAME)=lower('{0}')", m_RdvTp_Queue);

                        object itemValue = cmd.ExecuteScalar();

                        if (itemValue == null || String.IsNullOrEmpty(itemValue.ToString()))
                        {
                            throw new Exception("RdvTP LISTENING_QUEUE has no value.");
                        }
                        else
                        {
                            queueName = itemValue.ToString();
                        }
                    }

                    using (cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = String.Format("SELECT NETWORK_ID FROM ADAPTER WHERE lower(ADDRESS)=lower('{0}')", m_DestAddress);
                        object itemValue = cmd.ExecuteScalar();

                        if (itemValue == null || String.IsNullOrEmpty(itemValue.ToString()))
                        {
                            throw new Exception("No record found, " + String.Format("SELECT NETWORK_ID FROM ADAPTER WHERE lower(ADDRESS)=lower('{0}')", m_DestAddress));
                        }
                        else
                        {
                            networkId = itemValue.ToString();
                        }

                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Message: [{0}]", ex.Message);
                Console.WriteLine("InnerException Message: [{0}]", ex.InnerException?.Message);
                Console.WriteLine("StackTrace Message: [{0}]", ex.StackTrace);
            }

            return isSuccess;
        }

        public override string ToString()
        {
            return m_SqlDBConnectionString;
        }
    }
}

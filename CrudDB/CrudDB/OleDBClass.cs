using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CrudDB
{
    class OleDBClass : BaseDBClass
    {
        private static string m_OleDBConnectionString = String.Empty;

        static OleDBClass()
        {
            m_OleDBConnectionString = ConfigurationManager.AppSettings["RDV_OLEDB_CONNSTR"];
        }

        public override bool RequestPreRequisite(out string queueName, out string networkId)
        {
            OleDbCommand cmd;
            bool isSuccess = false;

            queueName = networkId = String.Empty;

            try
            {
                using (OleDbConnection conn = new OleDbConnection(m_OleDBConnectionString))
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
            return m_OleDBConnectionString;
        }
    }
}

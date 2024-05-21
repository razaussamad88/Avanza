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
    abstract class BaseDBClass
    {
        protected static string m_RdvTp_Queue = String.Empty;
        protected static string m_DestAddress = String.Empty;

        static BaseDBClass()
        {
            m_RdvTp_Queue = ConfigurationManager.AppSettings["RDV_TP_QUEUE"];
            m_DestAddress = ConfigurationManager.AppSettings["DEST_ADDRESS"];
        }

        public virtual bool RequestPreRequisite(out string queueName, out string networkId)
        {
            queueName = networkId = String.Empty;

            throw new Exception("RequestPreRequisite method does not define.");
        }
    }
}

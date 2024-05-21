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
    abstract class AppConfig
    {
        public const string RDV_OLEDB_CONNSTR = "RDV_OLEDB_CONNSTR";
        public const string RDV_ORCL_CONNSTR = "RDV_ORCL_CONNSTR";
        public const string RDV_SQL_CONNSTR = "RDV_SQL_CONNSTR";
    }
}

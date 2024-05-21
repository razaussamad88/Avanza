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
    class Program
    {
        private const string c_MSG_FORMAT = "QueueName [{0}]  |  NetworkId [{1}]";
        private const string c_MSG_ReqPre_EXECUTE = "### {0} RequestPreRequisite...";
        private const string c_MSG_ReqPre_CONNSTR = "### ConnectionString [{0}]";
        private const string c_MSG_ReqPre_SUCCESS = "### {0} RequestPreRequisite... Success";
        private const string c_MSG_ReqPre_FAIL = "### {0} RequestPreRequisite... Fail";

        private const string c_ORCL_CLASSIC = "ORACLE Classic";
        private const string c_ORCL_MANAGED = "ORACLE Managed";
        private const string c_MSSQL = "MSSQL";
        private const string c_OLEDB = "OLEDB";

        private static bool hasOledb = ConfigurationManager.AppSettings.AllKeys.Contains(AppConfig.RDV_OLEDB_CONNSTR);
        private static bool hasOrcl = ConfigurationManager.AppSettings.AllKeys.Contains(AppConfig.RDV_ORCL_CONNSTR);
        private static bool hasMsSql = ConfigurationManager.AppSettings.AllKeys.Contains(AppConfig.RDV_SQL_CONNSTR);

        static void Main(string[] args)
        {
            Console.WriteLine("***** Init...");

            Assembly asm = Assembly.GetExecutingAssembly();

            #region Assembly Size
            if (IntPtr.Size == 8)
                Console.WriteLine("[{0}] assembly is 64-bit", asm.Location);
            else if (IntPtr.Size == 4)
                Console.WriteLine("[{0}] assembly is 32-bit", asm.Location);
            else
                Console.WriteLine("[{0}] assembly is unknown bit size", asm.FullName);
            #endregion


            Console.WriteLine("***** Init! ******"); Console.WriteLine(String.Empty); Console.WriteLine(String.Empty);


            Console.WriteLine("***** RequestPreRequisite..."); Console.WriteLine(String.Empty);
            if (hasOledb)
            {
                OLEDB(); Console.WriteLine(String.Empty);
            }
            if (hasOrcl)
            {
                ORACLE_Classic(); Console.WriteLine(String.Empty);
                ORACLE_Managed(); Console.WriteLine(String.Empty);
            }
            if (hasMsSql)
            {
                MSSQL(); Console.WriteLine(String.Empty);
            }


            Console.WriteLine("***** RequestPreRequisite! ******");

            MainEnd();
        }

        private static void MainEnd()
        {
            Console.WriteLine(String.Empty); Console.WriteLine(String.Empty); Console.WriteLine(String.Empty);
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }

        private static void OLEDB()
        {
            OleDBClass oleDBObj = new OleDBClass();
            string queueName = String.Empty, networkId = String.Empty;

            Console.WriteLine(c_MSG_ReqPre_EXECUTE, c_OLEDB);
            Console.WriteLine(c_MSG_ReqPre_CONNSTR, oleDBObj.ToString());

            if (oleDBObj.RequestPreRequisite(out queueName, out networkId))
            {
                Console.WriteLine(c_MSG_FORMAT, queueName, networkId);
                Console.WriteLine(c_MSG_ReqPre_SUCCESS, c_OLEDB);
            }
            else
                Console.WriteLine(c_MSG_ReqPre_FAIL, c_OLEDB);
        }

        private static void MSSQL()
        {
            MsSqlDBClass sqlDBObj = new MsSqlDBClass();
            string queueName = String.Empty, networkId = String.Empty;

            Console.WriteLine(c_MSG_ReqPre_EXECUTE, c_MSSQL);
            Console.WriteLine(c_MSG_ReqPre_CONNSTR, sqlDBObj.ToString());

            if (sqlDBObj.RequestPreRequisite(out queueName, out networkId))
            {
                Console.WriteLine(c_MSG_FORMAT, queueName, networkId);
                Console.WriteLine(c_MSG_ReqPre_SUCCESS, c_MSSQL);
            }
            else
                Console.WriteLine(c_MSG_ReqPre_FAIL, c_MSSQL);
        }

        private static void ORACLE_Classic()
        {
            OracleDBClassicClass orclDBObj = new OracleDBClassicClass();
            string queueName = String.Empty, networkId = String.Empty;

            Console.WriteLine(c_MSG_ReqPre_EXECUTE, c_ORCL_CLASSIC);
            Console.WriteLine(c_MSG_ReqPre_CONNSTR, orclDBObj.ToString());

            if (orclDBObj.RequestPreRequisite(out queueName, out networkId))
            {
                Console.WriteLine(c_MSG_FORMAT, queueName, networkId);
                Console.WriteLine(c_MSG_ReqPre_SUCCESS, c_ORCL_CLASSIC);
            }
            else
                Console.WriteLine(c_MSG_ReqPre_FAIL, c_ORCL_CLASSIC);
        }

        private static void ORACLE_Managed()
        {
            OracleDBClassicClass orclDBObj = new OracleDBClassicClass();
            string queueName = String.Empty, networkId = String.Empty;

            Console.WriteLine(c_MSG_ReqPre_EXECUTE, c_ORCL_MANAGED);
            Console.WriteLine(c_MSG_ReqPre_CONNSTR, orclDBObj.ToString());

            if (orclDBObj.RequestPreRequisite(out queueName, out networkId))
            {
                Console.WriteLine(c_MSG_FORMAT, queueName, networkId);
                Console.WriteLine(c_MSG_ReqPre_SUCCESS, c_ORCL_MANAGED);
            }
            else
                Console.WriteLine(c_MSG_ReqPre_FAIL, c_ORCL_MANAGED);
        }
    }
}

using System;
using System.Collections.Specialized;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Vision.Web.Common
{
    public class HelperModule
    {
        private static string _ConnectionString = "";
        private static string _ConnectionStringTagName = "SdkDbContext";
        private static string _ConnectionEncryptKey = "2428GD19569F9B2C2341839416C8E87G";
        

        public static string EncryptConnectionString(string connection)
        {
            string output = null;
            if (new Cryptographer().AESEncrypt(connection, ref output, _ConnectionEncryptKey))
                return output;
            throw new Exception("unable to Encrypt");
        }

        public static string DecryptConnectionString(string connection)
        {
            string output = null;
            if (!new Cryptographer().AESDecrypt(connection, ref output, _ConnectionEncryptKey))
            {
                throw new Exception("Unable to get connection string");
            }
            return output.Replace("&quot;", "'");
        }

        public static string GetFullExceptionForLog(Exception ex)
        {
            if (ex == null) return string.Empty;

            StringBuilder sb = new StringBuilder();


            if (ex.InnerException == null)
            {
                sb.AppendLine();
                sb.AppendLine("Exception message: " + ex.Message);
                sb.AppendLine("Stack trace: " + ex.StackTrace);
                sb.AppendLine();

                return sb.ToString();
            }
            else
            {
                sb.AppendLine("---BEGIN Inner Exception---");
                sb.Append(GetFullExceptionForLog(ex.InnerException));
                sb.AppendLine("---END Inner Exception---");
            }


            return sb.ToString();
        }
    }
}

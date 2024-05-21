using System;
using System.Collections.Specialized;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace RdvEncryption
{
    public class HelperModule
    {
        public static string EncryptConnectionString(string connection)
        {
            string output = RdvCryptography.Encrypt(connection);
            if (!String.IsNullOrEmpty(output))
                return output;
            throw new Exception("unable to Encrypt");
        }

        public static string DecryptConnectionString(string connection)
        {
            string output = RdvCryptography.Decrypt(connection);
            if (String.IsNullOrEmpty(output))
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

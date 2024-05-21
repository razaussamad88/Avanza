using System;
using System.Text;

namespace VigilusEncryption
{
    public class HelperModule
    {
        public static string EncryptConnectionString(string connection)
        {
            string output = String.Empty;
            try
            {
                output = new Cryptographer().Encrypt(connection);

                if (!String.IsNullOrEmpty(output))
                    return output;

                throw new Exception("unable to Encrypt");
            }
            catch (Exception ex)
            {
                GetFullExceptionForLog(ex);
            }

            return output;
        }

        public static string DecryptConnectionString(string connection)
        {
            string output = String.Empty;
            try
            {
                output = new Cryptographer().Decrypt(connection);

                if (String.IsNullOrEmpty(output))
                {
                    throw new Exception("Unable to get connection string");
                }

                return output.Replace("&quot;", "'");
            }
            catch (Exception ex)
            {
                GetFullExceptionForLog(ex);
            }

            return output;
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

using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EmailSender
{
    public class EmailSender
    {
        private const string MSG_FMT = "Sender: {0}; Receiver: {1}; SmtpHost: {2}; SmtpPort: {3};";

        public bool SentChangedPassword(string EmailID, string Password)
        {
            bool IsSent = false;
            //string clearPwd = new Cryptographer().Encrypt(ConfigurationManager.AppSettings["VisionEmailPassword"]).ToUpper();
            string clearPwd = new Cryptographer().Decrypt(ConfigurationManager.AppSettings["VisionEmailPassword"]);

            // SMTP Code to send email
            MailMessage messageOutput = new MailMessage();
            messageOutput.Subject = "Password Changed";
            messageOutput.Body = "Your New Password is " + Password;
            messageOutput.From = new MailAddress(ConfigurationManager.AppSettings["VisionEmailId"]);

            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SmtpHost"], Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]));
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = false;
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["VisionEmailId"], clearPwd);

            messageOutput.To.Clear();
            messageOutput.To.Add(EmailID);

            try
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("SENDING EMAIL ... ");
                msg.AppendLine(String.Format("Sender: {0}", messageOutput.From));
                msg.AppendLine(String.Format("Receiver: {0}", messageOutput.To));
                msg.AppendLine(String.Format("SmtpHost: {0}", client.Host));
                msg.AppendLine(String.Format("SmtpPort: {0}", client.Port));

                msg.AppendLine(String.Empty);

                //ActivityLogger.Log(MethodBase.GetCurrentMethod(), msg);
                Console.WriteLine(msg.ToString());

                client.Send(messageOutput);
                IsSent = true;

                //ActivityLogger.Log(MethodBase.GetCurrentMethod(), "Sent Successfully!");
            }
            catch (Exception ex)
            {
                // Log or display error
                IsSent = false;

                //ActivityLogger.Log(ex, MethodBase.GetCurrentMethod().Name);
                Console.WriteLine("ERROR!!");
                Console.WriteLine(GetInnerMessage(ex));
            }

            Console.WriteLine(String.Empty);
            return IsSent;
        }



        public static string GetInnerMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            if (ex.InnerException == null)
                return ex.Message;
            else
                sb.Append(GetInnerMessage(ex.InnerException));

            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace EmailSenderAsync
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);

        private static volatile object m_Lock = new object();

        public static volatile int m_RequestCounter = 0;
        public static volatile int m_ResponseCounter = 0;
        public static volatile int m_SendCompleted_Counter = 0;
        public static volatile int m_Error_Counter = 0;

        private static Mutex mutex = new Mutex();
        private static System.Timers.Timer m_mainTimer;

        public static string headLine;
        public static List<string> toRecepients = null;
        private static Queue<string> que_toRecepients = null;

        private static string SmtpHost = null;
        private static string SmtpPort = null;
        private static string SmtpUser = null;
        private static string SmtpPassword = null;
        private static string SmtpDomain = null;
        private static string EmailTimer = null;
        private static string Recepients = null;
        public static string FromAddress = null;
        private static string SmtpTimeout = null;
        private static string IsSendMailAwait = null;
        private static bool boo_IsSendMailAwait = false;
        public static bool IsAllResponseCompleted = false;
        public static bool IsSummaryPrinted = false;


        public const string subject = "Test Email RdvNotification";

        private static List<string> getAllRecepients()
        {
            string[] emailIds = Recepients?.TrimEnd(';').Split(';');

            List<string> ids = (from x in emailIds
                                select x).ToList();

            return ids;
        }

        static Program()
        {
            SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            SmtpPort = ConfigurationManager.AppSettings["SmtpPort"];
            SmtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            SmtpDomain = ConfigurationManager.AppSettings["SmtpDomain"];

            FromAddress = ConfigurationManager.AppSettings["FromAddress"];
            EmailTimer = ConfigurationManager.AppSettings["EmailTimer"];
            Recepients = ConfigurationManager.AppSettings["Recepients"];
            SmtpTimeout = ConfigurationManager.AppSettings["SmtpTimeout"];

            IsSendMailAwait = ConfigurationManager.AppSettings["IsSendMailAwait"];

            
            //string clearPwd = new Cryptographer().Decrypt(ConfigurationManager.AppSettings["VisionEmailPassword"]);

            if (String.IsNullOrEmpty(EmailTimer))
            {
                EmailTimer = "30"; // 30 second is default time
            }

            if (String.IsNullOrEmpty(SmtpTimeout))
            {
                SmtpTimeout = "15"; // 15 second is default time
            }

            if (!String.IsNullOrEmpty(IsSendMailAwait) && bool.TryParse(IsSendMailAwait, out boo_IsSendMailAwait))
            {

            }
        }

        static void Maximize()
        {
            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
        }

        static void Main(string[] args)
        {
            Maximize();

            try
            {
                m_mainTimer = new System.Timers.Timer();
                m_mainTimer.Interval = TimeSpan.FromSeconds(int.Parse(EmailTimer)).TotalMilliseconds;
                m_mainTimer.Elapsed += M_mainTimer_Elapsed; ;
                m_mainTimer.AutoReset = true;

                toRecepients = getAllRecepients();
                que_toRecepients = new Queue<string>();

                foreach (var toRecepient in toRecepients)
                {
                    que_toRecepients.Enqueue(toRecepient);
                }

                headLine = String.Format($"Host [{SmtpHost}:{SmtpPort}]  |  User [{SmtpUser}]  |  Domain [{SmtpDomain}]  |  Timeout [{SmtpTimeout}]\r\nFrom [{FromAddress}]  |  Subject [{subject}]");

                iConsole.WriteLine(String.Empty);
                iConsole.WriteLine(headLine);
                iConsole.WriteLine("Total Recipients : {0:00}", toRecepients.Count);
                iConsole.WriteLine(String.Empty.PadLeft(headLine.Length, '='));

                m_mainTimer.Start(); // Start
            }
            catch (Exception ex)
            {
                iConsole.WriteLine(ex.Message);
                Console.Write(ex.StackTrace);
            }

            Console.ReadLine();
        }

        static void M_mainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (m_Lock)
            {
                Task.Run(() => Execute());
            }
        }

        static void Execute()
        {
            try
            {
                mutex.WaitOne();

                if (que_toRecepients.Count > 0)
                {
                    if (boo_IsSendMailAwait)
                    {
                        SendMailAwait.SendEmail(que_toRecepients.Dequeue());
                    }
                    else
                    {
                        SendMail.SendEmail(que_toRecepients.Dequeue());
                    }
                }
            }
            catch (Exception ex)
            {
                iConsole.WriteLine(ex.Message);
                iConsole.WriteLine(ex.StackTrace);
            }
            finally { mutex.ReleaseMutex(); }
        }

        private static SmtpClient getSmtpClient_Exchange()
        {
            SmtpClient client = new SmtpClient(SmtpHost, Convert.ToInt32(SmtpPort));

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = false;

            if (String.IsNullOrEmpty(SmtpDomain))
                client.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
            else
                client.Credentials = new NetworkCredential(SmtpUser, SmtpPassword, SmtpDomain);

            return client;
        }

        private static SmtpClient getSmtpClient_Baseline(bool isRelay = false)
        {
            SmtpClient client = new SmtpClient(SmtpHost, int.Parse(SmtpPort));

            if (isRelay)
            {
                client.UseDefaultCredentials = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = false;
            }
            else
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;

                if (String.IsNullOrEmpty(SmtpDomain))
                    client.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
                else
                    client.Credentials = new NetworkCredential(SmtpUser, SmtpPassword, SmtpDomain);
            }


            return client;
        }

        public static SmtpClient getSmtpClient()
        {
            SmtpClient client = null;

            //if (client == null)
            {
                if (SmtpHost.EndsWith(".gmail.com"))
                {
                    client = getSmtpClient_Baseline();
                    //client.Timeout = (int)TimeSpan.FromSeconds(int.Parse(SmtpTimeout)).TotalMilliseconds;

                    // for testing
                    client.Timeout = (int)TimeSpan.FromMilliseconds(int.Parse("200")).TotalMilliseconds;
                }
                else
                {
                    client = getSmtpClient_Exchange();
                    client.Timeout = (int)TimeSpan.FromSeconds(int.Parse(SmtpTimeout)).TotalMilliseconds;
                }


                if (boo_IsSendMailAwait)
                {
                    client.SendCompleted += SendMailAwait.Client_SendCompleted;
                }
                else
                {
                    client.SendCompleted += SendMail.Client_SendCompleted;
                }
            }

            return client;
        }
    }
}

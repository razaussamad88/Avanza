using NATS.Client;
using NATS.Client.JetStream;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Nats_Messaging
{
    public class NATsManager
    {
        //public const string c_STREAM_ASPIRE_WEBUI = "ASPIREWEBUI";
        public const string c_WRITERSRC_USERINTERFACE = "userinterface-test";

        private const int c_REQUEST_REPLY_TIMEOUT = 5000;
        public const int c_NATS_DEFAULT_TIMEOUT = 5000;
        public const int c_UI_PUSH_SUBSCRIPTION_TIMER = 1000;

        #region Members

        public static IAsyncSubscription NatsAsyncSubscription = null;

        public static string sNatsStreamUI;// = "127.0.0.1:4222";
        public static string sNatsUrl;// = "127.0.0.1:4222";
        public static string sNatsUser;
        public static string sNatsPwd;
        private static IConnection natsCon;
        private static Options natsOpts;
        private static ConnectionFactory natsCf;
        private static volatile bool natsServerConnected;
        private static int noOfThreads;
        private static IJetStreamPushSyncSubscription[] subscriberList;

        public NATsQueue natsQueue;
        private Thread natsUIPushSubscriberSyncThread;
        private Thread connectionThread;

        private static NATsManager m_Instance = null;
        #endregion


        #region Properties

        public static bool IsNatsConnected
        {
            get
            {
                if ((natsCon != null) && (natsCon.State == ConnState.CONNECTED))
                    return true;

                return false;
            }
        }

        public static IConnection NatsConnection
        {
            get { return natsCon; }
        }
        #endregion


        #region Constructors

        static NATsManager()
        {
            sNatsStreamUI = ConfigurationManager.AppSettings["sNatsStreamUI"].ToString();

            sNatsUrl = ConfigurationManager.AppSettings["NatsAddress"].ToString();
            sNatsUser = ConfigurationManager.AppSettings["NatsUser"].ToString();
            //sNatsPwd = CryptoHelper.Decrypt(ConfigurationManager.AppSettings["NatsPassword"].ToString());
            sNatsPwd = ConfigurationManager.AppSettings["NatsPassword"].ToString();

            if (m_Instance == null)
            {
                m_Instance = new NATsManager();
            }
        }

        public NATsManager()
        {
            natsQueue = new NATsQueue(sNatsStreamUI, ref natsServerConnected);
        }
        #endregion


        #region Private Static Methods

        public void InitializeCertificate()
        {
            try
            {
                Logger.LogInfo("Initiliazing Certificate parameters for Digital Signing");

                string LdapServer = ConfigurationManager.AppSettings["SigServer"].ToString();
                string LdapPort = ConfigurationManager.AppSettings["SigPort"].ToString();
                string BasePath = ConfigurationManager.AppSettings["SigBasePath"].ToString();
                string CertPath = ConfigurationManager.AppSettings["SigCertificate"].ToString();
                //string CertPass = CryptoHelper.Decrypt(ConfigurationManager.AppSettings["SigPassword"].ToString());

                /*
				DigitalSigning.Instance.OnLog += new LogEventHandler(Instance_OnLog);
                DigitalSigning.Instance.Init(LdapServer, LdapPort, BasePath, CertPath, CertPass);
                DigitalSigning.Instance.Start();
				*/
            }
            catch (Exception ex)
            {
                Logger.LogError("Error Occured while initializing Certificate for Digital Signing Error: {0}", ex.Message);
            }
        }

        public static void Start()
        {
            Logger.LogInfo("Entered in Start Function.");

            int threadId = Thread.CurrentThread.ManagedThreadId;

            try
            {
                m_Instance.InitializeNats(threadId);
                m_Instance.connectionThread = new Thread(m_Instance.MonitorNatsConnection);
                m_Instance.connectionThread.IsBackground = true;
                m_Instance.connectionThread.Start();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error: {0}", ex.Message);
                throw ex;
            }

            Logger.LogInfo("Waiting for nats connection");
            while (!NATsManager.IsNatsConnected) { }
            Logger.LogInfo("Connection successful ");

            noOfThreads = int.Parse(ConfigurationManager.AppSettings["noOfSubscribers"]);
            subscriberList = new IJetStreamPushSyncSubscription[noOfThreads];
            m_Instance.natsQueue.Initialize(noOfThreads, ref subscriberList);

            for (int i = 0; i < noOfThreads; i++)
            {
                m_Instance.natsUIPushSubscriberSyncThread = new Thread(new ParameterizedThreadStart(m_Instance.natsQueue.UIPushSubscriberSync));
                m_Instance.natsUIPushSubscriberSyncThread.IsBackground = true;
                m_Instance.natsUIPushSubscriberSyncThread.Start(i);
            }

            NatsAsyncSubscription = NATsManager.NatsConnection.SubscribeAsync(String.Concat(NATS_Param.Stream.ASPIREWEBUI, ".*"), m_Instance.natsQueue.SubscribeAsyncInboxHandler);
        }

        public void Stop()
        {
            Logger.LogInfo("Entered in Stop Function.");
            int threadId = Thread.CurrentThread.ManagedThreadId;

            for (int i = 0; i < noOfThreads; i++)
            {
                if ((subscriberList[i] != null) && (subscriberList[i].IsValid))
                    subscriberList[i].Unsubscribe();
            }


            if (NatsAsyncSubscription != null)
                NatsAsyncSubscription.Unsubscribe();


            CloseNats(threadId);
            Logger.LogInfo("Service Stopped.");
        }

        public void ConnectToNats(int threadId)
        {
            Logger.LogInfo("Entered in ConnectToNats Function, Thread [{0}].", threadId);

            if ((natsCon == null) || (natsCon.State != ConnState.CONNECTED))
            {
                natsServerConnected = false;
                try
                {
                    if (natsCf == null)
                        natsCf = new ConnectionFactory();

                    natsCon = natsCf.CreateConnection(natsOpts);
                    Logger.LogInfo("ConnectToNats natsCon {0}", natsCon);

                    if (natsCon != null)
                    {
                        Logger.LogInfo("*** Connected to Nats server ***");
                        Logger.LogInfo("Nats server connection: {0} - ClientIP: {1} - Subscribers: {2}", natsCon.State, natsCon.ClientIP, natsCon.SubscriptionCount);
                        natsServerConnected = true;
                    }
                    else
                        Logger.LogInfo("--- Unable to connect to Nats server ---");
                }
                catch (Exception ex)
                {
                    Logger.LogError("--- Not connected to Nats server ---");
                    //throw exception;
                }
            }
        }

        public void MonitorNatsConnection()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Logger.LogInfo("Entered in MonitorNatsConnection Function, Thread [{0}].", threadId);

            try
            {
                int iRetryInterval = c_NATS_DEFAULT_TIMEOUT;

                while (true)
                {
                    if ((!natsServerConnected) || (natsCon == null) || ((natsCon != null) && (natsCon.State != ConnState.CONNECTED)))
                    {
                        Logger.LogInfo("connecting nats");
                        ConnectToNats(threadId);
                    }
                    if (natsServerConnected)
                        Thread.Sleep(200); // 200
                    else
                    {
                        Logger.LogInfo("Connection to server failed, Trying again in {0} seconds", iRetryInterval);
                        Thread.Sleep(iRetryInterval);
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                Logger.LogInfo("Aborted the thread.");
                return;
            }
            catch (Exception ex)
            {
                Logger.LogError("Stopped the thread.");
            }
        }
        #endregion


        #region Private Methods

        private void Instance_OnLog(string pMessage)
        {
            Logger.LogInfo(pMessage);
        }

        private void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
        }

        private void InitializeNats(int threadId)
        {
            Logger.LogInfo("Entered in InitializeNats function, Thread [{0}].", threadId);

            try
            {
                SetCertificatePolicy();
                natsOpts = ConnectionFactory.GetDefaultOptions();
                natsOpts.Url = sNatsUrl;
                natsOpts.User = sNatsUser;
                natsOpts.Password = sNatsPwd;
                natsOpts.ClosedEventHandler += NatsClosedEventHandler;
                natsOpts.DisconnectedEventHandler += NatsDisconnectedEventHandler;
                natsCf = new ConnectionFactory();

                Logger.LogInfo("InitializeNats natsOpts Url {0}", natsOpts.Url);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error: {0}", ex.Message);
                throw ex;
            }
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            //Debug.WriteLine("Trusting X509Certificate '{0}'", cert.Subject);
            return true;
        }

        private static void NatsDisconnectedEventHandler(object sender, ConnEventArgs eArgs)
        {
            Logger.LogInfo("Entered in NatsDisconnectedEventHandler.");
            Logger.LogInfo("Connection State: {0}", eArgs.Conn.State.ToString());
        }

        private static void NatsClosedEventHandler(object sender, ConnEventArgs eArgs)
        {
            Logger.LogInfo("Entered in NatsClosedEventHandler.");
            Logger.LogInfo("Connection State: {0}", eArgs.Conn.State.ToString());
        }

        private static void Publish(string stream, string queue_subject, UIQueueMessage message)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Logger.LogInfo("Entered in PublishMessage Function");

            try
            {
                if (stream == string.Empty)
                    Logger.LogInfo("Queue name is not specified, Nats publish failed.");

                if (NATsManager.IsNatsConnected)
                {
                    try
                    {
                        queue_subject = String.Concat(stream, ".", queue_subject);

                        var itm = Newtonsoft.Json.JsonConvert.SerializeObject(message);

                        byte[] payload = Encoding.ASCII.GetBytes(itm);

                        MsgHeader msg = new MsgHeader();
                        msg.Add("WriterSource", c_WRITERSRC_USERINTERFACE);
                        Msg natsMessage = new Msg(queue_subject, msg, payload);
                        NATsManager.NatsConnection.Publish(natsMessage);

                        Logger.LogInfo("Published Message over {0} : Nats Subject: {1}", stream, queue_subject);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Error: Failed to publish message. {0}", ex.Message);
                    }
                }
                else
                {
                    // what to do when nats is not connected ?? wait or retry
                    Logger.LogInfo("Nats server is not connected");
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        private static UIQueueMessage Request(string stream, UIQueueMessage message)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Logger.LogInfo("Entered in PublishMessage Function");

            UIQueueMessage retVal = null;

            try
            {
                if (stream == string.Empty)
                    Logger.LogInfo("Queue name is not specified, Nats publish failed.");

                if (NATsManager.IsNatsConnected)
                {
                    string inbox_subject = Enum.GetName(typeof(NATS_Param.Subject), NATS_Param.Subject.REQUEST_REPLY).ToString();

                    try
                    {
                        inbox_subject = String.Concat(stream, ".", inbox_subject);

                        var itm = Newtonsoft.Json.JsonConvert.SerializeObject(message);

                        byte[] payload_Req = Encoding.ASCII.GetBytes(itm);

                        MsgHeader msg = new MsgHeader();
                        msg.Add("WriterSource", c_WRITERSRC_USERINTERFACE);
                        msg.Add("MethodName", message.MethodName);
                        Msg natsMessage = new Msg(inbox_subject, msg, payload_Req);

                        //asyncSubscription = NATsManager.NatsConnection.SubscribeAsync(String.Concat(NATS_Param.Stream.ASPIREWEBUI, NATS_Param.Subject.REQUEST_REPLY), m_Instance.natsQueue.SubscribeAsyncInboxHandler);
                        //asyncSubscription = NATsManager.NatsConnection.SubscribeAsync(String.Concat(NATS_Param.Stream.ASPIREWEBUI, ".*"), m_Instance.natsQueue.SubscribeAsyncInboxHandler);
                        var reply = NATsManager.NatsConnection.Request(natsMessage, c_REQUEST_REPLY_TIMEOUT);

                        var payload_Res = Encoding.ASCII.GetString(reply.Data);
                        retVal = Newtonsoft.Json.JsonConvert.DeserializeObject<UIQueueMessage>(payload_Res);

                        Logger.LogInfo("Published Message over Inbox : Nats Subject: {0}", inbox_subject);
                    }
                    catch (NATSTimeoutException)
                    {

                    }
                    catch (NATSException ex)
                    {
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Error: Failed to publish message. {0}", ex.Message);
                    }
                    finally
                    {
                        //if (asyncSubscription != null)
                        //    asyncSubscription.Unsubscribe();
                    }
                }
                else
                {
                    // what to do when nats is not connected ?? wait or retry
                    Logger.LogInfo("Nats server is not connected");
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }

            return retVal;
        }
        #endregion


        #region Public Static Methods

        public static void CloseNats(int threadId)
        {
            if (natsCon != null && (!natsCon.IsClosed()))
            {
                natsCon.Flush(c_NATS_DEFAULT_TIMEOUT); //makes sure all messages are sent over 
            }
            Logger.LogInfo("Nats connection closed...");
        }

        public static void Publish(NATS_Param.Stream stream, NATS_Param.Subject subject, UIQueueMessage message)
        {
            Publish(Enum.GetName(typeof(NATS_Param.Stream), stream).ToString(), Enum.GetName(typeof(NATS_Param.Subject), subject).ToString(), message);
        }

        public static UIQueueMessage Request(NATS_Param.Stream stream, UIQueueMessage message)
        {
            return Request(Enum.GetName(typeof(NATS_Param.Stream), stream).ToString(), message);
        }

        #endregion
    }
}

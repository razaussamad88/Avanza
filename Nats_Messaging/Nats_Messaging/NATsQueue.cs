using NATS.Client;
using NATS.Client.JetStream;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Nats_Messaging
{
    public class NATsQueue
    {
        public static string sNatsStream;
        public static string sNatsSubject;
        private static StorageType StorageType;
        private static DiscardPolicy DiscardPolicy;
        private static RetentionPolicy RetentionPolicy;
        private static bool DenyDelete;
        private static bool AllowRollup;
        private static int Replicas;
        private static int MessageLimit;
        private static int MessageLimitPerSubject;
        private static int TotalStreamSize;
        private static int MessageTTL;
        private static int MaxMessageSize;
        private static int DuplicateWindow;
        private static bool AllowPurge;

        private static IJetStream natsJetStream;
        private static IJetStreamManagement natsJsManager;
        private static StreamConfiguration natsStreamConfig;
        private static ConsumerConfiguration natsConsumerConfig;
        private static PushSubscribeOptions natsPushSubscribeOpts;
        private static IJetStreamPushSyncSubscription natsJsSyncSubs;
        private static IJetStreamPushSyncSubscription[] natsSubsribersList;
        private static ClusterInfo natsClusterInfo;

        public string queueName;
        private static StreamInfo natsStreamInfo;
        private Thread msgReceiverThread;

        private static object hold = new object();
        private List<String> natsTopic;

        private static volatile object m_Lock = new object();

        public NATsQueue(string streamName, ref bool natsConnStatus)
        {
            queueName = streamName;
            natsConnStatus = NATsManager.IsNatsConnected;
        }

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

                //DigitalSigning.Instance.OnLog += new LogEventHandler(Instance_OnLog);
                //DigitalSigning.Instance.Init(LdapServer, LdapPort, BasePath, CertPath, CertPass);
                //DigitalSigning.Instance.Start();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error Occured while initializing Certificate for Digital Signing Error: {0}", ex.Message);
            }
        }

        private void Instance_OnLog(string pMessage)
        {
            Logger.LogInfo(pMessage);
        }

        public void Initialize(int subscriberCount, ref IJetStreamPushSyncSubscription[] _natsSubsribersList)
        {
            Logger.LogInfo("Entered in Initialize Function.");

            try
            {
                // configuring nats connection variable from config file and registry
                // stream and subject name is same as queue name received from registry
                sNatsStream = queueName;

                if (sNatsStream.ToUpper().Contains(NATsManager.sNatsStreamUI.ToUpper()))
                { sNatsSubject = ConfigurationManager.AppSettings["sNatsSubjectsUI"]; }
                else
                { throw new Exception("This Stream not supported."); }
            }
            catch (Exception ex)
            {
                Logger.LogError("Nats sNatsStream is not found ");
                Logger.LogError("Error: {0}", ex.Message);
            }

            int threadId = Thread.CurrentThread.ManagedThreadId;

            natsSubsribersList = _natsSubsribersList;
            InitializeNats(threadId);
        }

        private T getConfigItem<T>(string keyName)
        {
            String keyValue = ConfigurationManager.AppSettings[keyName];
            return (T)Enum.Parse(typeof(T), keyValue);
        }

        private void InitializeNats(int threadId)
        {
            Logger.LogInfo("Entered in InitializeNats function, Thread [{0}].", threadId);
            try
            {
                // defining stream configurations
                /*
                String TempStorageType = ConfigurationManager.AppSettings["StorageType"];
                StorageType = (StorageType)Enum.Parse(typeof(StorageType), TempStorageType);

                String TempDiscard = ConfigurationManager.AppSettings["DiscardPolicy"];
                DiscardPolicy = (DiscardPolicy)Enum.Parse(typeof(DiscardPolicy), TempDiscard);

                String TempRetention = ConfigurationManager.AppSettings["RetentionPolicy"];
                RetentionPolicy = (RetentionPolicy)Enum.Parse(typeof(RetentionPolicy), TempRetention);
                */

                StorageType = this.getConfigItem<StorageType>("StorageType");
                DiscardPolicy = this.getConfigItem<DiscardPolicy>("DiscardPolicy");
                RetentionPolicy = this.getConfigItem<RetentionPolicy>("RetentionPolicy");

                DenyDelete = bool.Parse(ConfigurationManager.AppSettings["DenyDeletion"]);
                AllowRollup = bool.Parse(ConfigurationManager.AppSettings["AllowRollup"]);
                Replicas = int.Parse(ConfigurationManager.AppSettings["Replicas"]);
                MessageLimit = int.Parse(ConfigurationManager.AppSettings["MessageLimit"]);

                MessageLimitPerSubject = int.Parse(ConfigurationManager.AppSettings["MessageLimitPerSubject"]);
                TotalStreamSize = int.Parse(ConfigurationManager.AppSettings["TotalStreamSize"]);
                MessageTTL = int.Parse(ConfigurationManager.AppSettings["MessageTTL"]);
                MaxMessageSize = int.Parse(ConfigurationManager.AppSettings["MaxMessageSize"]);

                DuplicateWindow = int.Parse(ConfigurationManager.AppSettings["DuplicateWindow"]);
                AllowPurge = bool.Parse(ConfigurationManager.AppSettings["AllowPurge"]);

                natsStreamConfig = StreamConfiguration.Builder()
                        .WithName(sNatsStream)
                        .WithSubjects(sNatsSubject.Split(',').ToList<string>())
                        .WithStorageType(StorageType)
                        .WithDenyDelete(DenyDelete)
                        .WithAllowRollup(AllowRollup)
                        .WithDiscardPolicy(DiscardPolicy)
                        .WithRetentionPolicy(RetentionPolicy)
                        .WithMaxAge(MessageTTL)
                        .WithReplicas(Replicas)
                        .WithMaxMsgSize(MaxMessageSize)
                        .WithMaxMessages(MessageLimit)
                        .WithMaxMessagesPerSubject(MessageLimitPerSubject)
                        .WithDuplicateWindow(DuplicateWindow)
                        .WithMaxBytes(TotalStreamSize)
                        .Build();

                // defining consumer configurations
                natsConsumerConfig = ConsumerConfiguration.Builder()
                            .WithAckPolicy(AckPolicy.Explicit)
                            .WithDurable(sNatsStream + "Durable")
                            .WithFilterSubject(sNatsStream + "FREE_REQUESTS")
                            .WithDeliverGroup(sNatsStream + "Group")
                            .WithMaxAckPending(200)
                            .WithAckWait(60000)
                            .Build();

                natsPushSubscribeOpts = PushSubscribeOptions.Builder()
                    .WithStream(sNatsStream)
                    .WithConfiguration(natsConsumerConfig)
                    .Build();

                //IConnection nats = NATsManager.NatsConnection;
                natsJsManager = NATsManager.NatsConnection.CreateJetStreamManagementContext();

                try
                {
                    natsStreamInfo = natsJsManager.GetStreamInfo(sNatsStream); // this throws if the stream does not exist
                }
                catch (NATSJetStreamException ex)
                {
                    Logger.LogError("Nats stream [{0}] does not exist.", sNatsStream);
                }

                if (natsStreamInfo == null)
                {
                    natsStreamInfo = natsJsManager.AddStream(natsStreamConfig);
                }

                //natsJetStream = NATsManager.NatsConnection.CreateJetStreamContext();
                natsJetStream = NATsManager.NatsConnection.CreateJetStreamContext();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error: {0}", ex.Message);
                throw ex;
            }
        }

        public void RefreshSubscription(int threadId, ref IJetStreamPushSyncSubscription subscriberHandle)
        {
            Logger.LogInfo("Entered in RefreshSubscription Function, Thread [{0}].", threadId);

            try
            {
                lock (hold)
                {
                    if (!NATsManager.IsNatsConnected)
                    {
                        Logger.LogInfo("--- Unable to connect to Nats server ---");
                    }
                    if (NATsManager.IsNatsConnected)
                    {
                        var que = String.Concat(sNatsStream, ".>");
                        var sub = String.Concat(sNatsStream, "Group");
                        subscriberHandle = natsJetStream.PushSubscribeSync(que, sub, natsPushSubscribeOpts);

                        //var que = String.Concat(sNatsStream, "Group");
                        //var sub = String.Concat(sNatsStream, ".AWAIT_RESPONSE");

                        //  IJetStreamPushSyncSubscription PushSubscribeSync(string subject, string queue, PushSubscribeOptions options);
                        //subscriberHandle = natsJetStream.PushSubscribeSync(sNatsStream + ".>", sNatsStream + "Group", natsPushSubscribeOpts);
                        //subscriberHandle = natsJetStream.PushSubscribeSync(sub, que, natsPushSubscribeOpts);

                        if (subscriberHandle != null)
                        {
                            Logger.LogInfo("Nats stream [{0}] subscription successful on NATS server.", sNatsStream);
                        }
                        else
                        {
                            Logger.LogInfo("Nats stream [{0}] subscription unsuccessful on NATS server.", sNatsStream);
                        }
                    }
                }
                if (!NATsManager.IsNatsConnected)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in Subscriber: {0}", ex.Message);
            }
        }

        public void UIPushSubscriberSync(object param)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            int index = int.Parse(param.ToString());

            Logger.LogInfo("Entered in PushSubscriber Function, Thread [{0}].", threadId);

            IJetStreamPushSyncSubscription natsJsSyncSubHandle = natsSubsribersList[index];

            Logger.LogInfo("natsServerConnected state : {0}", NATsManager.IsNatsConnected);

            try
            {
                while (!NATsManager.IsNatsConnected)
                {
                    Logger.LogInfo("Waiting for nats connection...");
                    Thread.Sleep(1000);
                }

                RefreshSubscription(threadId, ref natsJsSyncSubHandle);

                while (true)
                {
                    Msg msg = null;
                    bool foundJsMsg = FetchNewMessage(threadId, ref natsJsSyncSubHandle, out msg);

                    if (foundJsMsg)
                    {
                        foundJsMsg = ProcessMessage(ref msg);
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                Logger.LogError("Aborted the thread.");
                return;
            }
            catch (Exception ex)
            {
                Logger.LogError("Stopped the thread.");
            }
        }

        private bool ProcessMessage(ref Msg msg)
        {
            bool foundJsMsg = true;

            try
            {
                string text = Encoding.UTF8.GetString(msg.Data);

                if (msg.Header["WriterSource"].ToUpper().Equals(NATsManager.c_WRITERSRC_USERINTERFACE.ToUpper()))
                {
                    Logger.LogInfo("Message received from Gateway - Incoming Subject: {0}", msg.Subject);

                    /*
                        DO YOUR WORK HERE - AFTER MESSAGE RECEIVE 
                     */
                    //NATsManager.Drain();
                    msg.Ack(); //Temporay quick ack, should be after processing in Production
                }
                else
                {
                    Logger.LogError("Message received from Unknown source. Unable to process incoming message");
                }

                Logger.LogInfo("Message processed and acknowledged ");
                foundJsMsg = false;
                msg = null;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error: Failed to parse message: {0}", ex.Message);
                Thread.Sleep(1000);
            }

            return foundJsMsg;
        }

        private bool FetchNewMessage(int threadId, ref IJetStreamPushSyncSubscription natsJsSyncSubHandle, out Msg msg)
        {
            bool foundJsMsg = false;
            msg = null;

            if (NATsManager.IsNatsConnected)
            {
                if (!foundJsMsg)
                {
                    try
                    {
                        if (natsJsSyncSubHandle != null)
                        {
                            if (natsJsSyncSubHandle.DeliverSubject.ToUpper().StartsWith("_INBOX."))
                            {
                                foundJsMsg = false;
                                return foundJsMsg;
                            }

                            msg = natsJsSyncSubHandle.NextMessage(NATsManager.c_NATS_DEFAULT_TIMEOUT); // next msg timeout 
                            foundJsMsg = true;

                            msg.Ack();
                        }
                    }
                    catch (NATSTimeoutException ex)
                    {
                        Thread.Sleep(200);
                    }
                    catch (Exception ex)
                    {
                        foundJsMsg = false;
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    // Some message found at NATS Server
                    Thread.Sleep(100);
                }
            }
            else
            {
                // NATS Server disconnected
                RefreshSubscription(threadId, ref natsJsSyncSubHandle);
            }

            return foundJsMsg;
        }

        public void SubscribeAsyncInboxHandler(object sender, MsgHandlerEventArgs args)
        {
            if (String.IsNullOrEmpty(args.Message.Reply))
            {
                return;
            }


            if (args.Message.HasHeaders && args.Message.Header["WriterSource"].ToUpper().Equals(NATsManager.c_WRITERSRC_USERINTERFACE.ToUpper()))
            {
                string response = String.Empty;

                var payload = Encoding.ASCII.GetString(args.Message.Data);

                if (args.Message.Subject.ToUpper().Equals("ASPIREWEBUI.REQUEST_REPLY") &&
                    args.Message.Header["MethodName"].ToUpper().Equals("UPDATESWITCHSTATUS"))
                {
                    var qMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<UIQueueMessage>(payload);
                    qMessage.AddReturnParam(typeof(int), 014);

                    response = Newtonsoft.Json.JsonConvert.SerializeObject(qMessage);
                }

                NATsManager.NatsConnection.Publish(args.Message.Reply, Encoding.UTF8.GetBytes(response));
                args.Message.Ack();
            }
        }
    }
}

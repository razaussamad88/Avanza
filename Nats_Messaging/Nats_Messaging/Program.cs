using NATS.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nats_Messaging
{
    class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_MAXIMIZE = 3;


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

        static void Main_(string[] main_args)
        {
            string natsUrl = Environment.GetEnvironmentVariable("NATS_URL");
            string sNatsUser = String.Empty, sNatsPwd = String.Empty;

            if (natsUrl == null)
            {
                natsUrl = ConfigurationManager.AppSettings["NatsAddress"];
                sNatsUser = ConfigurationManager.AppSettings["NatsUser"];
                sNatsPwd = ConfigurationManager.AppSettings["NatsPassword"];
            }


            Options opts = ConnectionFactory.GetDefaultOptions();
            opts.Url = natsUrl;
            opts.User = sNatsUser;
            opts.Password = sNatsPwd;
            opts.ClosedEventHandler = NatsClosedEventHandler;
            opts.DisconnectedEventHandler = NatsDisconnectedEventHandler;


            var str = Newtonsoft.Json.JsonConvert.SerializeObject(opts);

            ConnectionFactory cf = new ConnectionFactory();
            IConnection c = cf.CreateConnection(opts);



            EventHandler<MsgHandlerEventArgs> handler = (sender, args) =>
            {
                string name = args.Message.Subject.Substring(6);
                string response = $"hello {name}";
                c.Publish(args.Message.Reply, Encoding.UTF8.GetBytes(response));
            };
            IAsyncSubscription sub = c.SubscribeAsync("greet.*", handler);


            try
            {
                //MsgHeader msgHdr = new MsgHeader();
                //msgHdr.Add("xxxx","val");

                //Msg msg = new Msg()
                //{
                //    Data = "Header",
                //    Header = "Header",
                //};

                Msg m0 = c.Request("greet.bob", null, 5000);
                Console.WriteLine("Response received: " + Encoding.UTF8.GetString(m0.Data));
            }
            catch (NATSTimeoutException ex)
            {
                Console.WriteLine($"NATSTimeoutException: The request did not complete in time.");
            }
            catch (Exception ex)
            {

            }


            Task<Msg> task1 = c.RequestAsync("greet.pam", null);
            task1.Wait(1000);
            Msg m1 = task1.Result;
            Console.WriteLine("Response received: " + Encoding.UTF8.GetString(m1.Data));




            //-----------------

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            Console.Title = "NATS Messaging System";
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, SW_MAXIMIZE);

            NATsManager.Start();
            ConsoleKeyInfo? consoleInput = null;
            UIQueueMessage qMsg = null;
            bool? is_REQUEST_REPLY = null;
            bool isExit = false;

            do
            {
                if (consoleInput != null)
                {
                    try
                    {
                        is_REQUEST_REPLY = null;
                        Console.WriteLine();
                        qMsg = SwitchCase(consoleInput, out is_REQUEST_REPLY, out isExit);

                        if (isExit)
                            break;

                        if (qMsg!=null)
                        {
                            if (is_REQUEST_REPLY.HasValue && is_REQUEST_REPLY.Value)
                            {
                                Console.Write("Requesting...");

                                var replyObj = NATsManager.Request(NATS_Param.Stream.ASPIREWEBUI, qMsg);

                                Console.WriteLine(" Response Received!");
                                Console.WriteLine("  -> your NATS replied value : {0}", replyObj?.ParamReturn?.ParamValue);
                            }
                            else
                            {
                                Console.Write("Publishing...");
                                //NATsManager.Publish(NATS_Param.Stream.ASPIREWEBUI, NATS_Param.Subject.FREE_REQUESTS, qMsg);
                                NATsManager.Publish(NATS_Param.Stream.ASPIREWEBUI, NATS_Param.Subject.AWAIT_REQUESTS, qMsg);
                                Console.Write(" Publish Succeed!");
                            } 
                        }


                        Console.WriteLine(Environment.NewLine + Environment.NewLine);
                        Console.WriteLine("".PadRight(50, '*'));
                    }
                    catch (Exception ex) { }
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Press [1] : To UpdateSwitchStatus");
                sb.AppendLine("Press [2] : To RefreshParticipantAccount");
                sb.AppendLine("Press [3] : To FetchBillerList");
                sb.AppendLine();
                sb.AppendLine("Press [0] or ENTER : To Exit...");
                Console.WriteLine(sb);
                Console.Write("your entered key : ");

                consoleInput = Console.ReadKey();
                ;
            } while (true);

            Console.WriteLine();
            Console.WriteLine("you are exiting...");
            Thread.Sleep(3000);
        }

        private static UIQueueMessage SwitchCase(ConsoleKeyInfo? consoleInput, out bool? is_REQUEST_REPLY, out bool isExit)
        {
            UIQueueMessage qMsg = null;
            is_REQUEST_REPLY = null;
            isExit = false;

            switch (consoleInput.Value.Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    {
                        qMsg = new UIQueueMessage("UpdateSwitchStatus");
                        qMsg.AddParam(typeof(int), "_status", 0);
                        //qMsg.AddReturnParam(typeof(int));
                        is_REQUEST_REPLY = true;
                    }
                    break;

                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    {
                        qMsg = new UIQueueMessage("RefreshParticipantAccount");
                    }
                    break;

                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    {
                        qMsg = new UIQueueMessage("FetchBillerList");
                        //qMsg.AddReturnParam(typeof(bool));
                    }
                    break;


                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                case ConsoleKey.Enter:
                    isExit = true;
                    break;

                default:
                    Console.WriteLine("Key not found!");
                    break;
            }

            return qMsg;
        }
    }
}

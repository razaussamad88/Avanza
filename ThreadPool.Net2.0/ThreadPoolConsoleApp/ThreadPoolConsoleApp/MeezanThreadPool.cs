using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPoolConsoleApp
{
    public class MeezanThreadPool
    {
        static void Main()
        {
            const double CONCURRENT_THREAD_LIMIT = 64;

            Logger.Init();

            var batchSize = (int)Math.Ceiling(Data.PAN_List.Count / CONCURRENT_THREAD_LIMIT);
            int batchCounter = 0;

            Logger.WriteLine("Launching {0} tasks...", CONCURRENT_THREAD_LIMIT);
            Logger.WriteLine("Calling block for GenerateHSM_Bulk started...");

            Queue<string> que_Pans = new Queue<string>();

            foreach (var itm in Data.PAN_List)
                que_Pans.Enqueue(itm);


            while (batchCounter < batchSize)
            {
                batchCounter++;

                List<ManualResetEvent> doneEvents = new List<ManualResetEvent>();

                Logger.WriteLine("\tBatch {0:00} started...", batchCounter);

                for (int i = 0; i < CONCURRENT_THREAD_LIMIT; i++)
                {
                    if (que_Pans.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        var pan = que_Pans.Dequeue();

                        doneEvents.Add(new ManualResetEvent(false));

                        var th_Card = new ThreadStruct(pan, doneEvents[i]);

                        ThreadContext thCtx = new ThreadContext()
                        {
                            ThreadIndex = i,
                            Pan = pan,
                            Card = new DebitCard() { Pan = pan },
                            IsEMVCard = false,
                            Imd = pan.Substring(0, 6),
                            CustPin = new CustomerChannelAuthen() { Pan = pan }
                        };

                        th_Card.MeezanStrategyEvent += new MBLStrategy().Calculate;

                        ThreadPool.QueueUserWorkItem(th_Card.ThreadPoolCallback, thCtx);
                    }
                }

                WaitHandle.WaitAll(doneEvents.ToArray());

                Logger.WriteLine("\tBatch {0:00} Completed!", batchCounter);
            }

            Logger.WriteLine("Call block for GenerateHSM_Bulk completed!");
            Logger.WriteLine("All calculations are complete.");

            Logger.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine);
            Logger.WriteLine("Press ENTER to exit...");

            Console.ReadLine();
        }
    }
}

using System;
using System.Threading;

namespace ThreadPoolConsoleApp
{

    public class Fibonacci
    {
        private ManualResetEvent _doneEvent;

        public Fibonacci(int n, ManualResetEvent doneEvent)
        {
            N = n;
            _doneEvent = doneEvent;
        }

        public int N;

        public int FibOfN { get; private set; }

        public void ThreadPoolCallback(Object threadContext)
        {
            int threadIndex = (int)threadContext;
            Console.WriteLine("Thread {0} started...", threadIndex);
            FibOfN = Calculate(N);
            Console.WriteLine("Thread {0} result calculated...", threadIndex);
            _doneEvent.Set();
        }

        public int Calculate(int n)
        {
            if (n <= 1)
            {
                return n;
            }
            return Calculate(n - 1) + Calculate(n - 2);
        }
    }

    public class ThreadPoolExample
    {
        static void Main()
        {
            const int FibonacciCalculations = 64;

            var doneEvents = new ManualResetEvent[FibonacciCalculations];
            var fibArray = new Fibonacci[FibonacciCalculations];
            var rand = new Random();

            Console.WriteLine("Launching {0} tasks...", FibonacciCalculations);
            for (int i = 0; i < FibonacciCalculations; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                var f = new Fibonacci(rand.Next(20, 40), doneEvents[i]);
                fibArray[i] = f;
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
            }

            WaitHandle.WaitAll(doneEvents);
            Console.WriteLine("All calculations are complete.");

            for (int i = 0; i < FibonacciCalculations; i++)
            {
                Fibonacci f = fibArray[i];
                Console.WriteLine("Fibonacci({0}) = {1}", f.N, f.FibOfN);
            }

            Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine);
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}

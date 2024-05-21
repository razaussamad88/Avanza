using System;
using System.Threading;

namespace ThreadPoolConsoleApp
{
    public class ThreadStruct
    {
        private ManualResetEvent _doneEvent;

        public ThreadStruct(string pan, ManualResetEvent doneEvent)
        {
            PAN = pan;
            _doneEvent = doneEvent;
        }

        public string PAN;

        public delegate void MeezanStrategy(string imd, DebitCard card, CustomerChannelAuthen custPin, bool isEMVCard);
        public event MeezanStrategy MeezanStrategyEvent;

        public void ThreadPoolCallback(object threadObject)
        {
            ThreadContext threadContext = (ThreadContext)threadObject;

            Logger.WriteLine("\t\tThread {0} started...", threadContext.ThreadIndex);

            if (MeezanStrategyEvent != null)
                MeezanStrategyEvent(threadContext.Imd, threadContext.Card, threadContext.CustPin, threadContext.IsEMVCard);

            Logger.WriteLine("\t\tThread {0} result calculated...", threadContext.ThreadIndex);

            _doneEvent.Set();
        }
    }
}

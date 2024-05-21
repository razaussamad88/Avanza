using NATS.Client.JetStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nats_Messaging
{
    public class CustomTimer : System.Timers.Timer
    {
        public string Data;
        public IJetStreamPushSyncSubscription NatsJsSyncSubHandle;
    }
}

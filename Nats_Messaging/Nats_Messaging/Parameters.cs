
namespace Nats_Messaging
{
    public static class NATS_Param
    {
        public enum Stream { ASPIREWEBUI };
        //public enum Subject { FREE_REQUESTS, AWAIT_REQUESTS, AWAIT_RESPONSE };
        public enum Subject { FREE_REQUESTS, REQUEST_REPLY, AWAIT_REQUESTS };
    }
}

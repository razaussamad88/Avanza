using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadPoolConsoleApp
{
    public class DebitCard
    {
        public string CVV { get; set; }
        public string CVV2 { get; set; }
        public string ICVV { get; set; }
        public string Pan { get; set; }
    }

    public class CustomerChannelAuthen
    {
        public string PinCode { get; set; }
        public string PVV { get; set; }
        public string Pan { get; set; }
    }

    public class ThreadContext
    {
        public int ThreadIndex { get; set; }
        public string Imd { get; set; }
        public string Pan { get; set; }
        public DebitCard Card { get; set; }
        public CustomerChannelAuthen CustPin { get; set; }
        public bool IsEMVCard { get; set; }
    }
}

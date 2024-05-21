using System;
using System.Net;

namespace Common.BusinessModels.Common
{
    public class AvanzaResponse
    {
        public String Code { get; set; }
        public String ShortDescription { get; set; }
        public String FullDescription { get; set; }
        public String AlertType { get; set; }
        public HttpStatusCode HttpCode { get; set; }
    }
}
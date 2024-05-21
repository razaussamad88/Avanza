using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Avanza.Common.Logging;
using Avanza.Common.Utility;
using Common.BusinessModels.Common;
using Avanza.Common.Enums;

namespace Avanza.Common.BusinessProcess
{
    public class ShortMessage : IProcessMessage
    {
        public ShortMessage(string loginId)
        {
            LoginId = loginId;
        }

        public override AvanzaResponse AvanzaResponse
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string EventOrigin
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override NameValueCollection Headers
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsSuccess
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override Dictionary<string, string> MsgData
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override Dictionary<string, object> MsgObjArray
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override object MsgObjData
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string PermissionId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override UserActionType UserActionType
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string GetParams(string Key)
        {
            throw new NotImplementedException();
        }

        public override void InitParamsDic(NameValueCollection queryString)
        {
            throw new NotImplementedException();
        }

        public override void InitParamsDic(object obj)
        {
            throw new NotImplementedException();
        }

        public override void PopulateProcessMsgDict(object obj)
        {
            throw new NotImplementedException();
        }

        public override void SetParam(string Key, string Value)
        {
            throw new NotImplementedException();
        }

        public override string LoginId { get; set; }
        public override string MachineName { get { return Environment.MachineName; } }
        public override string EntityId { get { return String.Empty; } }
        public override string Message { get { return String.Empty; } set { throw new NotImplementedException(); } }
    }
}
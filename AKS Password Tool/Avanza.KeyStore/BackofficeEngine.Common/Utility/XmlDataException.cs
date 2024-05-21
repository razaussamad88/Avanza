//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;

//namespace Avanza.Core.Utility
namespace Avanza.Common.Utility
{
    public class XmlDataException : System.Exception
    {
        public XmlDataException(string msgFormat, params object[] args)
            : base(string.Format(msgFormat, args))
        { }

        public XmlDataException(Exception innerExcep, string message)
            : base(message, innerExcep)
        { }
        public XmlDataException(Exception innerExcep, string msgFormat, params object[] args)
            : base(string.Format(msgFormat, args), innerExcep)
        { }

    }
}

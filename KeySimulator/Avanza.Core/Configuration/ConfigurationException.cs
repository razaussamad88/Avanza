//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================
using System;

namespace Avanza.Core.Configuration
{
    public class ConfigurationException : System.Exception
    {
        public ConfigurationException(string message)
            : base(message)
        { }

        public ConfigurationException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        public ConfigurationException(Exception innerExcep, string message)
            : base(message, innerExcep)
        { }

        public ConfigurationException(Exception innerExcep, string format, params object[] args)
            : base(string.Format(format, args), innerExcep)
        { }
    }
}
//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;

namespace Avanza.Core.Logging
{
    /// <summary>
    /// Use LogException for drive classes of LogHandler, 
    /// so that you can able to handle exception occuring from AvanLog seperatly
    /// </summary>
    public class LogException : System.Exception
    {
        public LogException(string msgFormat, params object[] args)
            : base(string.Format(msgFormat, args))
        { }

        public LogException(Exception innerExcep, string message)
            : base(message, innerExcep)
        { }
        public LogException(Exception innerExcep, string msgFormat, params object[] args)
            : base(string.Format(msgFormat, args), innerExcep)
        { }
    }

    // This exception class created to just classify b/w fatal exception and log exception, so that they handle differently
    public class FatalException : LogException 
    {
         public FatalException(string msgFormat, params object[] args)
            : base(msgFormat, args)
        { }

        public FatalException(Exception innerExcep, string message)
            : base(innerExcep, message)
        { }
        public FatalException(Exception innerExcep, string msgFormat, params object[] args)
            : base(innerExcep, msgFormat, args)
        { }
    }

    public class FatalLogHandlerException : FatalException
    {
        private string _loggerName;
        private string _logHandlerName;

        public string LoggerName
        {
            get { return this._loggerName; }
        }

        public string LogHandlerName
        {
            get { return this._logHandlerName; }
        }

        public FatalLogHandlerException(string loggerName,string logHandlerName,string msgFormat, params object[] args)
            : base(msgFormat, args)
        {
            this._loggerName = loggerName;
            this._logHandlerName = logHandlerName;
        }

        public FatalLogHandlerException(string loggerName, string logHandlerName, Exception innerExcep, string message)
            : base(innerExcep, message)
        {
            this._loggerName = loggerName;
            this._logHandlerName = logHandlerName;
        }
       
        public FatalLogHandlerException(string loggerName, string logHandlerName, Exception innerExcep, string msgFormat, params object[] args)
            : base(innerExcep, msgFormat, args)
        {
            this._loggerName = loggerName;
            this._logHandlerName = logHandlerName;
        }
    }

    public class RegisterationException : LogException
    {
        /// <summary>
        /// Exception: "{0} with specified name: {1} already exist in directory"
        /// </summary>
        /// <param name="typeName">Type of object which cannot be registered</param>
        /// <param name="objectName">Name of object which cannot be registered</param>
        public RegisterationException(string typeName, string objectName)
            : base("{0} with specified name: {1} already exist in directory", typeName, objectName)
        { }

        /// <summary>
        /// Exception: "{0} with specified name: {1} already exist in directory"
        /// </summary>
        /// <param name="typeName">Type of object which cannot be registered</param>
        /// <param name="objectName">Name of object which cannot be registered</param>
        public RegisterationException(Exception innerExcep, string typeName, string objectName)
            : base(innerExcep, "{0} with specified name: {1} already exist in directory", typeName, objectName)
        { }

        public RegisterationException(string msg, params object[] args)
            : base(msg, args)
        { }

        public RegisterationException(Exception innerExcep, string message)
            : base(innerExcep, message)
        { }

        public RegisterationException(Exception innerExcep, string msgFormat, params object[] args)
            : base(innerExcep, msgFormat, args)
        { }
    }

    public class ComponentDisposeException : LogException
    {
        public ComponentDisposeException(string msg, params object[] args)
            : base(msg, args)
        { }

        public ComponentDisposeException(Exception innerExcep, string msg)
            : base(innerExcep, msg)
        { }

        public ComponentDisposeException(Exception innerExcep, string msg, params object[] args)
            : base(innerExcep, msg, args)
        { }
    }
}
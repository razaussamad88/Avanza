using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Avanza.Core.Utility;

namespace Avanza.Core.Logging
{
    //[FlagsAttribute]
    public enum LogLevel : short
    {
        Unknown = 0,
        Finest = 1,
        Finer = 2,
        Fine = 4,
        Debug = 8,
        Info = 16,
        Warning = 32,
        Error = 64,
        Severe = 128
    };

    public class Logger : IComparable<Logger>
    {
        #region Private Members

        private string _name;
        private LogLevel _level;
        private List<Logger> _childLoggers;
        private List<LogHandler> _logHandlers;

        #endregion

        protected internal Logger()
        {
            this._childLoggers = new List<Logger>(5);
            this._logHandlers = new List<LogHandler>(5);
        }

        protected internal Logger(string name, LogLevel level)
            : this()
        {
            this._name = name;
            this._level = level;
        }

        internal LogHandler[] LogHandlersArray
        {
            get { return this._logHandlers.ToArray(); }
        }

        /// <summary>
        /// get Name of logger.
        /// </summary>
        public string Name
        {
            get { return this._name; }
        }

        /// <summary>
        /// get LogLevel filter of logger.
        /// </summary>
        public LogLevel LogLevel
        {
            get { return this._level; }
        }

        /// <summary>
        /// Determine if particular LogLevel is loggable to this logger.
        /// </summary>
        /// <param name="level">Loglevel which is to be determine</param>
        /// <returns>Return true if LogLevel is loggable.</returns>
        public bool IsLoggable(LogLevel level)
        {
            return (level >= this._level);
        }

        /// <summary>
        /// Give total number to handlers attached to logger.
        /// </summary>
        public int HandlersCount
        {
            get { return this._logHandlers.Count; }
        }

        /// <summary>
        /// It gives first level of its child.
        /// Note: 
        /// 1: first level child is "parent.child1" not "parent.child1.child1".
        /// 2: both gives same logger "thisName.ChildName" and "ChildName".
        /// </summary>
        /// <param name="name">Name of child. i.e. "com.foo" if parent: "com" then child name: "foo"</param>
        /// <returns>Return child logger if found, else null</returns>
        public Logger this[string name]
        {
            get
            { return GetChildLogger(name); }
        }

        /// <summary>
        /// It gives first level of its child.
        /// Note: 
        /// 1: first level child is "parent.child1" not "parent.child1.child1".
        /// 2: both gives same logger "thisName.ChildName" and "ChildName".
        /// </summary>
        /// <param name="name">Name of child. i.e. "com.foo" if parent: "com" then child name: "foo"</param>
        /// <returns>Return child logger if found, else null</returns>
        public Logger GetChildLogger(string name)
        {
            if (name.StartsWith(this._name))
                return LogManager.GetLogger(name);
            return LogManager.GetLogger(string.Concat(this._name, ".", name));
        }

        #region IComparable<Logger> Members

        /// <summary>
        /// It give culture specific string matching on logger name.
        /// </summary>
        /// <param name="other">logger name how name you want to compare</param>
        /// <returns>return culture specific string comparetion in logger name</returns>
        public int CompareTo(Logger other)
        {
            return this._name.CompareTo(other._name);
        }

        #endregion

        internal void AddChildLogger(Logger childLogger)
        {
            this._childLoggers.Add(childLogger);
        }

        internal void SetLevel(LogLevel level)
        { this._level = level; }

        #region Log Methods

        /// <summary>
        /// Log record/event if it passed logger's log level filter
        /// </summary>
        /// <param name="logEvt">New log information to be logged.</param>
        public void Log(LogRecord logEvt)
        {
            if (IsLoggable(logEvt.Level))
            {
                LogRecord record = logEvt.Clone() as LogRecord;
                record.LogSource = this._name;
                this.Publish(logEvt);
            }
        }

        /// <summary>
        /// Creates a new log information base on data provide and logs it. if it pass log level filter test
        /// </summary>
        /// <param name="level">level of log information. i.e. Finest, Debug, Warning and etc</param>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="param">param will replace {0},{1}..{n} in message string (if provide).</param>
        public void Log(LogLevel level, string msg, params object[] param)
        {
            if (IsLoggable(level))
            {
                string msgcomp = string.Format(msg, param);
                this.Publish(new LogRecord(level, this._name, msgcomp));// Create Log Record and add to distributor queue
            }
        }

        public void Log(LogLevel level, string msg)
        {
            if (IsLoggable(level))
            {
                this.Publish(new LogRecord(level, this._name, msg));// Create Log Record and add to distributor queue
            }
        }

        /// <summary>
        /// logs a new information of Servere level. if it pass logger's log level filter test
        /// </summary>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="param">param will replace {0},{1}..{n} in message string (if provide).</param>
        public void LogSevere(string msg, params object[] param)
        {
            this.Log(LogLevel.Severe, msg, param);
        }

        public void LogSevere(string msg)
        {
            this.Log(LogLevel.Severe, msg);
        }

        /// <summary>
        /// logs a new information of Error level. if it pass logger's log level filter test
        /// </summary>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="param">param will replace {0},{1}..{n} in message string (if provide).</param>
        public void LogError(string msg, params object[] param)
        {
            this.Log(LogLevel.Error, msg, param);
        }

        public void LogError(string msg)
        {
            this.Log(LogLevel.Error, msg);
        }

        /// <summary>
        /// logs a new information of Warning level. if it pass logger's log level filter test
        /// </summary>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="param">param will replace {0},{1}..{n} in message string (if provide).</param>
        public void LogWarning(string msg, params object[] param)
        {
            this.Log(LogLevel.Warning, msg, param);
        }

        public void LogWarning(string msg)
        {
            this.Log(LogLevel.Warning, msg);
        }

        /// <summary>
        /// logs a new information of Debug level. if it pass logger's log level filter test
        /// </summary>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="param">param will replace {0},{1}..{n} in message string (if provide).</param>
        public void LogDebug(string msg, params object[] param)
        {
            this.Log(LogLevel.Debug, msg, param);
        }

        public void LogDebug(string msg)
        {
            this.Log(LogLevel.Debug, msg);
        }

        /// <summary>
        /// logs a new information of Fine level. if it pass logger's log level filter test
        /// </summary>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="param">param will replace {0},{1}..{n} in message string (if provide).</param>
        public void LogFine(string msg, params object[] param)
        {
            this.Log(LogLevel.Fine, msg, param);
        }

        public void LogFine(string msg)
        {
            this.Log(LogLevel.Fine, msg);
        }

        /// <summary>
        /// logs a new information of Finer level. if it pass logger's log level filter test
        /// </summary>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="param">param will replace {0},{1}..{n} in message string (if provide).</param>
        public void LogFiner(string msg, params object[] param)
        {
            this.Log(LogLevel.Finer, msg, param);
        }

        public void LogFiner(string msg)
        {
            this.Log(LogLevel.Finer, msg);
        }

        /// <summary>
        /// logs a new information of Finest level. if it pass logger's log level filter test
        /// </summary>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="param">param will replace {0},{1}..{n} in message string (if provide).</param>
        public void LogFinest(string msg, params object[] param)
        {
            this.Log(LogLevel.Finest, msg, param);
        }

        public void LogFinest(string msg)
        {
            this.Log(LogLevel.Finest, msg);
        }

        public void LogInfo(string msg, params object[] args)
        {
            this.Log(LogLevel.Info, msg, args);
        }

        public void LogInfo(string msg)
        {
            this.Log(LogLevel.Info, msg);
        }
        #region Log SAD
        //This is method is used for logging Secure Authentication Data
        //public void LogSAD(string msg)
        //{
        //    this.LogSAD(LogLevel.Info, msg);        
        //}
        //public void LogSAD(LogLevel level, string msg)
        //{
        //    if (LogManager.IsSADEnabled)
        //    {
        //        if (IsLoggable(level))
        //        {
        //            Cryptographer encrypt = new Cryptographer();
        //            encrypt.AESEncrypt(msg, ref msg, Util.EncryptionKey);
        //            this.Publish(new LogRecord(level, this._name, msg));// Create Log Record and add to distributor queue
        //        }
        //    }
        //}
        #endregion Log SAD

        /// <summary>
        /// logs a new information with class and method info of caller. i.e. "MyClass.MyFunction method log: MyFunction is executed"
        /// if it pass logger's log level filter test
        /// </summary>
        /// <param name="level">level of log information</param>
        /// <param name="sourceClass">Class name which call LogP method</param>
        /// <param name="sourceMethod">Function name which call LogP method</param>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="param">param will replace {0},{1}..{n} in message string (if provide).</param>
        public void LogPrecise(LogLevel level, string sourceClass, string sourceMethod, Exception excep, string msg, params object[] param)
        {
            this.Log(level, string.Format("{0}.{1} method, Log Msg: {2}.{3} Exception: {4}", sourceClass, sourceMethod, string.Format(msg, param), System.Environment.NewLine, Util.BuildExceptionString(excep)));
        }

        /// <summary>
        /// logs a new information with class and method info of caller. i.e. "MyClass.MyFunction method log: MyFunction is executed"
        /// if it pass logger's log level filter test
        /// </summary>
        /// <param name="level">level of log information</param>
        /// <param name="sourceClass">Class name which call LogP method</param>
        /// <param name="sourceMethod">Function name which call LogP method</param>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="excep">Exception due to which current information will be logged</param>        
        public void LogPrecise(LogLevel level, string sourceClass, string sourceMethod, string msg, System.Exception exception)
        {
            this.Log(level, string.Format("{0}.{1} method log: {2}.{3}, Exception: {4}", sourceClass, sourceMethod, msg, System.Environment.NewLine, Util.BuildExceptionString(exception)));
        }

        /// <summary>
        /// logs a new information with class and method info of caller. i.e. "MyClass.MyFunction method log: MyFunction is executed"
        /// if it pass logger's log level filter test
        /// Note: This LogP method will automatically finds Class and Method which called it.
        /// </summary>
        /// <param name="level">level of log information</param>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="param">param will replace {0},{1}..{n} in message string (if provide).</param>
        public void LogPrecise(LogLevel level, Exception excep, string msg, params object[] param)
        {
            MethodBase previousMehtod;
            try
            {
                previousMehtod = (new StackFrame(1)).GetMethod();
            }
            catch (System.Exception ex)
            {
                throw new LogException(ex, "Error traversing caller method in LogP method, Logger: {0}", this._name);
            }
            this.LogPrecise(level, previousMehtod.DeclaringType.FullName, previousMehtod.Name, excep, msg, param);
        }

        /// <summary>
        /// logs a new information with class and method info of caller. i.e. "MyClass.MyFunction method log: MyFunction is executed"
        /// if it pass logger's log level filter test
        /// Note: This LogP method will automatically finds Class and Method which called it.
        /// </summary>
        /// <param name="level">level of log information</param>
        /// <param name="msg">Message is information to log.</param>
        /// <param name="excep">Exception due to which current information will be logged</param>        
        public void LogPrecise(LogLevel level, string msg, System.Exception exception)
        {
            MethodBase previousMehtod;
            try
            {
                previousMehtod = (new StackFrame(1)).GetMethod();
            }
            catch (System.Exception ex)
            {
                throw new LogException(ex, "Error traversing caller method at LogP method, Logger: {0}", this._name);
            }
            this.LogPrecise(level, previousMehtod.DeclaringType.FullName, previousMehtod.Name, msg, exception);
        }

        #endregion

        #region Handler Methods

        /// <summary>
        /// It gives all handlers attach to logger.
        /// Note: This function is thread safe.
        /// </summary>
        /// <returns>return IEnumeration of log handlers attached. if handler count is 0 then null</returns>
        public IEnumerable<LogHandler> GetHandlers()
        {
            lock (this._logHandlers)
            {
                foreach (LogHandler listener in this._logHandlers)
                    yield return listener;
            }
        } // should be thread safe to add/remove handler methods

        internal void AddHandler(LogHandler handler)
        {
            lock (this._logHandlers)
            {
                if (!this._logHandlers.Contains(handler))
                    this._logHandlers.Add(handler);
            }
        }// use by Logmanager to attach handler

        internal void RemoveHandler(LogHandler handler)
        {
            lock (this._logHandlers)
            { this._logHandlers.Remove(handler); }
        }// use by LogManager to remove handler

        internal void RemoveHandler(string handlerName)
        {
            foreach (LogHandler handler in this.LogHandlersArray) // using threadsafe Handlers enumeration
                if (!string.IsNullOrEmpty(handler.Name) && handler.Name.Equals(handlerName))
                {
                    RemoveHandler(handler);
                    break;
                }
        }// use by LogManager to remove handler      

        #endregion

        internal Logger[] GetChildLoggers()
        {
            return this._childLoggers.ToArray();
        }

        /// <summary>
        /// Simply writes record to LogManager
        /// </summary>
        /// <param name="logRec"></param>
        private void Publish(LogRecord logRec)
        {
            LogManager.Publish(logRec);
        }
    }
}

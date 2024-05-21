using System;
using System.Collections.Generic;
using System.Text;
using Avanza.Core.Configuration;
using Avanza.Core.Module;
using System.Configuration;
//using Avanza.Core.Data;
//using Avanza.Core.Utility;

namespace Avanza.Core.Logging
{
    public delegate void ExcepEventHandler<TExcep>(object sender, ExcepEventArgs<TExcep> ExcepArgs) where TExcep : Exception;
    public enum EComponentType
    {
        VisionServer,
        VisionClient,
        AvanzaFramework
    }

    public class ExcepEventArgs<TExcep> : EventArgs
        where TExcep : Exception
    {
        private TExcep _exception;
        public ExcepEventArgs(TExcep excep)
        {
            this._exception = excep;
        }
        public TExcep Exception
        {
            get { return this._exception; }
        }
    }

    public static class LogManager
    {
        public const string XmlAvanLog = "avan-log";
        public static EComponentType ComponentType;

        private static Dictionary<string, Logger> _loggerList;
        private static Dictionary<string, LogHandler> _logHandlers;
        private static Dictionary<string, int> _logHandlersRefCount;
        private static LogDistributor _logDistributor;
        private static Logger _defaultLogger;
        private static ModuleFactory<LogHandler> _moduleFactory;

        private static event ExcepEventHandler<Exception> _logExepEvent;
        //private static SystemConfigurationVision sysConfig = new SystemConfigurationVision();
        private static object sysConfig = null;

        static LogManager()
        {
            LogManager._loggerList = new Dictionary<string, Logger>();
            LogManager._logHandlers = new Dictionary<string, LogHandler>();
            LogManager._logHandlersRefCount = new Dictionary<string, int>();
            LogManager._logDistributor = new LogDistributor();
            LogManager._moduleFactory = new ModuleFactory<LogHandler>();
        }

        #region Properties

        public static int LoggersCount
        {
            get { return LogManager._loggerList.Count; }
        }

        public static int HandlersCount
        {
            get { return LogManager._logHandlers.Count; }
        }

        /// <summary>
        /// This event raise when handler throws exception writing
        /// to there target.
        /// </summary>
        public static event ExcepEventHandler<Exception> LogExcepEvent
        {
            add { LogManager._logExepEvent += value; }
            remove { LogManager._logExepEvent -= value; }
        }

        #endregion

        #region  Logger Methods

        public static Logger GetLogger(string name)
        {
            if (LogManager.LoggersCount == 0)
                return LogManager._defaultLogger;

            name = name.ToLower();
            int index = -1;
            while (!LogManager._loggerList.ContainsKey(name))
            {
                if ((index = name.LastIndexOf('.')) != -1)
                    name = name.Substring(0, index);
                else
                {
                    name = string.Empty;
                    break;
                }
            }

            if (name.Length != 0)
                return LogManager._loggerList[name];

            return LogManager._defaultLogger;
        } // return logger. 

        public static void AddLogger(Logger logger)
        {
            string loggerName = logger.Name.ToLower();

            if (LogManager._loggerList.ContainsKey(loggerName))
                throw new LogException("Cannot add specified logger \"{0}\" into loggers collection, because logger with this name already exists.",
                    logger.Name);
            LogManager._loggerList.Add(loggerName, logger);

        } // new logger instance

        public static void RemoveLogger(string loggerName, bool isDisposeHandler)
        {
            loggerName = loggerName.ToLower();
            if (LogManager._loggerList.ContainsKey(loggerName))
            {
                Logger logger = LogManager._loggerList[loggerName];
                LogHandler[] handlerArray = logger.LogHandlersArray;

                foreach (LogHandler handler in handlerArray)
                {
                    logger.RemoveHandler(handler);

                    if (LogManager._logHandlers.ContainsKey(handler.Name)
                        && --LogManager._logHandlersRefCount[handler.Name] == 0
                        && isDisposeHandler)
                    {
                        LogManager._logHandlers.Remove(handler.Name);
                        LogManager._logHandlersRefCount.Remove(handler.Name);
                    }
                }
                LogManager._loggerList.Remove(loggerName);
            }
        }// remove existing logger and handlers(if true) which attach to this logger only

        public static void RemoveLogger(Logger logger, bool isDisposeHandler)
        {
            RemoveLogger(logger.Name, isDisposeHandler);
        } // remove existing logger and handlers(if true) which attach to this logger only

        #endregion

        #region Log Handler Methods

        /// <summary>
        /// Register new handler to its directory.
        /// </summary>
        /// <remarks>You can use this function to add new instance of your custom LogHandler in code. 
        /// Added handler can be reference with its unique name</remarks>
        /// <exception cref="Avanza.Util.Logging.RegisterationException">throw RegisterationException if handler with specified 
        /// name already found.</exception>
        /// <param name="handler">Drive class of LogHandler like FileLogHandler.</param>
        public static void RegisterHandler(LogHandler handler)
        {
            string handlerName = handler.Name.ToLower();
            if (LogManager._logHandlers.ContainsKey(handlerName))
                throw new RegisterationException("handler with specifed name: {0} is already registered with LogManager", handler.Name);
            lock (LogManager._logHandlers)
            {
                LogManager._logHandlers.Add(handlerName, handler);
                LogManager._logHandlersRefCount.Add(handlerName, 0);
            }
        }

        /// <summary>
        /// Return all registered handlers found in LogManager registered handlers directory.
        /// </summary>
        /// <returns>Array of custom LogHandlers</returns>
        public static LogHandler[] GetRegisteredHandlers()
        {
            LogHandler[] regisHandlers = new LogHandler[LogManager._logHandlers.Count];
            int count = 0;
            foreach (LogHandler handler in LogManager._logHandlers.Values)
            {
                if (count == regisHandlers.Length)
                    break;
                regisHandlers[count++] = handler;
            }

            return regisHandlers;
        }

        /// <summary>
        /// Return all handlers attached to the logger.
        /// </summary>
        /// <param name="loggername">Name of logger who's handlers are required</param>
        /// <returns>Array of custom LogHandlers or null if logger does not exists</returns>
        public static LogHandler[] GetHandlers(string loggerName)
        {
            loggerName = loggerName.ToLower();
            if (LogManager._loggerList.ContainsKey(loggerName))
                return LogManager._loggerList[loggerName].LogHandlersArray;

            return null;
        }// returns all handlers attach to that logger.

        /// <summary>
        /// Attach handler to specifed logger in LogManager's logger list.
        /// Note: It is also registered handler if not already registered.
        /// </summary>
        /// <param name="loggerName">Name of logger you want attach handler with</param>
        /// <param name="handler">New Or existing instance of handler</param>
        /// <exception cref="Avanza.Util.logging.RegisterationException">Underlining function throw exception,
        /// if logger is not found in LogManager.</exception>
        public static void AddHandler(string loggerName, LogHandler handler)
        {
            if (!LogManager._logHandlers.ContainsKey(loggerName.ToLower()))
                LogManager.RegisterHandler(handler);
            LogManager.AddHandler(loggerName, handler.Name);
        } // Add new/existing instance of handler to particular logger

        /// <summary>
        /// Attach specified handler with specified logger.
        /// </summary>
        /// <param name="loggerName">Name of logger the handler will be attach</param>
        /// <param name="handlerName">Name of handler which attach to logger</param>
        /// <exception cref="Avanza.Util.Logging.RegisterationException">throw RegisterationException if logger Or handler 
        /// with specified name not found</exception>
        public static void AddHandler(string loggerName, string handlerName)
        {
            loggerName = loggerName.ToLower();
            handlerName = handlerName.ToLower();

            if (!LogManager._loggerList.ContainsKey(loggerName))
                throw new RegisterationException(string.Format("LogManager cannot add loghandler {0} to logger {1}, because {1} is not registered with LogManager", handlerName, loggerName));

            if (!LogManager._logHandlers.ContainsKey(handlerName))
                throw new RegisterationException(string.Format("LogManager cannot add loghandler {0} to logger {1}, because {0} is not registered with LogManager", handlerName, loggerName));

            Logger logger = LogManager._loggerList[loggerName];
            logger.AddHandler(LogManager._logHandlers[handlerName]);

            LogManager._logHandlersRefCount[handlerName]++;

        } // Add existing instance of handler to particular logger

        /// <summary>
        /// Remove Handler from specified logger
        /// </summary>
        /// <param name="loggerName">Name of logger who's handler will be deattach</param>
        /// <param name="handlerName">Name of handler which will be deattach </param>
        /// <exception cref="Avanza.Util.Logging.RegistrationException">throw RegisterationException if logger Or handler 
        /// with specified name not found</exception>
        public static void RemoveHandler(string loggerName, string handlerName)
        {
            loggerName = loggerName.ToLower();
            handlerName = handlerName.ToLower();

            if (!LogManager._loggerList.ContainsKey(loggerName))
                throw new System.InvalidOperationException(string.Format("LogManager cannot remove loghandler {0} to logger {1}, because {1} is not registered with LogManager", handlerName, loggerName));

            if (!LogManager._logHandlers.ContainsKey(handlerName))
                throw new System.InvalidOperationException(string.Format("LogManager cannot remove loghandler {0} to logger {1}, because {0} is not registered with LogManager", handlerName, loggerName));

            Logger logger = LogManager._loggerList[loggerName];
            logger.RemoveHandler(handlerName);

            LogManager._logHandlersRefCount[handlerName]--;//decrease attachment count of loghandler with logger

        } // Remove Handler from particular logger

        /// <summary>
        /// Remove specified handler from all loggers(Registered)
        /// If isDispose is true then dispose handler too.
        /// </summary>
        /// <param name="handlerName">Name of handler which will be deattach</param>
        /// <param name="isDisposeHandler">Set isDisposeHandler to true if you handler to be distoryed after degisteration.</param>
        public static void RemoveHandler(string handlerName, bool isDisposeHandler)
        {
            LogHandler toDisposeHandler = null;
            if (LogManager._logHandlers.TryGetValue(handlerName, out toDisposeHandler))
            {
                foreach (Logger logger in LogManager._loggerList.Values)
                    logger.RemoveHandler(toDisposeHandler);

                if (isDisposeHandler)
                {
                    LogManager.DeregisterHandler(handlerName);
                    try
                    {
                        toDisposeHandler.Dispose();
                    }
                    catch (System.ObjectDisposedException disposeExcep)
                    {
                        throw new ComponentDisposeException(disposeExcep, "Unable to dispose handler with name: {0} right now, some of logger threads may be still using it.", handlerName);
                    }
                }
                else
                    LogManager._logHandlersRefCount[handlerName] = 0;
            }
            else
                throw new RegisterationException("LogHandler with name: {0} is not registered with LogManager cannot be removed", handlerName);

        }// Remove Handler from all loggers

        /// <summary>
        /// Deregister LogHandler with LogManager.
        /// Note: It does not deattach handler from its loggers.
        /// </summary>
        /// <param name="handlerName">Handler name which will be deregister.</param>
        /// <remarks>Once handler deregistered, cannot be available for addition or removel with handler.</remarks>
        public static void DeregisterHandler(string handlerName)
        {
            handlerName = handlerName.ToLower();
            lock (LogManager._logHandlers)
            {
                LogManager._logHandlers.Remove(handlerName);
                LogManager._logHandlersRefCount.Remove(handlerName);
            }
        }

        #endregion
        /*
        /// <summary>
        /// Iniitialize LogManager with loggers and handler avaiable, it also set hierachcal structure
        /// of parent child relation amongs loggers.
        /// </summary>
        /// <remarks>It should be called before using LogManager.</remarks>
        public static void Initialize()
        {
            XmlConfigReader configReader = System.Configuration.ConfigurationManager.GetSection("avan-log") as XmlConfigReader;

            if (configReader != null)
                LogManager.Initialize(configReader.RootSection);
        }
*/
        /// <summary>
        /// Iniitialize LogManager with loggers and handler avaiable, it also set hierachcal structure
        /// of parent child relation amongs loggers.
        /// PA-DSS: Component Type = Vision.Client ; Vision.Server ; Avanza.Framework
        /// </summary>
        /// <remarks>It should be called before using LogManager.</remarks>
        public static void Initialize(EComponentType _componentType)
        {
            XmlConfigReader configReader = System.Configuration.ConfigurationManager.GetSection("avan-log") as XmlConfigReader;

            if (configReader != null)
                LogManager.Initialize(configReader.RootSection, _componentType);
        }

        /*        public static void Initialize(IConfigSection rootSection)
                {
                    //Loading all handlers in define in app.config
                    LogManager.LoadHandlers(rootSection.GetChild("handler-list"));

                    //Loading all loggers in define in app.config
                    LogManager.LoadLoggers(rootSection.GetChild("logger-list"));

                    //Finding and registering child logger to there parents and asign hieracle log levels
                    LogManager.LinkChildLoggers();

                    //Initializing Distributor to Start processing
                    LogManager._logDistributor.Initialize(false);
                }*/

        /// <summary>
        /// Iniitialize LogManager with loggers and handler avaiable, it also set hierachcal structure
        /// of parent child relation amongs loggers.
        /// rootSection is configuration section containing log information
        /// Component Type (PA-DSS) = Vision.Client ; Vision.Server ; Avanza.Framework
        /// </summary>
        /// <remarks>It should be called before using LogManager.</remarks>
        public static void Initialize(IConfigSection rootSection, EComponentType _componentType)
        {
            // PA-DSS: Setting ComponentName to identify the component from logger is called
            ComponentType = _componentType;
            //Loading all handlers in define in app.config
            LogManager.LoadHandlers(rootSection.GetChild("handler-list"));

            //Loading all loggers in define in app.config
            LogManager.LoadLoggers(rootSection.GetChild("logger-list"));

            //Finding and registering child logger to there parents and asign hieracle log levels
            LogManager.LinkChildLoggers();

            //Initializing Distributor to Start processing
            LogManager._logDistributor.Initialize(false);
        }

        /// <summary>
        /// Iniitialize SYSTEM_CONFIGURATION table to be used for loading configuration parameters e.g. ENABLE_SAD (PA-DSS)
        /// </summary>
        /// <remarks>It must be called after database is initialized.</remarks>
        /*
        public static void InitSysConfiguration()
        {
            if (sysConfig != null)
                sysConfig.LoadSysConfigTable();

            if (ComponentType == EComponentType.AvanzaFramework)
            {
                XmlConfigReader configReader = System.Configuration.ConfigurationManager.GetSection("avan-log") as XmlConfigReader;

                if (configReader != null)
                {
                    IConfigSection handlersConfigList = configReader.RootSection.GetChild("handler-list");

                    foreach (IConfigSection handlerConfig in handlersConfigList.GetChildSections("log-handler"))
                    {
                        string handlerName = handlerConfig.GetTextValue("name");
                        handlerName = handlerName.ToLower();
                        LogHandler handler = _logHandlers[handlerName];
                        if (handler != null)
                        {
                            handler.Update(handlerConfig);
                        }
                    }
                }
            }
        }
        */

        #region SAD

        public static bool IsSADEnabled
        {
            get
            {
                if (LogManager.ComponentType == EComponentType.VisionClient)
                {
                    return false;
                }
                else
                {
                    if (sysConfig != null)
                    {
                        object sadOn = null;
                        try
                        {
                            //sadOn = sysConfig.GetValue("ENABLE_SAD");
                        }
                        catch { }

                        if (sadOn != null)
                            return Convert.ToBoolean(Convert.ToInt16(sadOn.ToString()));
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
        }

        public static LogHandler GetHandler
        {
            get
            {
                LogHandler fileLogHandler = null;
                foreach (LogHandler logH in _logHandlers.Values)
                {
                    if (logH is FileLogHandler)
                        fileLogHandler = logH;
                }
                return fileLogHandler;
            }
        }


        #endregion SAD
        public static void Close()
        {
            LogManager._logDistributor.Close();
        }

        internal static void Publish(LogRecord record)
        {
            LogManager._logDistributor.AddToQueue(record);
        }// use by loggers, therefore it is internal. This function will publish record to Distributer;

        internal static void RaiseExcepEvent(object sender, Exception excep)
        {
            if (LogManager._logExepEvent != null)
                LogManager._logExepEvent(sender, new ExcepEventArgs<Exception>(excep));
        }

        private static void LoadHandlers(IConfigSection handlersConfigList)
        {
            try
            {
                if (handlersConfigList != null)
                {
                    LogManager._moduleFactory.Load(handlersConfigList, "log-handler");
                    foreach (IConfigSection handlerConfig in handlersConfigList.GetChildSections("log-handler"))
                    {
                        string handlerName = handlerConfig.GetTextValue("name");
                        handlerName = handlerName.ToLower();
                        List<AsmInfo> lstAsmInfo = LogManager._moduleFactory.GetAssemblyInfoList(handlerName);
                        if (lstAsmInfo.Count == 1)
                        {
                            LogHandler handler = lstAsmInfo[0].GetModule<LogHandler>();
                            handler.Initialize(handlerConfig);
                            LogManager.RegisterHandler(handler);
                        }
                        else
                            throw new FatalException("Loading handler from configuration file failed. Assembly information not found");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FatalException(ex, "Loading handler from configuration file failed.");
            }
        }

        private static void LoadLoggers(IConfigSection loggerConfigList)
        {
            try
            {
                Type typeOfLogLevel = typeof(LogLevel);
                foreach (IConfigSection loggerConfig in loggerConfigList.GetChildSections("./logger"))
                {
                    string loggerName = loggerConfig.GetTextValue("name");
                    loggerName = loggerName.ToLower();
                    LogLevel level = LogLevel.Unknown;

                    if (loggerConfig.HasAttribute("level"))
                    {
                        string strlevel = loggerConfig.GetTextValue("level");
                        if (System.Enum.IsDefined(typeOfLogLevel, strlevel))
                            level = (LogLevel)System.Enum.Parse(typeOfLogLevel, strlevel, true);
                    }

                    Logger newLogger = new Logger(loggerName, level);
                    if (loggerConfig.GetValue("default", false))
                        if (LogManager._defaultLogger == null)
                            LogManager._defaultLogger = newLogger;
                        else
                            throw new ConfigurationErrorsException(string.Format("You can not define multiple default loggers \"{0}\"", newLogger.Name));

                    LogManager.AddLogger(newLogger);

                    IConfigSection[] handlersRefList = loggerConfig.GetChildSections("handler-list/handler");
                    if (handlersRefList != null)
                        foreach (IConfigSection handlerRefConfigSection in handlersRefList)
                        {
                            string handlerRefName = handlerRefConfigSection.GetTextValue("ref");
                            LogManager.AddHandler(loggerName, handlerRefName);
                        }
                }

                if (LogManager.LoggersCount != 0 && LogManager._defaultLogger == null)
                    throw new ConfigurationErrorsException("You must defince atleast one default logger");

                if (LogManager._defaultLogger == null)
                {
                    Logger emptyLogger = new Logger("EmptyLogger", LogLevel.Unknown);
                    LogManager._defaultLogger = emptyLogger;

                    //LogManager.AddLogger(defaultLogger);
                    //LogManager.AddHandler(defaultLogger.Name,new ConsoleLogHandler("defaultHandler",LogLevel.Unknown)); 
                }
            }
            catch (Exception ex)
            {
                throw new FatalException(ex, "Loading loggers from configuration file failed.");
            }
        }

        private static void LinkChildLoggers()
        {
            try
            {
                Logger[] sortedLogger = new Logger[LogManager._loggerList.Count];
                LogManager._loggerList.Values.CopyTo(sortedLogger, 0);
                Array.Sort<Logger>(sortedLogger);

                foreach (Logger logger in sortedLogger)
                {
                    // Assiging level of imediate parent if level is not set
                    if (logger.LogLevel == LogLevel.Unknown)
                        logger.SetLevel(GetImediateParentLevel(logger.Name));

                    Logger[] childloggers = LogManager.GetChildLoggers(logger.Name);
                    if (childloggers != null && childloggers.Length != 0)
                        foreach (Logger childlogger in childloggers)
                            logger.AddChildLogger(childlogger);
                }
            }
            catch (Exception ex)
            {
                throw new FatalException(ex, "Link child loggers to its parent failed.");
            }
        }

        private static LogLevel GetImediateParentLevel(string childLoggerName)
        {
            int index = -1;
            string parentLoggerName = childLoggerName;
            Logger parentLogger = null;

            while ((index = parentLoggerName.LastIndexOf('.')) != -1)
            {
                parentLoggerName = parentLoggerName.Substring(0, index);
                if (LogManager._loggerList.TryGetValue(parentLoggerName, out parentLogger))
                    return parentLogger.LogLevel;
            }
            return LogLevel.Unknown;
        }

        private static Logger[] GetChildLoggers(string parentName)
        {
            if (string.IsNullOrEmpty(parentName)) return null;

            if (!parentName.EndsWith(".")) parentName += ".";

            List<Logger> childLoggers = new List<Logger>();
            foreach (Logger childLogger in LogManager._loggerList.Values)
            {
                if (childLogger.Name.StartsWith(parentName))
                {
                    string childName = childLogger.Name.Replace(parentName, "");
                    if (childName.IndexOf('.') == -1)
                        childLoggers.Add(childLogger);
                }
            }
            return childLoggers.ToArray();
        }
    }
}

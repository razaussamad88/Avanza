using Avanza.Common.BusinessProcess;
using NLog;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Avanza.Common.Logging
{
    public enum MethodState { Begin, End, None };

    public class ActivityLogger
    {
        private static ActivityLogger mInstance = null;
        public Logger logger;

        public static ActivityLogger Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new ActivityLogger();
                }

                return mInstance;
            }
        }

        public ActivityLogger()
        {
            string logLevel = string.Empty;
            IList<NLog.Config.LoggingRule> lstLoggingRules = LogManager.Configuration.LoggingRules;

            if (lstLoggingRules != null)
            {
                NLog.Config.LoggingRule loggingRule = lstLoggingRules[0];

                CurrentLogLevel = loggingRule.Levels[0];
            }

            logger = LogManager.GetLogger(m_LoggerName);
        }

        public LogLevel CurrentLogLevel { get; set; }

        public void ActivityLog(LogLevel level, string message, string action, string primaryKeyVals, string changedColumn, string userId, string machineName, string eventOrigin, string description, ActionType actionType, int result, Exception ex = null)
        {
            ActivityLogEvent logEvent = new ActivityLogEvent();

            logger.Log(logEvent.GetLogEvent(level, m_LoggerName, message, "", action, primaryKeyVals, changedColumn, "", DateTime.Now, userId, DateTime.Now, machineName, result, eventOrigin, description, actionType, ex));
        }

        private static string m_LoggerName = "SymmetryWeb";

        public static void Init(string loggerName)
        {
            m_LoggerName = loggerName;
        }

        public void SystemLog(LogLevel level, string message, string action, string entityId, string userId, string machineName, string eventOrigin, string description, int result, Exception ex = null)
        {
            ActivityLogEvent logEvent = new ActivityLogEvent();

            logger.Log(logEvent.GetLogEvent(level, m_LoggerName, message, entityId, action, "", "", userId, DateTime.Now, userId, DateTime.Now, machineName, result, eventOrigin, description, ActionType.Undefined, ex));
        }

        public void SystemLog(LogLevel level, string message, string action, string eventOrigin, string description, int result, Exception ex = null)
        {
            ActivityLogEvent logEvent = new ActivityLogEvent();

            logger.Log(logEvent.GetLogEvent(level, m_LoggerName, message, "", action, "", "", "", DateTime.Now, "", DateTime.Now, "", result, eventOrigin, description, ActionType.Undefined, ex));
        }

        public void SystemLog(LogLevel level, string methodName, string eventOrigin, string description, Exception ex = null)
        {
            ActivityLogEvent logEvent = new ActivityLogEvent();

            logger.Log(logEvent.GetLogEvent(
                level, m_LoggerName, String.Format("Executing Method {0} ", methodName), String.Empty, ActionType.CoreFunction.ToString(),
                String.Empty, String.Empty, String.Empty, DateTime.Now, String.Empty, DateTime.Now, String.Empty, 1, eventOrigin,
                description, ActionType.Undefined, ex));
        }

        public void SystemLog(LogLevel level, string description)
        {
            ActivityLogEvent logEvent = new ActivityLogEvent();

            logger.Log(logEvent.GetLogEvent(
                level, m_LoggerName, String.Empty, String.Empty, String.Empty,
                String.Empty, String.Empty, String.Empty, DateTime.Now, String.Empty, DateTime.Now, String.Empty, 1, String.Empty,
                description, ActionType.Undefined, null));
        }

        public void SystemLog(LogLevel level, MethodBase method, string eventOrigin, string description, Exception ex = null)
        {
            ActivityLogEvent logEvent = new ActivityLogEvent();

            logger.Log(logEvent.GetLogEvent(
                level, m_LoggerName, String.Format("Executing Method {0} ", method.Name), String.Empty, ActionType.CoreFunction.ToString(),
                String.Empty, String.Empty, String.Empty, DateTime.Now, String.Empty, DateTime.Now, String.Empty, 1, eventOrigin,
                description, ActionType.Undefined, ex));
        }

        public void Log(IProcessMessage tranMessage, MethodBase method, Type typeObj, MethodState state = MethodState.None)
        {
            Log(tranMessage, method, typeObj, String.Empty, ActionType.View, state);
        }

        public void Log(string loginId, MethodBase method, MethodState state = MethodState.None)
        {
            Log(new ShortMessage(loginId), method, null, String.Empty, ActionType.View, state);
        }

        public void Log(string loginId, MethodBase method, Exception ex)
        {
            Log(new ShortMessage(loginId), method, null, ex);
        }

        public void Log(IProcessMessage tranMessage, MethodBase method, Type typeObj, string message, ActionType actionType = ActionType.View, MethodState state = MethodState.None)
        {
            string strState = String.Empty;

            switch (state)
            {
                case MethodState.Begin:
                    strState = String.Format("{0} ", MethodState.Begin.ToString());
                    break;

                case MethodState.End:
                    strState = String.Format("{0} ", MethodState.End.ToString());
                    break;
            }

            mInstance.SystemLog(
                        LogLevel.Info,
                        String.Format("{0}Executing Method {1}.", strState, method.Name),
                        actionType.ToString(),
                        tranMessage.EntityId,
                        tranMessage.LoginId,
                        tranMessage.MachineName,
                        (typeObj == null ? method.Name : typeObj.Name),
                        String.IsNullOrEmpty(message) ? tranMessage.Message : message,
                        1);
        }

        public void Log(IProcessMessage tranMessage, MethodBase method, Type typeObj, Exception ex)
        {
            mInstance.SystemLog(
                        LogLevel.Error,
                        String.Format("Exception occurred in {0}.", method.Name),
                        ActionType.View.ToString(),
                        tranMessage.EntityId,
                        tranMessage.LoginId,
                        tranMessage.MachineName,
                        (typeObj == null ? method.Name : typeObj.Name),
                        String.Format("Failed. Exception: {0} | {1} | {2} ", ex.Message, ex.InnerException, ex.StackTrace),
                        0, ex);
        }
    }
}
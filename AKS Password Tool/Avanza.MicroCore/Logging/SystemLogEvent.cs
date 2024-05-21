using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Avanza.Common.Logging
{
    /*
    class SystemLogEvent : IVisionLogEvent
    {
        public LogEventInfo GetLogEvent(LogLevel level, string loggerName, string message, string Entity, string Action, string PrimaryKeyVals, string ChangedColVals, string CreatedBy, DateTime CreatedOn, string UpdateBy, DateTime UpdatedOn, string MachineName, int Result, string EventOrigin, string Description, ActionType actionType)
        {
            var logEvent = new LogEventInfo(level, loggerName, message);

            logEvent.Properties["ENTITY"] = Entity;
            logEvent.Properties["ACTION"] = Action;
            logEvent.Properties["PRIMARY_KEY_VALS"] = PrimaryKeyVals;
            logEvent.Properties["CHANGED_COLS_VAL"] = ChangedColVals;
            logEvent.Properties["CREATED_ON"] = CreatedOn;
            logEvent.Properties["CREATED_BY"] = CreatedBy;
            logEvent.Properties["UPDATED_ON"] = UpdatedOn;
            logEvent.Properties["UPDATED_BY"] = UpdateBy;
            logEvent.Properties["WINDOWS_CREATED_BY"] = "";
            logEvent.Properties["WINDOWS_UPDATED_BY"] = "";
            logEvent.Properties["MACHINE_NAME"] = MachineName;
            logEvent.Properties["LOG_TYPE_ID"] = ((int)LogType.System).ToString();
            logEvent.Properties["RESULT"] = Result;
            logEvent.Properties["EVENT_ORIGIN"] = EventOrigin;
            logEvent.Properties["DESCRIPTION"] = Description;
            logEvent.Properties["ACTION_TYPE"] = actionType;
            logEvent.Properties["LEVEL"] = level;
            logEvent.Properties["LOGGER_NAME"] =loggerName;
            logEvent.Properties["MESSAGE"] = message;

            return logEvent;
        }
    }*/
}

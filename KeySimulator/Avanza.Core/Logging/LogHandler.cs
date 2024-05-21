using System;
using System.Collections.Generic;
using System.Text;
using Avanza.Core.Configuration;

namespace Avanza.Core.Logging
{
    public abstract class LogHandler : IDisposable
    {
        private LogLevel _level = LogLevel.Warning;
        protected string _name;

        public LogHandler()
        { }

        protected LogHandler(string name, LogLevel level)
        {
            if (string.IsNullOrEmpty(name))
                throw new LogException("Log handler name cannot be set to null. handler type: {0}", this.GetType().Name);
            this._name= name;
            this._level= level;
        }

        public abstract void Publish(LogRecord record);
        public abstract void Close();
        public abstract void Flush();
        public abstract void Initialize(Avanza.Core.Configuration.IConfigSection config);
        public abstract void Update(Avanza.Core.Configuration.IConfigSection config);

        public LogLevel Level
        {
            get { return this._level; }
            set { this._level = value; }
        }
        
        public string Name
        {
            get { return this._name; }
        }

        public bool IsLoggable(LogLevel level)
        {
            return (level >= this._level);
        }


        #region IDisposable Members

        public abstract void Dispose() ;

        #endregion
    }   
}
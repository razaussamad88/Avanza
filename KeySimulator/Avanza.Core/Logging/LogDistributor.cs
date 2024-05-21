using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Avanza.Core.Logging
{
    internal class LogDistributor
    {
        private const int WAIT_TIME_OUT = 30000;

        private bool _isPersistable;
        private bool _isActive;
        private Thread _worker;
        private ManualResetEvent _queueSignal;
        private LogLevel _currentRecordLevel;
        private Queue<LogRecord> _logQueue;
        private readonly List<string> _processedHandlers;
        private readonly List<Logger> _loggableLoggers;
        

        public LogDistributor()
        {
            this._isPersistable = false;
            this._isActive = false;
            this._processedHandlers = new List<string>(10);
            this._loggableLoggers = new List<Logger>(10);
        }

        public bool IsPersistable
        {
            get { return this._isPersistable; }
            set { this._isPersistable = value; }
        }

        public void Initialize(bool isPersist)
        {
            lock (this)
            {
                if (!this._isActive)
                {
                    this._logQueue = new Queue<LogRecord>(50);
                    this._queueSignal = new ManualResetEvent(false);
                    this._worker = new Thread(new ThreadStart(this.publishLog));
                    this._isActive = true;
                    this._worker.Start();
                    
                }
            }
        }

        public void Close()
        {
            lock (this)
            {
                if (this._isActive)
                {
                    this._isActive = false;

                    this._queueSignal.Set();
                    if (!this._worker.Join(LogDistributor.WAIT_TIME_OUT))
                        this._worker.Abort();
                }
            }
        }
        
        public void AddToQueue(LogRecord record)
        {
            if (this._isActive)
            {
                lock (this._logQueue)
                {
                    this._logQueue.Enqueue(record);

                    if (this._logQueue.Count == 1)
                        this._queueSignal.Set();
                }
            }
        }
     
        private void publishLog()
        {
            while(this._isActive)
            {
                this._queueSignal.WaitOne();
                this.ProcessQueue();
            }

            this.ProcessQueue();
        }

        private void ProcessQueue()
        {
            LogRecord logRec = null;

            try
            {
                while (this._logQueue.Count > 0)
                {
                    lock (this._logQueue)
                    {
                        logRec = this._logQueue.Dequeue();
                    }

                    Logger temp = LogManager.GetLogger(logRec.Source);
                    
                    this.FindLoggableLoggers(temp, logRec.Level);
                    this.WriteToUniqueHandlers(logRec);
                    
                }
                lock (this._logQueue)
                {
                    if(this._logQueue.Count == 0)
                        this._queueSignal.Reset();
                }
            }
            catch (ThreadAbortException)
            { }
            catch (Exception ex)
            {
                throw new FatalException(ex, string.Format("Exception caught while processing Queue. Count: {0}, Log Message: {1}",
                                                            this._logQueue.Count, ((logRec != null) ? logRec.ToString() : string.Empty)));
            }
        }

        private void FindLoggableLoggers(Logger logger,LogLevel level)
        {
            this._loggableLoggers.Clear();
            this._currentRecordLevel = level;
            this._loggableLoggers.Add(logger);
            
            // UnComment this function to send log record to childs
            //this.RecursiveChildFinder(logger);
        }

        private void RecursiveChildFinder(Logger logger)
        {
            Logger[] childloggers = logger.GetChildLoggers();
            foreach (Logger childlogger in childloggers)
            {
                if (childlogger.IsLoggable(this._currentRecordLevel))
                {
                    this._loggableLoggers.Add(childlogger);
                    RecursiveChildFinder(childlogger);
                }
            }
        }

        private void WriteToUniqueHandlers(LogRecord logRec)
        {
            this._processedHandlers.Clear();
            foreach (Logger logger in this._loggableLoggers)
                foreach (LogHandler handler in logger.GetHandlers())
                    this.WriteRecord(handler, logRec);
        }

        private void WriteRecord(LogHandler handler, LogRecord logRec)
        {
            if (!this._processedHandlers.Contains(handler.Name))
            {
                this._processedHandlers.Add(handler.Name);
                try
                { handler.Publish(logRec); }
                catch (Exception writeExcep)
                { LogManager.RaiseExcepEvent(handler, writeExcep); }
            }
        }
    }
}

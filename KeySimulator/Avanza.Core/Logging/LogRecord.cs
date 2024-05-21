//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml;
using Avanza.Core.Utility;

namespace Avanza.Core.Logging
{
    
    [Serializable]
    [XmlRoot(ElementName = "log-rec")]
    public class LogRecord : ICloneable , IXmlSerializable
    {
        public const int UnknownEvent = 0;

        private const string XmlNode = "log-rec";
        private const string XmlSource = "source";
        private const string XmlLevel = "level";
        private const string XmlEvent = "event";
        private const string XmlThread = "thread";
        private const string XmlMsg = "msg";
        private const string XmlPropsBag = "props-bag";
        private const string XmlItem = "item";
        private const string XmlKey = "key";
        private const string XmlValue = "value";
        private const string XmlMachName = "mach-name";
        private const string XmlProcess = "process";
        private const string XmlProcName = "proc-name";
        private const string XmlDtStamp = "dt-stamp";
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss fff";



        private static int processId;
        private static string machineName;
        private static string processName;

        private string _machineName;
        private int _processId;
        private string _processName;
        private int _eventId;  // application specific events
        private StringDictionary _propsBag; // Check whether we required name to object mapping or name to string 
        private DateTime _timeStamp;
        private LogLevel _level;
        private string _source;
        private int _threadId;
        private string _message;
        private System.Text.StringBuilder _msgBuilder;

        static LogRecord()
        {
            // Initialize the static members 
            try
            {
                Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                LogRecord.machineName = currentProcess.MachineName;
                LogRecord.processId = currentProcess.Id;
                LogRecord.processName = currentProcess.ProcessName;
            }
            catch (Exception ProcEx)
            {
                throw new FatalException(ProcEx, "Unable to get system and process information for populating log record fields");
            }
        }

        public LogRecord()
        {
            this._machineName = LogRecord.machineName;
            this._processId = LogRecord.processId;
            this._processName = LogRecord.processName;

            this._timeStamp = DateTime.Now;
            this._threadId = Thread.CurrentThread.ManagedThreadId;
        }

        internal LogRecord(LogLevel level, string source, int eventId, string message):this()
        {
            this._level = level;
            this._source = this.SetEmpty(source);
            this._eventId = eventId;
            this._message = this.SetEmpty(message);
        }

        internal LogRecord(LogLevel level, string source, string message)
            : this(level, source, LogRecord.UnknownEvent, message)
        {}

        public LogRecord(LogLevel level, int eventId, string message)
            : this(level, string.Empty, eventId, message)
        { }

        public LogRecord(LogLevel level,string message)
            : this(level,  LogRecord.UnknownEvent, message)
        { }

        /// <summary>
        /// get or set: Uniquely identified event Id.
        /// </summary>
        /// <remarks>Use it to trace execution of event in your application.</remarks>
        public int EventId
        {
            get { return this._eventId; }
            set { this._eventId = value; }
        }

        /// <summary>
        /// get: Time when this log record is created.
        /// </summary>
        public DateTime TimeStamp
        {
            get { return this._timeStamp; }
        }

        /// <summary>
        /// get or set: Level of information this record holds.
        /// </summary>
        public LogLevel Level
        {
            get { return this._level; }
            set { this._level = value; }
        }
        
        /// <summary>
        /// get: logger name which log this record.
        /// Note: If you only create LogRecord object your self, then logger will Source at logging time.
        /// </summary>
        public string Source
        {
            get { return this._source; }
        }

        protected internal string LogSource
        {
            get { return this._source; }
            set { this._source = this.SetEmpty(value); }
        }
        
        /// <summary>
        /// get: Thread Id under which record is created.
        /// </summary>
        public int ThreadId
        {
            get { return this._threadId; }
        }
        
        /// <summary>
        /// get: Process Id under which record is created.
        /// </summary>
        public int ProcessId
        {
            get { return this._processId; }
        }


        /// <summary>
        /// get: Process name under which record is created.
        /// </summary>
        public string ProcessName
        {
            get { return this._processName; }
        }

        /// <summary>
        /// get: Machine name under which record is created.
        /// </summary>
        public string MachineName
        {
            get { return this._machineName; }
        }
        
        /// <summary>
        /// get or set: Message of log information.
        /// </summary>
        public string Message
        {
            get { return this._message; }
            
            set { this._message = this.SetEmpty(value); }
        }
     
        /// <summary>
        /// Gives clone of record.
        /// </summary>
        /// <returns>LogRecord</returns>
        public object Clone()
        {
            LogRecord record = new LogRecord(this._level, this._source, this._eventId, this._message);

            record._machineName = this._machineName;
            record._processId = this._processId;
            record._processName = this._processName;
            record._threadId = this._threadId;
            record._timeStamp = this._timeStamp;

            if (this._msgBuilder != null)
                record._msgBuilder =  new StringBuilder(this._msgBuilder.ToString());

            if (this._propsBag != null)
            {
                record._propsBag = new StringDictionary();
                foreach (string key in this._propsBag.Keys)
                    record._propsBag.Add(key, this._propsBag[key]);
            }
            
            return record;
        }

        public override string ToString()
        {
            return string.Format(" [{0}, {1} , {2}] {3} | {4} | {5} | {6} ", this._processName, this.ThreadId, this.TimeStamp, this.Source, this.EventId, this.Level, this.Message);
        }

        /// <summary>
        /// Add new message to record. Use it if you have multiple messages.
        /// Note: In serialization This.Message will be override with multiple messages added here.
        /// </summary>
        /// <param name="message">New message you want to add</param>
        /// <remarks>In serialization This.Message will be override with multiple messages added here.</remarks>
        public virtual void AddErrorMessage(string message)
        {
            if (this._msgBuilder == null)
            {
                this._msgBuilder = new StringBuilder();
            }

            this._msgBuilder.Append(message);
            this._msgBuilder.Append(Environment.NewLine);
        }

        /// <summary>
        /// Add key/value pair to record information. i.e. username=Myname, age=Myage
        /// </summary>
        /// <param name="key">Uniquely identify value</param>
        /// <param name="value">value of specified key</param>
        public void AddProperty(string key, string value)
        {
            if (this._propsBag == null)
                this._propsBag = new StringDictionary();

            this._propsBag[key] = value;
        }

        /// <summary>
        /// Remove key/value pair to record information (if found).
        /// </summary>
        /// <param name="key">Uniquely identify value</param>
        /// <param name="value">value of specified key</param>
        public void RemoveProperty(string key, string value)
        {
            if(this._propsBag != null)
                this._propsBag.Remove(key);
        }

        public string GetProperty(string key)
        {
            if( (this._propsBag == null) || !this._propsBag.ContainsValue(key))
                throw new LogException("Property \"0\" not found.", key);

                return this._propsBag[key];
        }

        public bool HasProperty(string key)
        {
            if (this._propsBag == null)
                return false;

            return this._propsBag.ContainsValue(key);
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            
            throw new NotImplementedException("IXmlSerializable.GetSchema is not implemented in LogRecord class");
        }

        /// <summary>
        /// Gives you serialized xml structure of current record information.
        /// </summary>
        /// <param name="writer">XmlWriter where xml information should be written</param>
        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            if (this._msgBuilder != null)
            {
                this._message = this._msgBuilder.ToString();
            }

            if (writer.WriteState != WriteState.Element)
                throw new InvalidOperationException("Xml serialization error! root element 'log-rec' has not been written yet.");

            writer.WriteAttributeString(LogRecord.XmlMachName, this._machineName);
            writer.WriteAttributeString(LogRecord.XmlProcess, this._processId.ToString());
            writer.WriteAttributeString(LogRecord.XmlProcName, this._processName);
            writer.WriteAttributeString(LogRecord.XmlSource, this._source);
            writer.WriteAttributeString(LogRecord.XmlLevel, this._level.ToString());
            writer.WriteAttributeString(LogRecord.XmlEvent, this._eventId.ToString());
            writer.WriteAttributeString(LogRecord.XmlDtStamp, this._timeStamp.ToString(LogRecord.DateTimeFormat));
            writer.WriteAttributeString(LogRecord.XmlThread, this._threadId.ToString());

            // write msg in separate tag
            writer.WriteElementString(LogRecord.XmlMsg, this._message);
           // writer.WriteElementString(LogRecord.xmlMsg, "Farhan Shehzad");
            if( (this._propsBag != null) && (this._propsBag.Count > 0) )
            {
                writer.WriteStartElement(LogRecord.XmlPropsBag);
                foreach (string key in this._propsBag.Keys)
                {
                    writer.WriteStartElement(LogRecord.XmlItem);
                    writer.WriteAttributeString(LogRecord.XmlKey, key);
                    writer.WriteAttributeString(LogRecord.XmlValue, this._propsBag[key]);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); // PropertyBag
            }         
        }

        /// <summary>
        /// Reads serialized xml structure LogRecord and populate it self.
        /// </summary>
        /// <param name="reader">XmlReader from where xml information should be read</param>
        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            XmlUtil util = new XmlUtil(reader);

            if (reader.ReadState == ReadState.Initial)
                reader.Read();

            if (reader.Name.CompareTo(LogRecord.XmlNode) != 0)
                throw new LogException("Invalid xml format. Node:{0}", reader.Name);

            this._machineName = util.GetTextValue(LogRecord.XmlMachName);
            this._processId = util.GetIntValue(LogRecord.XmlProcess);
            this._processName = util.GetTextValue(LogRecord.XmlProcName);
            this._source = util.GetTextValue(LogRecord.XmlSource);
            this._level = util.GetEnumValue<LogLevel>(LogRecord.XmlLevel, LogLevel.Unknown);
            this._eventId = util.GetValue(LogRecord.XmlEvent, LogRecord.UnknownEvent);
            this._timeStamp = util.GetDateTimeValue(LogRecord.XmlDtStamp, LogRecord.DateTimeFormat);
            this._threadId = util.GetIntValue(LogRecord.XmlThread);

            reader.Read();
            while (!reader.EOF)
            {
                reader.MoveToContent();
                
                if(reader.IsStartElement(LogRecord.XmlPropsBag))
                {
                    this._propsBag = new StringDictionary();
                    
                }
                else if (reader.IsStartElement(LogRecord.XmlItem))
                {
                    if(this._propsBag == null)
                        throw new LogException("Incorrect XML format. {0} element not found while parsing at {1}", LogRecord.XmlPropsBag,
                                               reader.Name);

                        this._propsBag.Add(util.GetTextValue(LogRecord.XmlKey), util.GetTextValue(LogRecord.XmlValue));

                }
                else if (reader.IsStartElement(LogRecord.XmlMsg))
                {
                    this._message = reader.ReadString();
                }

                reader.Read();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement(LogRecord.XmlNode);
            ((IXmlSerializable)this).WriteXml(writer);
            writer.WriteEndElement(); //Closing XmlNode/log-rec
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            ((IXmlSerializable)this).ReadXml(reader);
        }

        private string SetEmpty(string value)
        {
            if (value == null)
                return string.Empty;
            else
                return value;
        }
    }
}
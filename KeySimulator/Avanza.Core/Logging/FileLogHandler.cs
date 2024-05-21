using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Avanza.Core.Configuration;


namespace Avanza.Core.Logging
{
    public class FileLogHandler : LogHandler
    {
        protected static readonly string DEFAULTLOGPATH = AppDomain.CurrentDomain.BaseDirectory;
        protected const string DEFAULTLOGFILENAME = "Avanza Log File.log";
        protected const int DEFAULTFILESIZEINBYTES = 1048576;

        protected string _logPath;
        protected string _logFileName;
        protected string _logFileExt;
        protected decimal _fileSizeInBytes;
        protected StreamWriter Writer = null;

        private FileInfo _currentFileInfo;
        private int _fileNumber = 0;
        private string _sadLogFileName;
        public FileLogHandler()
        { }

        public FileLogHandler(string name, LogLevel level)
            : base(name, level)
        { }


        #region LogHandler override functions

        public override void Initialize(Avanza.Core.Configuration.IConfigSection config)
        {
            try
            {
                //DH: Commented as type is moved to child subsection
                if (config.Name != "log-handler" /*|| config.GetTextValue("type") != "Avanza.Core.Logging.FileLogHandler" */)
                    throw new System.Configuration.ConfigurationErrorsException(string.Format("FileLogHandler intialization failed. Log handler is not Avanza.Util.Logging.FileLogHandler type. Node name: {0}", config.Name));

                IConfigSection childconfig = config.GetChild("config");
                if (childconfig == null)
                    throw new System.Configuration.ConfigurationErrorsException(string.Format("'config' element section is missing in log-handler section of name '{0}'", config.GetTextValue("name")));
                if (string.IsNullOrEmpty((this._name= config.GetTextValue("name"))))
                    throw new System.Configuration.ConfigurationErrorsException("Handler name in log-handler configuration cannot be null, handler type is FileLogHandler");

                try
                {
                    this.Level = (LogLevel)System.Enum.Parse(typeof(LogLevel), config.GetTextValue("level"), true);
                }
                catch 
                { 
                    this.Level = LogLevel.Unknown; 
                }

//                string fileName = childconfig.GetTextValue("file-name");              
                
                // PA-DSS: Check if ENABLE_SAD is 1 in system configuration. If yes, then check for SAD-log-file-name attribute. if this path does not exist then throw error
                Update(config);
				/*
                if (string.IsNullOrEmpty(fileName))
            {
                    this._logFileName = Path.Combine(DEFAULTLOGPATH, DEFAULTLOGFILENAME);
            }
                else
                {
                    if (Path.IsPathRooted(fileName))
                        this._logFileName = fileName;
                    else
                        this._logFileName = Path.Combine(DEFAULTLOGPATH, fileName);                    
                }

                string fileDir = Path.GetDirectoryName(this._logFileName);
                if (!Directory.Exists(fileDir) && !string.IsNullOrEmpty(fileDir))
                    Directory.CreateDirectory(fileDir);
                
                this._logFileExt = Path.GetExtension(fileName);
                this._logFileName = this._logFileName.Substring(0, this._logFileName.Length - this._logFileExt.Length);               
                
                this._fileSizeInBytes = this.ParseMaxSize(childconfig.GetTextValue("max-size"));

                long lastFileSize = -1;
                FileInfo logFile = null;
                while ((logFile = new FileInfo(string.Format("{0} {1}{2}", this._logFileName, ++this._fileNumber, this._logFileExt))).Exists)
                    lastFileSize = logFile.Length;

                if (lastFileSize < this._fileSizeInBytes && lastFileSize != -1)
                    logFile = new FileInfo(string.Format("{0} {1}{2}", this._logFileName, --this._fileNumber, this._logFileExt));

                this._currentFileInfo = logFile;
                FileStream Fs = new FileStream(this._currentFileInfo.FullName, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write, FileShare.Read);
                Writer = new StreamWriter(Fs);
                Writer.AutoFlush = true;
*/

            }
            catch (System.Xml.XmlException xmlEx)
            {
                throw new System.Configuration.ConfigurationErrorsException(string.Format("Error parsing FileLogHandler configuration section of '{0}'", this.Name), xmlEx);
            }
            catch (System.IO.IOException IOEx)
            {
                throw new System.Configuration.ConfigurationErrorsException(string.Format("Unable to create log file from log handler configuration section named '{0}'.", this.Name), IOEx);
            }
        }

        public override void Update(Avanza.Core.Configuration.IConfigSection config)
        {
            if (config.Name != "log-handler" /*|| config.GetTextValue("type") != "Avanza.Core.Logging.FileLogHandler" */)
                throw new System.Configuration.ConfigurationErrorsException(string.Format("FileLogHandler intialization failed. Log handler is not Avanza.Util.Logging.FileLogHandler type. Node name: {0}", config.Name));

            IConfigSection childconfig = config.GetChild("config");
            if (childconfig == null)
                throw new System.Configuration.ConfigurationErrorsException(string.Format("'config' element section is missing in log-handler section of name '{0}'", config.GetTextValue("name")));
            if (string.IsNullOrEmpty((this._name = config.GetTextValue("name"))))
                throw new System.Configuration.ConfigurationErrorsException("Handler name in log-handler configuration cannot be null, handler type is FileLogHandler");
            
            string fileName = string.Empty;
            if (LogManager.ComponentType == EComponentType.VisionClient || LogManager.ComponentType == EComponentType.VisionServer)
            {
                fileName = childconfig.GetTextValue("file-name");
            }
            else
            {
                if (LogManager.IsSADEnabled)
                {
                    if (childconfig.GetTextValue("SAD-log-file-name") != null)
                        fileName = childconfig.GetTextValue("SAD-log-file-name");
                    else
                        throw new System.Configuration.ConfigurationErrorsException("Define SAD-log-file-name in log configuration section, handler type is FileLogHandler");
                }
                else
                {
                    if (childconfig.GetTextValue("SAD-log-file-name") != null)
                        _sadLogFileName = childconfig.GetTextValue("SAD-log-file-name");
                    else
                        throw new System.Configuration.ConfigurationErrorsException("Define SAD-log-file-name in log configuration section, handler type is FileLogHandler");
                    fileName = childconfig.GetTextValue("file-name");
                }
            }

            if (string.IsNullOrEmpty(fileName))
            {
                this._logFileName = Path.Combine(DEFAULTLOGPATH, DEFAULTLOGFILENAME);
            }
            else
            {
                if (Path.IsPathRooted(fileName))
                    this._logFileName = fileName;
                else
                    this._logFileName = Path.Combine(DEFAULTLOGPATH, fileName);
            }

            string fileDir = Path.GetDirectoryName(this._logFileName);
            if (!Directory.Exists(fileDir) && !string.IsNullOrEmpty(fileDir))
                Directory.CreateDirectory(fileDir);

            this._logFileExt = Path.GetExtension(fileName);
            this._logFileName = this._logFileName.Substring(0, this._logFileName.Length - this._logFileExt.Length);

            this._fileSizeInBytes = this.ParseMaxSize(childconfig.GetTextValue("max-size"));

            long lastFileSize = -1;
            FileInfo logFile = null;
            while ((logFile = new FileInfo(string.Format("{0} {1}{2}", this._logFileName, ++this._fileNumber, this._logFileExt))).Exists)
                lastFileSize = logFile.Length;

            if (lastFileSize < this._fileSizeInBytes && lastFileSize != -1)
                logFile = new FileInfo(string.Format("{0} {1}{2}", this._logFileName, --this._fileNumber, this._logFileExt));

            this._currentFileInfo = logFile;
            FileStream Fs = new FileStream(this._currentFileInfo.FullName, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write, FileShare.Read);
            Writer = new StreamWriter(Fs);
            Writer.AutoFlush = true;
        }
		
        public override void Publish(LogRecord record)
        {
            if (!IsLoggable(record.Level))
                return;
            try
            {
                Writer.WriteLine(" {0} | {1} | {2} | {3} | {4} | {5} ", record.TimeStamp, record.ThreadId, record.EventId, record.Level, record.Source, record.Message);
                CheckFileSize();//checking for file size. if exceeded file will be writer will move to new file no.
            }
            catch (System.IO.IOException IOEx)
            {
                throw new LogException(IOEx, "Error occured writing to {0} file in {1} log handler", this._currentFileInfo, this.Name);
            }
        }

        public override void Close()
        {
            Writer.Flush();
            Writer.Close();
        }

        public override void Flush()
        {
            Writer.Flush();
        }

        #endregion

        private void CheckFileSize()
        {
            this._currentFileInfo.Refresh();
            if (this._currentFileInfo.Length >= this._fileSizeInBytes)
            {
                Writer.Close();
                Writer.Dispose();

                this._currentFileInfo = new FileInfo(string.Format("{0} {1}{2}", this._logFileName, ++this._fileNumber, this._logFileExt));
                FileStream Fs = new FileStream(this._currentFileInfo.FullName, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write, FileShare.Read);
                Writer = new StreamWriter(Fs);
                Writer.AutoFlush = true;
            }
        }

        public override void Dispose()
        {
            if (Writer != null)
            {
                Writer.Flush();
                Writer.Close();
                Writer.Dispose();
            }
        }

        private decimal ParseMaxSize(string maxSize)
        {
            if (!string.IsNullOrEmpty(maxSize))
            {
                decimal size = 0;
                string Scale = "";

                try
                {
                    size = Decimal.Parse(maxSize.Substring(0, maxSize.Length - 2));
                    Scale = maxSize.Substring(maxSize.Length - 2, 2);
                }
                catch
                {
                    throw new System.Configuration.ConfigurationErrorsException(string.Format("Invalid file size ({0}) define in log-handler config section of {1}", maxSize, this.Name));
                }
                
                switch (Scale)
                {
                    case "KB":
                        return 1024 * size;
                    case "MB":
                        return 1048576 * size;
                    case "GB":
                        return 1073741824 * size;
                    default:
                        throw new System.Configuration.ConfigurationErrorsException(string.Format("Invalid file size ({0}) define in log-handler config section of {1}", maxSize, this.Name));
                }
            }
            else
                return DEFAULTFILESIZEINBYTES;
        }
    }
}

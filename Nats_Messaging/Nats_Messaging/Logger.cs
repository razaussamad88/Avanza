using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Nats_Messaging
{
    public enum LogLevel : uint
    {
        Off = 0,
        Error = 1,
        Debug = 2,
        Warning = 3,
        Info = 4,
        Verbose = 5
    }
    public class Logger
    {
        private static readonly object lockObject = new object();
        private static string filePath;
        private static string fileName;
        private static string fileExtension;
        private static string fileDate;
        private static FileStream fileStream = null;
        private static StreamWriter streamWriter = null;
        private static LogLevel logLevel;
        private static Logger instance;

        private Logger(string logPath, uint logLevel)
        {
            // get the file path
            filePath = Path.GetDirectoryName(logPath);

            // create the directory if does not exist
            if (Directory.Exists(filePath) == false)
                Directory.CreateDirectory(filePath);

            // get the file name
            fileName = Path.GetFileNameWithoutExtension(logPath);

            // get the file extension
            fileExtension = Path.GetExtension(logPath);

            // get the current formatted date
            DateTime date = DateTime.Now;
            fileDate = date.Month.ToString().PadLeft(2, '0') + date.Day.ToString().PadLeft(2, '0');

            // get the log level
            if (Enum.IsDefined(typeof(LogLevel), logLevel) == false)
            {
                logLevel = 5;
            }
            Logger.logLevel = (LogLevel)logLevel;
        }

        public static void CreateInstance(string logPath, uint logLevel)
        {
            if (instance == null)
            {
                instance = new Logger(logPath, logLevel);
            }
        }

        public static void LogError(string message, params object[] param)
        {
            Log(LogLevel.Error, message, param);
        }

        public static void LogDebug(string message, params object[] param)
        {
            Log(LogLevel.Debug, message, param);
        }

        public static void LogInfo(string message, params object[] param)
        {
            Log(LogLevel.Info, message, param);
        }

        public static void LogVerbose(string message, params object[] param)
        {
            Log(LogLevel.Verbose, message, param);
        }

        public static void LogWarning(string message, params object[] param)
        {
            Log(LogLevel.Warning, message, param);
        }

        private static void Log(LogLevel logLevel, string message, params object[] param)
        {
            if (instance != null)
            {
                if ((uint)logLevel <= (uint)Logger.logLevel && Logger.logLevel != LogLevel.Off && logLevel != LogLevel.Off)
                {
                    try
                    {
                        lock (lockObject)
                        {
                            // close the old file and open new file if required
                            if (IsDateChangeRequired() == true)
                            {
                                if (streamWriter != null)
                                {
                                    streamWriter.Close();
                                    streamWriter = null;
                                }
                                if (fileStream != null)
                                {
                                    fileStream.Close();
                                    fileStream = null;
                                }
                            }
                            if (fileStream == null)
                            {
                                fileStream = new FileStream(Path.Combine(filePath, fileName + fileDate + fileExtension), FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                                streamWriter = new StreamWriter(fileStream);
                                streamWriter.AutoFlush = true;
                            }

                            // get the method name
                            StackTrace st = new StackTrace(false);
                            string methodName = "";
                            if (st.FrameCount > 2)
                            {
                                StackFrame frame = st.GetFrame(2);
                                methodName = frame.GetMethod().DeclaringType.Name + "." + frame.GetMethod().Name;
                            }
                            methodName = methodName.PadRight(50, ' ');
                            if (methodName.Length > 50)
                                methodName = methodName.Remove(50);

                            // get User Id
                            string userName = string.Empty;
                            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["SessionUserName"] != null)
                            {
                                userName = System.Web.HttpContext.Current.Session["SessionUserName"].ToString();
                            }
                            if (string.IsNullOrEmpty(userName) == false)
                            {
                                userName = userName.PadRight(25, ' ');
                            }
                            else
                            {
                                userName = new string(' ', 25);
                            }

                            if (param?.Length > 0)
                            {
                                message = string.Format(message, param);
                            }

                            // write to the file
                            streamWriter.WriteLine("{0}|{1}|{2}|{3}|{4}", DateTime.Now.ToString("yy/MM/dd HH:mm:ss.fff"), logLevel.ToString().PadRight(8, ' '), userName, methodName, message);
                            streamWriter.Flush();
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static bool IsDateChangeRequired()
        {
            // check for date change
            DateTime date = DateTime.Now;
            string newFileDate = date.Month.ToString().PadLeft(2, '0') + date.Day.ToString().PadLeft(2, '0');
            if (newFileDate.Equals(fileDate) == false)
            {
                fileDate = newFileDate;
                return true;
            }
            return false;
        }
    }
}

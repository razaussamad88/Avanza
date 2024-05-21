using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ThreadPoolConsoleApp
{
    class Logger
    {
        private static string m_FileName = null;
        private static string m_Path = null;

        private static volatile object m_Lock = new object();

        static Logger()
        {
            m_FileName = String.Format("WriteLines_{0:ddMMMyyyy_HHmmss}.log", DateTime.Now);
            m_Path = Directory.GetCurrentDirectory();
        }

        public static void Init() { }

        public static void WriteLine(string message, params object[] param)
        {
            lock (m_Lock)
            {
                string finalMsg = (param == null || param.Length == 0) ? message : String.Format(message, param);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(m_Path, m_FileName), true))
                {
                    outputFile.WriteLine("{0} |  {1}", DateTime.Now, finalMsg);
                }

                Console.WriteLine(finalMsg);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmailSenderAsync
{
    public static class iConsole
    {
        private static StringBuilder sb = new StringBuilder();

        static iConsole()
        {
            //Console.SetOut(new iConsole());
        }

        public static void WriteLine(string value)
        {
            sb.AppendLine(value);

            Console.WriteLine(value);
        }

        public static void WriteLine(string format, object arg0)
        {
            sb.AppendFormat(format, arg0);
            sb.AppendLine();

            Console.WriteLine(format, arg0);
        }

        public static void WriteLine(string format, object arg0, object arg1)
        {
            sb.AppendFormat(format, arg0, arg1);
            sb.AppendLine();

            Console.WriteLine(format, arg0, arg1);
        }

        public static void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            sb.AppendFormat(format, arg0, arg1, arg2);
            sb.AppendLine();

            Console.WriteLine(format, arg0, arg1, arg2);
        }

        public static void WriteLine(string format, params object[] arg)
        {
            sb.AppendFormat(format, arg);
            sb.AppendLine();

            Console.WriteLine(format, arg);
        }

        public static StringBuilder Output
        {
            get
            {
                return sb;
            }
        }

        public static void WriteAll()
        {
            FileStream ostrm;
            StreamWriter writer;
            string fileName = $"./Output-{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";

            try
            {
                ostrm = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
                writer.WriteLine(iConsole.Output.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Cannot open file '{fileName}' for writing");
                Console.WriteLine(e.Message);
                return;
            }

            writer.Close();
            ostrm.Close();

            sb.Clear();
        }
    }
}
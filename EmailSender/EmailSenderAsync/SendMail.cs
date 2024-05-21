﻿using System;
using System.Net.Mail;
using System.Threading;

namespace EmailSenderAsync
{
    public class SendMail : BaseSendMail
    {
        //private static volatile object m_Lock = new object();
        //private static SmtpClient client = null;
        //private static string email_html = "<html> <head> <style>.cls_mpgweb_avanza_01{height:100%!important;width:100%!important;border-spacing:0;border-collapse:collapse}.cls_mpgweb_avanza_03{width:100%;border-spacing:0;border-collapse:collapse;margin:40px 0 20px}.cls_mpgweb_avanza_15{font-family:-apple-system,BlinkMacSystemFont,\"Segoe UI\",Roboto,Oxygen,Ubuntu,Cantarell,\"Fira Sans\",\"Droid Sans\",\"Helvetica Neue\",sans-serif}.cls_mpgweb_avanza_05{width:560px;text-align:left;border-spacing:0;border-collapse:collapse;margin:0 auto}.cls_mpgweb_avanza_10{width:560px;text-align:left;border-spacing:0;border-collapse:collapse;margin:0 auto;}.cls_mpgweb_avanza_20{width:100%;border:0;background:#e5e5e5;height:.05em}.cls_mpgweb_avanza_100{width:100%;border-spacing:0;border-collapse:collapse}.cls_mpgweb_avanza_110{font-weight:400;font-size:20px;margin:0 0 10px}.cls_mpgweb_avanza_120,.cls_mpgweb_avanza_130{color:#777;line-height:150%;font-size:16px;margin:0}.cls_mpgweb_avanza_130{padding-top:2em}.cls_mpgweb_avanza_140{font-size:16px;text-decoration:none;color:#000;margin-left:1em}.cls_mpgweb_avanza_150{color:#777;line-height:150%;font-size:16px;margin:0;padding-top:.5em}.cls_mpgweb_avanza_155{width:60px;display:inline-block}.cls_mpgweb_avanza_160{font-size:16px;text-decoration:none;color:#FFF;margin-left:1em;background:#088FCC;padding:5px 8px;border-radius:6px;font-weight:bold;border-color:#FFF;letter-spacing:4pt;text-align:center;}.cls_mpgweb_avanza_-adjust{width:100%;height:15%}.cls_mpgweb_avanza_-mpgweb{font-size:16px;text-decoration:none;color:#000}</style> </head> <body style=\"width: 800px;!important\"> <table class=\"cls_mpgweb_avanza_01\"> <tbody> <tr> <td class=\"cls_mpgweb_avanza_15\"> <table style=\"width:100%;border-spacing:0;border-collapse:collapse\"> <tbody> <tr> <td class=\"cls_mpgweb_avanza_15\" style=\"padding-bottom:40px;border:0\"> <center> <div class=\"cls_mpgweb_avanza_10\" style=\"padding: 10px;border:1px solid #EFEFEF;margin:10px\"> <table class=\"cls_mpgweb_avanza_10\"> <tbody> <tr> <td class=\"cls_mpgweb_avanza_15\"> <p style=\"padding:1em 0;margin:0\"><img src=\"data:image/png;base64,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\" alt=\"Micro payment Gateway\" style=\"max-width:140px;\"></p> </td> </tr> <tr> <td class=\"cls_mpgweb_avanza_15\"> <h2>Rdv Notification Service&nbsp;<h2 class=\"cls_mpgweb_avanza_110\"><span>TEST EMAIL – PLEASE IGNORE</span></h2></h2> <p class=\"cls_mpgweb_avanza_120\">We are working to improve our email system. We apologize for the unnecessary email, but it is the best way to solve the problems experienced.<br/><br/>Thank you for your patience with us and support!<br/><br/>Warmest regards,<br/>Avanza Product Development Team</p> </td> </tr> </tbody> </table> </div> </center> </td> </tr> </tbody> </table> <hr class=\"cls_mpgweb_avanza_20\"> <div class=\"cls_mpgweb_avanza_-adjust\"></div> </td> </tr> </tbody> </table> </body> </html>";

        public static async void SendEmail(string toAddress)
        {
            uint threadId = Convert.ToUInt32(Thread.CurrentThread.ManagedThreadId);
            string body = email_html;

            client = Program.getSmtpClient();

            MailMessage message = new MailMessage(Program.FromAddress, toAddress, Program.subject, body);
            message.IsBodyHtml = true;

            DateTime reqTime = DateTime.Now;

            string reqLine = String.Format("REQ # {0:00}/{1:00} [{2}]", ++Program.m_RequestCounter, Program.toRecepients.Count, toAddress);
            string resLine = String.Format("RES # {0:00}/{1:00} [{2}]", ++Program.m_ResponseCounter, Program.toRecepients.Count, toAddress);
            string msgErr = null;

            iConsole.WriteLine(String.Empty);
            iConsole.WriteLine("  {1} [{0}] Sending email...", DateTime.Now, reqLine);

            try
            {
                client.SendAsync(message, reqLine);
            }
            catch (SmtpException ex)
            {
                msgErr = ex.InnerException == null ? ex.Message : String.Concat(ex.Message, "... ", ex.InnerException.Message);
                resLine = String.Format("  *** ERROR [{0}]", toAddress);
                Program.m_Error_Counter++;
            }
        }

        public static void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DateTime receiveTime = DateTime.Now;

            Program.m_SendCompleted_Counter++;
            uint threadId = Convert.ToUInt32(Thread.CurrentThread.ManagedThreadId);

            string fmt_TmpStr = "  -> RES # {0:00}/{1:00}  [{2}] Email {3}\t>> {4}";
            string eventResponsed = null;

            if (e.Cancelled)
            {
                eventResponsed = "Cancelled";
            }
            else if (e.Error != null)
            {
                eventResponsed = "Failed";
                Program.m_Error_Counter++;
            }
            else
            {
                eventResponsed = "Sent!!";
            }

            iConsole.WriteLine(fmt_TmpStr, Program.m_SendCompleted_Counter, Program.toRecepients.Count, receiveTime, eventResponsed.PadRight(8, ' '), e.UserState);


            lock (m_Lock)
            {
                if (Program.m_SendCompleted_Counter >= Program.toRecepients.Count)
                {
                    iConsole.WriteLine(String.Empty);
                    iConsole.WriteLine(String.Empty.PadLeft(Program.headLine.Length, '='));
                    iConsole.WriteLine("Total Emails : {0:00}   |   Success : {1:00}   |   Failed : {2:00}", Program.toRecepients.Count, (Program.toRecepients.Count - Program.m_Error_Counter), Program.m_Error_Counter);

                    iConsole.WriteAll();
                }
            }
        }
    }
}

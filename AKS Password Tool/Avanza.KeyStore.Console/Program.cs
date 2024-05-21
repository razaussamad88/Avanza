using Avanza.Common.Logging;
using System;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Avanza.KeyStore.Console
{
    public class Program
    {
        private const string TabKey = "\t";
        private static string exitCode = null;
        private static string seg = null;

        static Program()
        {
            seg = String.Concat("-".PadLeft(25, '-'), " Avanza Key Store Console ", "-".PadRight(25, '-'));
        }

        public static void Main(string[] args)
        {
            try
            {
                while (String.IsNullOrEmpty(exitCode) || !exitCode.ToUpper().Equals("Q"))
                {
                    if (!String.IsNullOrEmpty(exitCode))
                        System.Console.WriteLine(seg);

                    KeyStore aks = new KeyStore();

                    System.Console.WriteLine(String.Empty);
                    System.Console.WriteLine(new StringBuilder()
                        .AppendLine("Press 1 - To Encrypt, the Plain Text.")
                        .AppendLine("Press 2 - To Decrypt, the Encrypted Text.")
                        .AppendLine("Press Q - To Exit...")
                        );

                    string output = string.Empty;
                    var key = System.Console.ReadKey();
                    System.Console.WriteLine(String.Empty);

                    switch (key.KeyChar.ToString().ToUpper())
                    {
                        case "1":
                            System.Console.WriteLine("Please enter [PLAIN] text to encrypt : ");
                            output = aks.Encrypt(System.Console.ReadLine());
                            Print("[ENCRYPTED] text", output);
                            Pause();
                            break;

                        case "2":
                            System.Console.WriteLine("Please enter [ENCRYPTED] text to decrypt : ");
                            output = aks.Decrypt(System.Console.ReadLine());
                            Print("[DECRYPTED] text", output);
                            Pause();
                            break;

                        case "Q":
                            System.Console.WriteLine("Application Shutting Down...");
                            exitCode = "Q";
                            Thread.Sleep(1000);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ActivityLogger.Instance.Log(Constants.User, MethodBase.GetCurrentMethod(), ex);
            }
        }

        private static void Print(string title, string data)
        {
            System.Console.WriteLine("{0}{2}:   {{{1}}}", title, data, TabKey);
        }

        private static void Pause()
        {
            Print("[ENCRYPTED DEK] Key", KeyStoreBroker.EncryptedDEK);
            Print("[CLEARED DEK] Key", KeyStoreBroker.ClearDEK);

            System.Console.WriteLine(String.Empty);
            System.Console.WriteLine("Press 'Q' to Exit...");

            var key = System.Console.ReadKey();
            exitCode = key.KeyChar.ToString();

            System.Console.WriteLine(String.Empty);
            System.Console.WriteLine(String.Empty);
        }
    }
}

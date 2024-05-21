using System;

namespace EmailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            EmailSender email = new EmailSender();

            if (email.SentChangedPassword("muhammad.raza@avanzasolutions.com", "Avanza123"))
            {
                Console.WriteLine("Sent Successfully!");
            }
            else
            {
                Console.WriteLine("Sending Fail!");
            }

            Console.Write("Press any key to continue...");
            Console.Read();
        }
    }
}
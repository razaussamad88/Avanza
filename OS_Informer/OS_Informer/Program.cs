using Microsoft.Management.Infrastructure;
using System;
using System.Management;

namespace OS_Informer
{
    public enum eStates
    {
        Start = 2,
        Stop = 4,
        Pause = 6,
    }

    public partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Main Server"); Console.WriteLine(String.Empty);

            try
            {
                //RemoteCopy.Run();
                RemoteNodeCopier obj = new RemoteNodeCopier();
                obj.CopyFiles();
                obj.Run();
                /*
                TCimSession_Machine obj = new TCimSession_Machine();
                do
                {
                    //obj.Get_LOCAL_SysDetails();
                    //obj.Get_REMOTE_SysDetails();
                } while (false);
                */

                //InvokeMethod.Run();
            }
            catch (CimException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine(String.Empty);
            Console.WriteLine("Complete Main");
            Console.ReadLine();
        }
    }
}

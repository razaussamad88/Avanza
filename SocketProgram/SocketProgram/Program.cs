using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// Socket Listener acts as a server and listens to the incoming
// messages on the specified port and protocol.
public class SocketListener
{
    public static int Main(String[] args)
    {
        StartServer();
        return 0;
    }

    public static void StartServer()
    {
        // Get Host IP Address that is used to establish a connection
        // In this case, we get one IP address of localhost that is IP : 127.0.0.1
        // If a host has multiple addresses, you will get a list of addresses

        var SocketIP = ConfigurationManager.AppSettings["SocketIP"];
        var SocketPort = Convert.ToInt32(ConfigurationManager.AppSettings["SocketPort"]);

        //IPHostEntry host = Dns.GetHostEntry("localhost");
        //IPAddress ipAddress = host.AddressList[0];
        //IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        IPAddress ipAddress = IPAddress.Parse(SocketIP);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, SocketPort);
        //IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        try
        {
            //CreateSyncSocket(ipAddress, localEndPoint);
            CreateAsyncSocket(ipAddress, localEndPoint);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        Console.WriteLine("\n Press any key to continue...");
        Console.ReadKey();
    }

    private static void CreateSyncSocket(IPAddress ipAddress, IPEndPoint localEndPoint)
    {
        // Create a Socket that will use Tcp protocol
        Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        // A Socket must be associated with an endpoint using the Bind method
        listener.Bind(localEndPoint);
        // Specify how many requests a Socket can listen before it gives Server busy response.
        // We will listen 10 requests at a time
        listener.Listen(10);

        listener.ReceiveTimeout = listener.SendTimeout = (int)TimeSpan.FromSeconds(60).TotalMilliseconds;
        listener.ReceiveBufferSize = listener.SendBufferSize = 10000;

        Console.WriteLine("Waiting for a connection...");
        Socket handler = listener.Accept();

        // Incoming data from the client.
        string data = null;
        byte[] bytes = null;

        while (true)
        {
            bytes = new byte[1024];
            int bytesRec = handler.Receive(bytes);
            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

            if (data.IndexOf("<EOF>") > -1)
            {
                break;
            }
        }

        Console.WriteLine("Text received : {0}", data);

        byte[] msg = Encoding.ASCII.GetBytes(data);
        handler.Send(msg);
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }

    private static void CreateAsyncSocket2(IPAddress ipAddress, IPEndPoint localEndPoint)
    {
        Socket s = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            s.Bind(localEndPoint);
            s.Listen(1000);
            s.ReceiveTimeout = s.SendTimeout = (int)TimeSpan.FromSeconds(60).TotalMilliseconds;
            s.ReceiveBufferSize = s.SendBufferSize = 10000;

            ManualResetEvent allDone = new ManualResetEvent(false);

            while (true)
            {
                allDone.Reset();

                Console.WriteLine("Waiting for a connection...");
                //s.BeginAccept(new AsyncCallback(Async_Send_Receive.Listen_Callback), s);

                allDone.WaitOne();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }


    private static void CreateAsyncSocket(IPAddress ipAddress, IPEndPoint localEndPoint)
    {
        DoBeginAcceptSocket(new TcpListener(ipAddress, localEndPoint.Port));
    }


    // Thread signal.
    public static ManualResetEvent clientConnected = new ManualResetEvent(false);

    // Accept one client connection asynchronously.
    public static void DoBeginAcceptSocket(TcpListener listener)
    {
        // Set the event to nonsignaled state.
        clientConnected.Reset();

        // Start to listen for connections from a client.
        Console.WriteLine("Waiting for a connection...");

        listener.Start();

        // Accept the connection.
        // BeginAcceptSocket() creates the accepted socket.
        listener.BeginAcceptSocket(new AsyncCallback(DoAcceptSocketCallback), listener);

        // Wait until a connection is made and processed before
        // continuing.
        clientConnected.WaitOne();
    }

    // Process the client connection.
    public static void DoAcceptSocketCallback(IAsyncResult ar)
    {
        // Get the listener that handles the client request.
        TcpListener listener = (TcpListener)ar.AsyncState;

        // End the operation and display the received data on the
        //console.
        Socket clientSocket = listener.EndAcceptSocket(ar);

        // Process the connection here. (Add the client to a
        // server table, read data, etc.)
        Console.WriteLine("Client connected completed");

        // Signal the calling thread to continue.
        clientConnected.Set();
    }
}
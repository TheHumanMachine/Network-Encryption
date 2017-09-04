using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Networking_Encryption
{
    public class CStateObject
    {
        public Socket workSocket = null;

        public const int BufferSize = 256;

        public byte[] buffer = new byte[BufferSize];

        public StringBuilder sb = new StringBuilder();
    }

    class ServerClient
    {
        #region ServerClient Variables 
        private const String HOST = "localhost";

        private const int PORT = 8008;

        private static ManualResetEvent connectDone = new ManualResetEvent(false);

        private static ManualResetEvent sendDone = new ManualResetEvent(false);

        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static String response = String.Empty;
        #endregion 

        public static void StartClient()
        {
            try
            {
                Console.WriteLine("Server Client Started...");
                IPHostEntry ipHostInfo = Dns.GetHostEntry(HOST);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                Console.WriteLine("Client IpAddress: {0}", ipAddress);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, PORT);

                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                Send(client, "This is a test<EOF>");
                sendDone.WaitOne();

                Receive(client);
                receiveDone.WaitOne();

                Console.WriteLine("Response received : {0}", response);

                Console.ReadLine();

                Disconnect(client);               

            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
                connectDone.Set();
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #region Send/Receive Functions
        private static void Receive(Socket client)
        {
            try
            {
                CStateObject state = new CStateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, CStateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                CStateObject state = (CStateObject)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesRead = client.EndReceive(ar);

                if(bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    client.BeginReceive(state.buffer, 0, CStateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }else
                {
                    if(state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    receiveDone.Set();
                }
            }catch(Exception e)
            {
                e.ToString();
            }
        }

        private static void Disconnect(Socket client)
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        private static void SendFile(Socket client)
        {

        }

        private static void Send(Socket client, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                sendDone.WaitOne();
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion 

        //Needs a main - Has to be ran as a different program than the server to see if they connect properly. 
    }
}

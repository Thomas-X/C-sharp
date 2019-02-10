using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using _10_ws_webserver.DataStructures;

namespace _10_ws_webserver
{
/* [X] - Basic HTTP parser (just the type and route requested)
 * [X] - Resolve files base on walking the public directory
 * [] - Add option to have routes like /about /me etc.
 * [] - Add session handling
 * [] - Support all mimetypes & fix bug where favicon is not loading for some reason
 * 
 *
 *
 */
    public class Webserver
    {
        public string Address { get; }
        public int Port { get; }

        // 10mb max response by default (this is really inefficient with higher loads, TODO)
        public static int ReceivingDataBufferSize { get; private set; } = 1 * 1000000;

        private IPAddress localAddress;

        private TcpListener listener;

        // The amount of backlog threads to keep listening for requests for
        private int backlog = 128;


        public Webserver(string address, int port)
        {
            Address = address;
            Port = port;
        }

        public void StartListening()
        {
            localAddress = IPAddress.Parse(Address);
            listener = new TcpListener(localAddress, Port);
            listener.Start(backlog);
            Console.WriteLine("Ready on port {0}", Port);
        }

        public void TcpClientListener()
        {
            // Perform a blocking call to accept requests.
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Connected to an incoming connection!");
           

            var stream = client.GetStream();
            int i;
            
            try
            {
                byte[] bytes = new byte[Webserver.ReceivingDataBufferSize];
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var data = Encoding.UTF8.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    var dataIndex = data.IndexOf("\r\n\r\n", StringComparison.Ordinal);
                    Console.WriteLine("Index of data {0}", dataIndex);
                    Console.WriteLine("Data: {0}", data.Substring(dataIndex));

                    var headersUnParsed = data.Substring(0, dataIndex);
                    var receivedData = data.Substring(dataIndex);

                    byte[] response = new Router(
                        new HttpRequest(headersUnParsed, receivedData)
                    ).Render();
                    
                    // Write the encoded http response to the stream
                    stream.Write(response, 0, response.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Closing connection.. TCPSOCKETID: {0}", Task.CurrentId);
            client.Close();
        }

        public void WaitForConnections()
        {
            while (true)
            {
                Console.WriteLine("Refreshed thread pool");
                // Create TCP listener pool
                Task[] tasks = new Task[backlog];

                for (int i = 0; i < tasks.Length; i++)
                {
                    Task task = Task.Run(() => TcpClientListener());
                    tasks.SetValue(task, i);
                }

                // Till all tasks have completed (each "task" can handle 1 request, we have a pool of tasks based on the backlog attribute)
                Task.WaitAll(tasks);
            }

            // ReSharper disable once FunctionNeverReturns
        }


        public void Listen()
        {
            try
            {
                StartListening();
                WaitForConnections();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients. Only reached after an error since while(true) in the try block
                listener.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
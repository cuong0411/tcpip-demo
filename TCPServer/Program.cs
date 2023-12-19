using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Program
    {
        private const int BUFFER_SIZE = 4096;
        private const int PORT_NUMBER = 12345;
        private const string IP_ADDRESS = "127.0.0.1";

        static void Main()
        {
            Console.Title = "Server";
            TcpListener listener = null;
            try
            {
                IPAddress ipAddress = IPAddress.Parse(IP_ADDRESS);
                listener = new TcpListener(ipAddress, PORT_NUMBER);
                listener.Start();
                Console.WriteLine($"Server is listening on {ipAddress}:{PORT_NUMBER}");

                while (true)
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();
                    Console.WriteLine($"Connection received from {tcpClient.Client.RemoteEndPoint}");

                    NetworkStream stream = tcpClient.GetStream();
                    byte[] bytes = new byte[BUFFER_SIZE];
                    int bytesRead;

                    while ((bytesRead = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {

                        string clientMessage = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                        Console.WriteLine($"Client: {clientMessage}");

                        byte[] response = Encoding.UTF8.GetBytes("Server recieved client's message successfully");
                        stream.Write(response, 0, response.Length);
                    }
                    tcpClient.Close();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
            }
            finally
            {
                if (listener != null) listener.Stop();
            }

            Console.WriteLine("Server is closed");
            Console.Read();
        }
    }
}

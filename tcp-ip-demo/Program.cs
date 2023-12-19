using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            try
            {
                Console.Write("Enter hostname: ");
                string serverIP = Console.ReadLine();
                Console.Write("Enter port: ");
                int serverPort = Int32.Parse(Console.ReadLine());

                TcpClient tcpClient = new TcpClient(serverIP, serverPort);
                Console.WriteLine($"Client connected to server {serverIP}:{serverPort}");

                NetworkStream stream = tcpClient.GetStream();

                Console.WriteLine(">>> Enter your message to client (type 'exit' to close connection): ");
                while (true)
                {
                    Console.Write(">>> Client: ");
                    string message = Console.ReadLine();
                    if (message.ToLower() == "exit")
                    {
                        break;
                    }

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    data = new byte[4096];
                    int bytesRead = stream.Read(data, 0, 4096);
                    string serverResponse = Encoding.UTF8.GetString(data, 0, bytesRead);
                    Console.WriteLine($">>> Server: {serverResponse}");
                }

                tcpClient.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            Console.WriteLine("Client Disconnected");
            Console.Read();
        }
    }
}

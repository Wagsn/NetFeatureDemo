using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketServer
{
    // TCP Socket http://www.jytek.com/seesharpsocket
    // UDP Socket https://blog.csdn.net/i1tws/article/details/86624951
    // TCP 与 Socket 的关系 https://www.cnblogs.com/xuan52rock/p/9454696.html
    // 通信框架 使用它进行互相通信，为什么不用HTTP请求？
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SocketServer is Run");
            try
            {
                var pro = new Program();
                pro.Server_Listen();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Fatal error occurred: " + ex.ToString());
            }
            Console.ReadKey();
        }

        // 服务端用于socket.Accept()
        // 与客户端的连接 多线程公共资源
        Dictionary<string, Socket> socketClientConns { get; } = new Dictionary<string, Socket>();
        // 连接所在的线程 多线程公共资源
        Dictionary<string, Thread> threadClients { get; } = new Dictionary<string, Thread>();

        private void Server_Listen()
        {
            //第一步：绑定端口
            // 服务端用于监听
            Socket socketListen = CreateServerSocket();

            //第二步：开启线程，进行监听
            Thread thrAccept = new Thread(Watching);
            thrAccept.IsBackground = true;
            thrAccept.Start(socketListen);
            Console.WriteLine("Server started Listen on "+ socketListen.LocalEndPoint.ToString());
            // 死循环发送数据到客户端
            while (true)
            {
                Console.Write("Receive to client> ");
                var str = Console.ReadLine();
                // 第五步：发送信息
                byte[] arrByte = System.Text.Encoding.UTF8.GetBytes($"{str} - by Server on {DateTime.Now.ToString()}");
                foreach(var comunicate in socketClientConns)
                {
                    if(comunicate.Value != null && comunicate.Value.Connected)
                    {
                        // TODO 数据转发
                        comunicate.Value.Send(arrByte);
                    }
                }
            }
        }

        private Socket CreateServerSocket(int port = 1014, int connCount = 10)
        {
            //IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(connCount);
            return socket;
        }

        /// <summary>
        /// 一个服务器Socket监听端口接受连接一个线程
        /// </summary>
        /// <param name="obj"></param>
        private void Watching(object obj)
        {
            var socket = obj as Socket;
            while (true)
            {
                //第三步：监听到请求后返回一个通信套接字，开启线程以接收信息
                var socketClientConn = socket.Accept();
                Console.WriteLine("Received a connection request from " + socketClientConn.RemoteEndPoint.ToString());
                socketClientConns.Add(socketClientConn.RemoteEndPoint.ToString(), socketClientConn);
                Console.WriteLine("All current connections: " + string.Join(",", socketClientConns.Keys));
                var thrReceive = new Thread(Receive);
                thrReceive.IsBackground = true;
                thrReceive.Start(socketClientConn);
                threadClients.Add(socketClientConn.RemoteEndPoint.ToString(), thrReceive);
            }
        }

        /// <summary>
        /// 一个Socket连接一个线程接收数据
        /// </summary>
        /// <param name="obj"></param>
        private void Receive(object obj)
        {
            Socket socket = obj as Socket;
            while (true)
            {
                // 第四步：准备2M缓冲数组接收信息
                byte[] byteDataArr = new byte[1024 * 1024 * 2];
                int length = -1;
                try
                {
                    length = socket.Receive(byteDataArr);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(socket.RemoteEndPoint.ToString() + " disconnect: " + ex.ToString());
                    // 移出已关闭的连接
                    socketClientConns.Remove(socket.RemoteEndPoint.ToString());
                    // 移出已关闭连接的线程
                    threadClients.Remove(socket.RemoteEndPoint.ToString());
                    socket.Close();
                    break; // 该连接已关闭，退出线程
                }
                // 数据处理
                // 将字节数据接收为字符串
                string strMsg = System.Text.Encoding.UTF8.GetString(byteDataArr, 0, length);
                if (string.IsNullOrWhiteSpace(strMsg)) continue;
                Console.WriteLine($"Receive from Client {socket.RemoteEndPoint.ToString()} on {DateTime.Now}: \r\n" + strMsg.Trim().Replace("\r\n", ""));
            }
        }
    }
}

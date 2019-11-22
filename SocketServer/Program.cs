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
            Console.WriteLine("SocketServer.exe is Run");
            var pro = new Program();
            pro.Server_Listen();
        }
        // 服务端用于socket.Accept()
        Thread thrAccept;
        // 服务端用于监听
        Socket socketListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // 与客户端的连接
        Dictionary<string, Socket> socketClientConns { get; } = new Dictionary<string, Socket>();
        // 连接所在的线程
        Dictionary<string, Thread> threadClients { get; } = new Dictionary<string, Thread>();

        private void Server_Listen()
        {
            //第一步：绑定端口
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, 10001);
            socketListen.Bind(endPoint);
            socketListen.Listen(10);

            //第二步：开启线程，进行监听
            thrAccept = new Thread(Watching);
            thrAccept.IsBackground = true;
            thrAccept.Start();
            Console.WriteLine("Server started Listen on 127.0.0.1:10001");
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
                        comunicate.Value.Send(arrByte);
                    }
                }
            }
        }

        private void Watching(object obj)
        {
            while (true)
            {
                //第三步：监听到请求后返回一个通信套接字，开启线程以接收信息
                var socketClientConn = socketListen.Accept();
                Console.WriteLine("监听到了一个连接请求！");
                socketClientConns.Add(socketClientConn.RemoteEndPoint.ToString(), socketClientConn);
                Console.WriteLine("当前所有的连接：" + string.Join(",", socketClientConns.Keys));
                var thrReceive = new Thread(Receive);
                thrReceive.IsBackground = true;
                thrReceive.Start(socketClientConn);
                threadClients.Add(socketClientConn.RemoteEndPoint.ToString(), thrReceive);
                Console.WriteLine("开始接收通信信息");
            }
        }

        private void Receive(object socketClientConn)
        {
            Socket socket = socketClientConn as Socket;
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
                    Console.WriteLine("客户端下线了！"+ex.Message);
                    // 移出已关闭的连接
                    socketClientConns.Remove(socket.RemoteEndPoint.ToString());
                    // 移出已关闭连接的线程
                    threadClients.Remove(socket.RemoteEndPoint.ToString());
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

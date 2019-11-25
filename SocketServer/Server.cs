using SocketCommons;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketCommons
{
    // TCP Socket http://www.jytek.com/seesharpsocket
    // UDP Socket https://blog.csdn.net/i1tws/article/details/86624951
    // TCP 与 Socket 的关系 https://www.cnblogs.com/xuan52rock/p/9454696.html
    // TODO 通信框架 协议层为什么不用HTTP协议？
    // TODO 通信框架 控制层应该与协议层分离
    public class Server
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server started!");
            try
            {
                var server = new Server();
                server.Start();
                while (true)
                {
                    Console.WriteLine("Receive to client> ");
                    var str = Console.ReadLine();
                    server.Send(str, null);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Fatal error occurred: " + ex.ToString());
            }
            Console.ReadKey();
        }

        // 服务端用于socket.Accept()
        /// <summary>
        /// 与客户端的连接 多线程公共资源
        /// </summary>
        Dictionary<string, Socket> socketClientConns { get; } = new Dictionary<string, Socket>();
        /// <summary>
        /// 连接所在的线程 多线程公共资源
        /// </summary>
        Dictionary<string, Thread> threadClients { get; } = new Dictionary<string, Thread>();
        /// <summary>
        /// 队列 多线程公共资源
        /// </summary>
        MessageQueue<string> MessageQueue { get; } = new MessageQueue<string>();

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            // 第一步：绑定端口
            // 服务端用于监听
            Socket socketListen = CreateServerSocket();

            // 第二步：开启线程，进行监听
            Thread thrAccept = new Thread(Watching);
            thrAccept.IsBackground = true;
            thrAccept.Start(socketListen);
            Console.WriteLine("Server started Listen on "+ socketListen.LocalEndPoint.ToString());
            // 死循环发送数据到客户端
            while (true)
            {
                var str = MessageQueue.WaitOne();
                // 第五步：发送信息
                // TODO 构建请求或响应管道，包含 请求，解密->反序列化->...处理...->序列化->加密，响应
                // TODO 一次请求的数据过大，构建流读取？JSON是否支持流读取
                // 使用序列化器 转换字节数据和对象
                var content = $"{str} - by Server on {DateTime.Now.ToString()}";
                var dataBytes = SerializationHelper.Serialize(new ProtocolData
                {
                    Headers = new List<Attribute>
                    {
                        new Attribute
                        {
                            Name = "ServerTime",
                            Value = DateTime.UtcNow.ToString()
                        }
                    },
                    Content = EncodingHelper.Encoding(content)
                });
                // TODO 这里是群发，改为通过协议转发
                foreach (var comunicate in socketClientConns)
                {
                    if (comunicate.Value != null && comunicate.Value.Connected)
                    {
                        comunicate.Value.Send(dataBytes);
                    }
                }
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="content">正文</param>
        /// <param name="endPoints">连接列表</param>
        public void Send(string content, List<string> endPoints)
        {
            // 向队列中中添加一个消息
            MessageQueue.Append(content);
        }

        private Socket CreateServerSocket(int port = 1014, int connCount = 10)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port)); // IPAddress.Parse("127.0.0.1")
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
                // 第四步：准备2MB缓冲数组接收信息
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
                // 接收数据处理
                if(length > 0)
                {
                    // 将字节数据接收为字符串
                    var data = SerializationHelper.Deserialize<ProtocolData>(byteDataArr, 0, length);
                    string strMsg = System.Text.Encoding.UTF8.GetString(data.Content, 0, data.Content.Length);
                    if (string.IsNullOrWhiteSpace(strMsg)) continue;
                    Console.WriteLine($"Receive from Client {socket.RemoteEndPoint.ToString()} on {DateTime.Now}: \r\n" + strMsg.Trim().Replace("\r\n", ""));
                }
            }
        }
    }
}

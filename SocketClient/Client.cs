using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SocketCommons;

namespace SocketClient
{
    // TCP Socket http://www.jytek.com/seesharpsocket
    // UDP Socket https://blog.csdn.net/i1tws/article/details/86624951
    public class Client
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client started!");
            try
            {
                var remoteIp = "127.0.0.1";
                var port = "1014";
                new Client().Start($"{remoteIp}:{port}");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Fatal error occurred: " + ex.ToString());
            }
            Console.ReadKey();
        }

        //客户端要准备一个Socket负责通信，一个线程用于socket.Receive();
        Socket socketClient;
        Thread thrRec;
        EndPoint remoteEndPoint;

        public void Start(string endPoint)
        {
            remoteEndPoint = IPEndPoint.Parse(endPoint);

            // 第一步：创建socket，并请求连接服务器
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketClient.Connect(remoteEndPoint);

            Console.WriteLine("Server connection successful: " + socketClient.RemoteEndPoint.ToString());

            // 第二步：连接服务器成功后，开启线程，接收服务器信息。
            thrRec = new Thread(Receive);
            thrRec.IsBackground = true;
            thrRec.Start(socketClient);

            while (true)
            {
                Console.WriteLine("Send to server> ");
                var str = Console.ReadLine();
                Console.WriteLine($"Server Connected: {socketClient?.Connected ?? false}");
                // 插入序列化器
                var content = $"{str} - on {DateTime.Now.ToString()}";
                var dataBytes = ProtocolDataHelper.Packing(content);
                try
                {
                    if (socketClient != null && socketClient.Connected)
                    {
                        // 发送请求报文
                        socketClient.Send(dataBytes);
                        //socket.SendFile(path);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Data sending failed: " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="socketClientObj">Socket对象</param>
        void Receive(object socketClientObj)
        {
            var socket = socketClientObj as Socket;
            while (true)
            {
                //第三步：将从服务器处接收到的信息储存到缓冲数组中，并读出
                byte[] byteDataArr = new byte[1024 * 1024 * 2];
                int length = -1;
                try
                {
                    // 重连
                    if (socketClient == null || !socketClient.Connected)
                    {
                        try
                        {
                            Console.WriteLine("Connection dropped!");
                            if(socketClient != null)
                            {
                                socketClient.Close();
                                socketClient = null;
                            }
                            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            // TODO 重连间隔时间应该呈Fibonacci数列 1 1 2 3 5 8 13 的方式递增，重连次数为10次，然后等待手动重连
                            Thread.Sleep(1000*3);
                            Console.WriteLine("Try to reconnect");
                            socketClient.Connect(remoteEndPoint);
                            Console.WriteLine("Reconnect successfully: "+ socketClient.RemoteEndPoint.ToString());
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("Reconnection failure: " + ex.ToString());
                            socketClient.Close();
                            socketClient = null;
                            continue;  // 重试失败，再次循环重试
                        }
                    }
                    else
                    {
                        length = socketClient.Receive(byteDataArr);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("服务器下线了！" + ex.ToString());
                }
                if(length > 0)
                {
                    var data = ProtocolDataHelper.Unpacking(byteDataArr, 0, length);
                    string strMsg = EncodingHelper.Decoding(data.Content, 0, data.Content.Length);
                    if (string.IsNullOrWhiteSpace(strMsg)) continue;
                    Console.WriteLine($"Receive from Server {socketClient.RemoteEndPoint.ToString()} on {DateTime.Now}: \r\n" + strMsg);
                    Console.WriteLine("Send to server> ");
                }
            }
        }
    }
}

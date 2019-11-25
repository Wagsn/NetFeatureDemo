using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketClient
{
    // TCP Socket http://www.jytek.com/seesharpsocket
    // UDP Socket https://blog.csdn.net/i1tws/article/details/86624951
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SocketClient run");
            try
            {
                var pro = new Program();
                pro.Request();
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

        private void Request()
        {
            //第一步：创建socket，并请求连接服务器
            var remoteIp = "127.0.0.1";
            var port = "1014";
            remoteEndPoint = IPEndPoint.Parse($"{remoteIp}:{port}");

            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketClient.Connect(remoteEndPoint);

            byte[] buffer = Encoding.UTF8.GetBytes($"Client {socketClient.LocalEndPoint.ToString()} start");
            // 发送请求报文
            socketClient.Send(buffer); 

            Console.WriteLine("连接服务器成功！！！");

            //第二步：连接服务器成功后，开启线程，接收服务器信息。
            thrRec = new Thread(Receive);
            thrRec.IsBackground = true;
            thrRec.Start(socketClient);

            while (true)
            {
                Console.Write("Send to server> ");
                var str = Console.ReadLine();
                Console.WriteLine($"Server Connected: {socketClient?.Connected ?? false}");
                byte[] arrByte = System.Text.Encoding.UTF8.GetBytes($"{str} - on {DateTime.Now.ToString()}");
                if (socketClient != null && socketClient.Connected)
                {
                    socketClient.Send(arrByte);
                    //socket.SendFile(path);
                }
            }
        }

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
                    if (!socketClient.Connected)
                    {
                        try
                        {
                            Console.WriteLine("Connection dropped!");
                            socketClient.Close();
                            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            // TODO 重连间隔时间应该呈Fibonacci数列 1 1 2 3 5 8 13 的方式递增，重连次数为10次，然后等待手动重连
                            Thread.Sleep(1000*3);
                            Console.WriteLine("Try to reconnect");
                            socketClient.Connect(remoteEndPoint);
                            Console.WriteLine("Reconnect successfully: "+ socketClient.RemoteEndPoint.ToString());
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("Reconnection failure: "+ex.ToString());
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
                    // TODO 重连 连接外部控制
                    Console.WriteLine("服务器下线了！"+ex.ToString());
                }
                if(length >= 0)
                {
                    string strMsg = System.Text.Encoding.UTF8.GetString(byteDataArr, 0, length);
                    if (string.IsNullOrWhiteSpace(strMsg)) continue;
                    Console.WriteLine($"Receive from Server {socketClient.RemoteEndPoint.ToString()} on {DateTime.Now}: \r\n" + strMsg);
                    Console.WriteLine("Send to server> ");
                }
            }
        }
    }
}

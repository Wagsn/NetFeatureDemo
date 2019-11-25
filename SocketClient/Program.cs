﻿using System;
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
            var pro = new Program();
            pro.Request();
            Console.ReadKey();
        }

        //客户端要准备一个Socket负责通信，一个线程用于socket.Receive();
        Socket socketClient;
        Thread thrRec;

        private void Request()
        {
            //第一步：创建socket，并请求连接服务器
            var remoteIp = "127.0.0.1";
            var port = "1014";
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(remoteIp), int.Parse(port));
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketClient.Connect(endPoint);

            byte[] buffer = Encoding.UTF8.GetBytes($"Client {socketClient.LocalEndPoint.ToString()} start");
            // 发送请求报文
            socketClient.Send(buffer); 

            Console.WriteLine("连接服务器成功！！！");

            //第二步：连接服务器成功后，开启线程，接收服务器信息。
            thrRec = new Thread(Receive);
            thrRec.IsBackground = true;
            thrRec.Start();

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
            try
            {
                while (true)
                {
                    //第三步：将从服务器处接收到的信息储存到缓冲数组中，并读出
                    byte[] byteDataArr = new byte[1024 * 1024 * 2];
                    int msgLength = socketClient.Receive(byteDataArr);
                    string strMsg = System.Text.Encoding.UTF8.GetString(byteDataArr, 0, msgLength);
                    if (string.IsNullOrWhiteSpace(strMsg)) continue;
                    Console.WriteLine($"Receive from Server {socketClient.RemoteEndPoint.ToString()} on {DateTime.Now}: \r\n" +strMsg);
                    Console.WriteLine("Send to server> ");
                }
            }
            catch (Exception)
            {
                // TODO 重连 连接外部控制
                Console.WriteLine("服务器下线了！");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SocketCommons
{
    /// <summary>
    /// 消息队列
    /// </summary>
    public class MessageQueue<T>
    {
        private Queue<T> Queue { get; } = new Queue<T>();

        /// <summary>
        /// 在队列中追加
        /// </summary>
        /// <param name="item"></param>
        public void Append(T item)
        {
            Queue.Enqueue(item);
        }

        /// <summary>
        /// 阻塞线程的方式等待下一个消息
        /// </summary>
        /// <returns></returns>
        public T WaitOne()
        {
            while (true)
            {
                if (Queue.Count > 0)
                {
                    return Queue.Dequeue();
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}

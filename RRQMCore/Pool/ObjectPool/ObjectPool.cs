//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace RRQMCore.Pool
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> : IObjectPool where T : IPoolObject
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capacity"></param>
        public ObjectPool(int capacity)
        {
            this.Capacity = capacity;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ObjectPool()
        {
        }

        private ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

        /// <summary>
        /// 对象池容量
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// 可使用（创建）数量
        /// </summary>
        public int FreeSize { get { return this.freeSize; } }

        private int freeSize;

        /// <summary>
        /// 清除池中所有对象
        /// </summary>
        public void Clear()
        {
            while (this.queue.TryDequeue(out _))
            {
            }
        }

        /// <summary>
        /// 获取对象T
        /// </summary>
        /// <returns></returns>
        public T GetObject()
        {
            T t;
            if (this.queue.TryDequeue(out t))
            {
                t.Recreate();
                t.NewCreate = false;
                Interlocked.Decrement(ref this.freeSize);
                return t;
            }

            t = (T)Activator.CreateInstance(typeof(T));
            t.Create();
            t.NewCreate = true;
            return t;
        }
        
        
        /// <summary>
        /// 预获取
        /// </summary>
        /// <returns></returns>
        public T PreviewGetObject()
        {
            T t;
            this.queue.TryPeek(out t);
            return t;
        }

        /// <summary>
        /// 注销对象
        /// </summary>
        /// <param name="t"></param>
        public void DestroyObject(T t)
        {
            t.Destroy();
            if (this.freeSize < this.Capacity)
            {
                Interlocked.Increment(ref this.freeSize);
                this.queue.Enqueue(t);
            }
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            this.Clear();
        }
    }
}
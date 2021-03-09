using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMCore.Pool
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : IPoolObject
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
        /// 获取对象T
        /// </summary>
        /// <returns></returns>
        public T GetObject()
        {
            T t;
            if (this.queue.TryDequeue(out t))
            {
                t.Recreate();
                t.NewCreat = false;
                Interlocked.Decrement(ref this.freeSize);
                return t;
            }

            t = (T)Activator.CreateInstance(typeof(T));
            t.Create();
            t.NewCreat = true;
            return t;
        }

        /// <summary>
        /// 注销对象
        /// </summary>
        /// <param name="t"></param>
        public void DestroyObject(T t)
        {
            t.Destroy();
            if (this.freeSize<this.Capacity)
            {
                Interlocked.Increment(ref this.freeSize);
                this.queue.Enqueue(t);
            }
            
        }
    }
}

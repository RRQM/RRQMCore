using System;
using System.Threading;

namespace RRQMCore.Run
{
    /// <summary>
    /// 等待数据对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WaitData<T> : IDisposable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WaitData()
        {
            this.waitHandle = new AutoResetEvent(false);
        }

        private EventWaitHandle waitHandle;

        /// <summary>
        /// 等待数据结果
        /// </summary>
        public T WaitResult { get; set; }

        /// <summary>
        /// 等待指定毫秒
        /// </summary>
        /// <param name="millisecond"></param>
        public void Wait(int millisecond)
        {
            this.waitHandle.WaitOne(millisecond);
        }

        /// <summary>
        /// 使等待的线程继续执行
        /// </summary>
        public void Set()
        {
            this.waitHandle.Set();
        }

        /// <summary>
        /// 使等待的线程继续执行
        /// </summary>
        /// <param name="waitResult">等待结果</param>
        public void Set(T waitResult)
        {
            this.WaitResult = waitResult;
            this.waitHandle.Set();
        }

        internal bool dispose;

        /// <summary>
        /// 回收
        /// </summary>
        public void Dispose()
        {
            this.WaitResult = default(T);
            this.dispose = true;
        }
    }
}
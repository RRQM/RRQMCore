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
            this.dispose = true;
        }
    }
}
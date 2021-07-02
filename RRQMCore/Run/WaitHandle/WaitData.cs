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

        private T waitResult;

        /// <summary>
        /// 等待数据结果
        /// </summary>
        public T WaitResult
        {
            get { return waitResult; }
        }

        /// <summary>
        /// 载入结果
        /// </summary>
        public void LoadResult(T result)
        {
            this.waitResult = result;
        }

        /// <summary>
        /// 等待指定毫秒
        /// </summary>
        /// <param name="millisecond"></param>
        public bool Wait(int millisecond)
        {
            return this.waitHandle.WaitOne(millisecond);
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
            this.waitResult = waitResult;
            this.waitHandle.Set();
        }

        internal bool @using;

        /// <summary>
        /// 使用中
        /// </summary>
        public bool Using
        {
            get { return @using; }
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void Dispose()
        {
            this.@using = false;
            this.waitResult = default;
        }

        /// <summary>
        /// 完全释放
        /// </summary>
        public void DisposeAbsolute()
        {
            this.@using = false;
            this.waitResult = default;
            this.waitHandle.Dispose();
        }
    }
}
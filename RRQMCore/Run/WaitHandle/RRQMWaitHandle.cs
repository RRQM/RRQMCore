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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace RRQMCore.Run
{
    /// <summary>
    /// 等待处理数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RRQMWaitHandle<T> where T : WaitResult, new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RRQMWaitHandle()
        {
            waitDic = new ConcurrentDictionary<int, WaitData<T>>();
        }

        private ConcurrentDictionary<int, WaitData<T>> waitDic;

        private int sign;

        /// <summary>
        /// 获取一个可等待对象
        /// </summary>
        public WaitData<T> GetWaitData()
        {
            WaitData<T> waitData;
            foreach (int item in waitDic.Keys)
            {
                waitData = waitDic[item];
                lock (this)
                {
                    if (waitData.dispose)
                    {
                        this.waitDic.TryRemove(item, out _);
                        waitData.WaitResult.Sign = Interlocked.Increment(ref sign);
                        waitData.dispose = false;
                        this.waitDic.TryAdd(waitData.WaitResult.Sign, waitData);
                        return waitData;
                    }
                }
            }

            waitData = new WaitData<T>();
            waitData.WaitResult = new T();
            waitData.WaitResult.Sign = Interlocked.Increment(ref sign);
            this.waitDic.TryAdd(waitData.WaitResult.Sign, waitData);
            return waitData;

        }

        /// <summary>
        /// 让等待对象恢复运行
        /// </summary>
        /// <param name="sign"></param>
        public void SetRun(int sign)
        {
            WaitData<T> waitData;
            if (this.waitDic.TryGetValue(sign, out waitData))
            {
                waitData.WaitResult.Sign = sign;
                waitData.Set();
            }
        }

        /// <summary>
        /// 让等待对象恢复运行
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="waitResult"></param>
        public void SetRun(int sign, T waitResult)
        {
            WaitData<T> waitData;
            if (this.waitDic.TryGetValue(sign, out waitData))
            {
                waitData.WaitResult.Sign = sign;
                waitData.Set(waitResult);
            }
        }

        /// <summary>
        /// 让等待对象恢复运行
        /// </summary>
        /// <param name="waitResult"></param>
        public void SetRun(T waitResult)
        {
            WaitData<T> waitData;
            if (this.waitDic.TryGetValue(sign, out waitData))
            {
                waitData.WaitResult.Sign = sign;
                waitData.Set(waitResult);
            }
        }
    }
}
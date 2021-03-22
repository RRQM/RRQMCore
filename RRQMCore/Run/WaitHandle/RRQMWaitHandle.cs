//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System.Collections.Generic;

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
            waitDic = new Dictionary<int, WaitData<T>>();
        }

        private Dictionary<int, WaitData<T>> waitDic;

        private int sign;

        /// <summary>
        /// 获取一个可等待对象
        /// </summary>
        public WaitData<T> GetWaitData()
        {
            lock (this)
            {
                foreach (WaitData<T> item in waitDic.Values)
                {
                    if (item.dispose)
                    {
                        item.dispose = false;
                        return item;
                    }
                }
                WaitData<T> waitData = new WaitData<T>();
                waitData.WaitResult = new T();
                waitData.WaitResult.Sign = sign++;
                this.waitDic.Add(waitData.WaitResult.Sign, waitData);
                return waitData;
            }
        }

        /// <summary>
        /// 让等待对象恢复运行
        /// </summary>
        /// <param name="sign"></param>
        public void SetRun(int sign)
        {
            if (this.waitDic.ContainsKey(sign))
            {
                WaitData<T> waitData;
                lock (this)
                {
                    waitData = this.waitDic[sign];
                }
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
            if (this.waitDic.ContainsKey(sign))
            {
                WaitData<T> waitData;
                lock (this)
                {
                    waitData = this.waitDic[sign];
                }
                waitData.Set(waitResult);
            }
        }

        /// <summary>
        /// 让等待对象恢复运行
        /// </summary>
        /// <param name="waitResult"></param>
        public void SetRun(T waitResult)
        {
            if (this.waitDic.ContainsKey(waitResult.Sign))
            {
                WaitData<T> waitData;
                lock (this)
                {
                    waitData = this.waitDic[waitResult.Sign];
                }
                waitData.Set(waitResult);
            }
        }
    }
}
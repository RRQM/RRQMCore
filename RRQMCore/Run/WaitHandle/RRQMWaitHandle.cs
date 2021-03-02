using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Run
{
    /// <summary>
    /// 等待处理数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RRQMWaitHandle<T> where T : WaitResult,new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RRQMWaitHandle()
        {
            waitDic = new Dictionary<int, WaitData<T>>();
        }
        private Dictionary<int, WaitData<T>> waitDic;

        int sign;
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
        public void SetRun(int sign,T waitResult)
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

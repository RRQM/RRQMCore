using System;
using System.Diagnostics;

namespace RRQMCore.Diagnostics
{
    /// <summary>
    /// 时间测量器
    /// </summary>
    public class TimeMeasurer
    {
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TimeSpan Run(Action action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action?.Invoke();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
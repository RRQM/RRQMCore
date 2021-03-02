using System;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMCore.Run
{
    /// <summary>
    /// 时间执行
    /// </summary>
    public class TimeRun
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="timeSpan"></param>
        public static void Run(Action action, TimeSpan timeSpan)
        {
            Task.Run(() =>
            {
                WaitHandle waitHandle = new AutoResetEvent(false);
                waitHandle.WaitOne(timeSpan);
                action?.Invoke();
            });
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
        public static void Run(Action action, double seconds)
        {
            Task.Run(() =>
            {
                WaitHandle waitHandle = new AutoResetEvent(false);
                waitHandle.WaitOne(TimeSpan.FromSeconds(seconds));
                action?.Invoke();
            });
        }
    }
}
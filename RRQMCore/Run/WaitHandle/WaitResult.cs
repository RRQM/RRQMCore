using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Run
{
    /// <summary>
    /// 等待返回类
    /// </summary>
    [Serializable]
    public class WaitResult
    {
        /// <summary>
        /// 标记
        /// </summary>
        public int Sign { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Exceptions
{
    /// <summary>
    /// 若汝棋茗程序集异常类基类
    /// </summary>
    [Serializable]
    public class RRQMException : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RRQMException() : base() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public RRQMException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public RRQMException(string message, System.Exception inner) : base(message, inner) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected RRQMException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RRQMCore.Exceptions;

namespace RRQMCore.Exceptions
{
    /*
    若汝棋茗
    */
    /// <summary>
    /// 未找到消息异常类
    /// </summary>
    [Serializable]
    public class MessageNotFoundException : RRQMException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mes"></param>
        public MessageNotFoundException(string mes) : base(mes)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Exceptions
{
    /*
    若汝棋茗
    */
    /// <summary>
    /// 消息已注册
    /// </summary>
    [Serializable]
   public class MessageRegisteredException:RRQMException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mes"></param>
        public MessageRegisteredException(string mes):base(mes)
        { 
        
        }
    }
}

using System;

namespace RRQMCore.Exceptions
{
    /*
    若汝棋茗
    */

    /// <summary>
    /// 消息已注册
    /// </summary>
    [Serializable]
    public class MessageRegisteredException : RRQMException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mes"></param>
        public MessageRegisteredException(string mes) : base(mes)
        {
        }
    }
}
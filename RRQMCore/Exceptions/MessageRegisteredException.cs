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

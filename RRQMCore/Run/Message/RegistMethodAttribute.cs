using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Run
{
    /*
    若汝棋茗
    */
    /// <summary>
    /// 注册为消息
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class RegistMethodAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RegistMethodAttribute()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="token"></param>
        public RegistMethodAttribute(string token)
        {
            this.Token = token;
        }
        /// <summary>
        /// 标识
        /// </summary>
        public string Token { get; set; }
    }
}

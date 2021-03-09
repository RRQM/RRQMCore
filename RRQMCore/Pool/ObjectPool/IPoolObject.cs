using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Pool
{
    /// <summary>
    /// 对象池单位接口
    /// </summary>
    public interface IPoolObject
    {
        /// <summary>
        /// 是否为新建对象
        /// </summary>
        bool NewCreat { get; set; }

        /// <summary>
        /// 初创建对象
        /// </summary>
        void Create();

        /// <summary>
        /// 重新创建对象
        /// </summary>
        void Recreate();

        /// <summary>
        /// 销毁对象
        /// </summary>
        void Destroy();


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Pool
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IObjectPool:IDisposable
    {
        /// <summary>
        /// 可使用数量
        /// </summary>
        int FreeSize { get; }

        /// <summary>
        /// 清空池中对象
        /// </summary>
        void Clear();
    }
}

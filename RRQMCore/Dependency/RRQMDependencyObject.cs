//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System.Collections.Concurrent;

namespace RRQMCore.Dependency
{
    /// <summary>
    /// 依赖项对象
    /// </summary>
    public class RRQMDependencyObject
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private ConcurrentDictionary<DependencyProperty, object> dp = new ConcurrentDictionary<DependencyProperty, object>();

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="dependencyProperty"></param>
        /// <returns></returns>
        public object GetValue(DependencyProperty dependencyProperty)
        {
            if (dp.TryGetValue(dependencyProperty, out object value))
            {
                return value;
            }
            else
            {
                return dependencyProperty.DefauleValue;
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="dependencyProperty"></param>
        /// <param name="value"></param>
        public RRQMDependencyObject SetValue(DependencyProperty dependencyProperty, object value)
        {
            dependencyProperty.SetValue(value);
            dp.AddOrUpdate(dependencyProperty, value, (DependencyProperty dp, object v) =>
            {
                return value;
            });
            return this;
        }
    }
}
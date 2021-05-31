using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Dependency
{
    /// <summary>
    /// 依赖项对象
    /// </summary>
   public class RRQMDependencyObject
    {
        [System.Diagnostics.DebuggerBrowsable( System.Diagnostics.DebuggerBrowsableState.Never)]
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

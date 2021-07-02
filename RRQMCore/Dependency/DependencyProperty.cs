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
using RRQMCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Dependency
{
    /// <summary>
    /// 依赖项属性
    /// </summary>
    public class DependencyProperty
    {
        private DependencyProperty()
        {

        }
        private string name;

        /// <summary>
        /// 属性名
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        private Type owner;
        /// <summary>
        /// 所属类型
        /// </summary>
        public Type Owner
        {
            get { return owner; }
        }

        private Type valueType;
        /// <summary>
        /// 值类型
        /// </summary>
        public Type ValueType
        {
            get { return valueType; }
        }

        private object value;
        /// <summary>
        /// 默认值
        /// </summary>
        public object DefauleValue
        {
            get { return value; }
        }

        internal void SetValue(object value)
        {
            if (value == null)
            {
                if (typeof(ValueType).IsAssignableFrom(valueType))
                {
                    throw new RRQMException($"属性“{this.name}”赋值类型与注册类型不一致，应当注入“{valueType}”类型");
                }
                this.value = value;
            }
            else if (valueType.IsAssignableFrom(value.GetType()))
            {
                this.value = value;
            }
            else
            {
                throw new RRQMException($"属性“{this.name}”赋值类型与注册类型不一致，应当注入“{valueType}”类型");
            }
        }

        /// <summary>
        /// 注册依赖项属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="valueType"></param>
        /// <param name="owner"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DependencyProperty Register(string propertyName, Type valueType, Type owner, object value)
        {
            DependencyProperty dp = new DependencyProperty();
            dp.name = propertyName;
            dp.valueType = valueType;
            dp.owner = owner;
            dp.SetValue(value);
            return dp;
        }
    }
}

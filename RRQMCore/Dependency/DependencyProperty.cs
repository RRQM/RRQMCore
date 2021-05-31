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
                    throw new RRQMException($"属性“{this.name}”赋值类型与注册类型不一致");
                }
                this.value = value;
            }
            else if (valueType.IsAssignableFrom(value.GetType()))
            {
                this.value = value;
            }
            else
            {
                throw new RRQMException($"属性“{this.name}”赋值类型与注册类型不一致");
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

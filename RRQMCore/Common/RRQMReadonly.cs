using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore
{
    /// <summary>
    /// 常量
    /// </summary>
    public class RRQMReadonly
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static readonly Type stringType = typeof(string);
        public static readonly Type byteType = typeof(byte);
        public static readonly Type shortType = typeof(short);
        public static readonly Type intType = typeof(int);
        public static readonly Type boolType = typeof(bool);
        public static readonly Type longType = typeof(long);
        public static readonly Type floatType = typeof(float);
        public static readonly Type doubleType = typeof(double);
        public static readonly Type decimalType = typeof(decimal);
        public static readonly Type dateTimeType = typeof(DateTime);
        public static readonly Type bytesType = typeof(byte[]);
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }
}

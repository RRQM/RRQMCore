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

namespace RRQMCore.Data.Converter
{
    /// <summary>
    /// 值转换器
    /// </summary>
    public static class ValueConvert
    {
        /// <summary>
        /// int型转换器
        /// </summary>
        /// <param name="valueString">数字字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回值结果</returns>
        public static int IntConvert(string valueString, int defaultValue = 0)
        {
            int result;
            if (int.TryParse(valueString, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// int型转换器
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public static int IntConvert(object valueObject)
        {
            return (int)valueObject;
        }

        /// <summary>
        /// long值转换器
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public static long LongConvert(object valueObject)
        {
            return (long)valueObject;
        }

        /// <summary>
        /// 双精度值转换
        /// </summary>
        /// <param name="valueString">数字字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回值结果</returns>
        public static double DoubleConvert(string valueString, double defaultValue = 0)
        {
            double result;
            if (double.TryParse(valueString, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 枚举类型转换
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="valueString">枚举字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回值结果</returns>
        public static T EnumConvert<T>(string valueString, T defaultValue = default(T)) where T : struct
        {
            T result;
            if (Enum.TryParse(valueString, out result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}

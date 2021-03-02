using System;

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
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Serialization
{
    /// <summary>
    /// 该序列化以二进制方式进行，但是不支持接口、抽象类、继承类等成员的序列化。
    /// </summary>
    public class RRQMBinaryFormatter
    {
        private static readonly Type stringType = typeof(string);
        private static readonly Type byteType = typeof(byte);
        private static readonly Type shortType = typeof(short);
        private static readonly Type intType = typeof(int);
        private static readonly Type boolType = typeof(bool);
        private static readonly Type longType = typeof(long);
        private static readonly Type floatType = typeof(float);
        private static readonly Type doubleType = typeof(double);
        private static readonly Type decimalType = typeof(decimal);
        private static readonly Type dateTimeType = typeof(DateTime);
        private static readonly Type bytesType = typeof(byte[]);
        #region Serialize
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="graph">对象</param>
        public void Serialize(Stream stream, object graph)
        {
            SerializeObject(stream, graph);
        }

        private int SerializeObject(Stream stream, object graph)
        {
            int len = 0;
            byte[] data = null;

            long startPosition = stream.Position;
            long endPosition;
            if (graph != null)
            {
                if (graph is string str)
                {
                    data = Encoding.UTF8.GetBytes(str);
                }
                else if (graph is byte by)
                {
                    data = new byte[] { by };
                }
                else if (graph is bool b)
                {
                    data = BitConverter.GetBytes(b);
                }
                else if (graph is short s)
                {
                    data = BitConverter.GetBytes(s);
                }
                else if (graph is int)
                {
                    data = BitConverter.GetBytes((int)graph);
                }
                else if (graph is long l)
                {
                    data = BitConverter.GetBytes(l);
                }
                else if (graph is float f)
                {
                    data = BitConverter.GetBytes(f);
                }
                else if (graph is double d)
                {
                    data = BitConverter.GetBytes(d);
                }
                else if (graph is DateTime time)
                {
                    data = Encoding.UTF8.GetBytes(time.Ticks.ToString());
                }
                else if (graph is Enum)
                {
                    var enumValType = Enum.GetUnderlyingType(graph.GetType());

                    if (enumValType == byteType)
                    {
                        data = new byte[] { (byte)graph };
                    }
                    else if (enumValType == shortType)
                    {
                        data = BitConverter.GetBytes((short)graph);
                    }
                    else if (enumValType == intType)
                    {
                        data = BitConverter.GetBytes((int)graph);
                    }
                    else
                    {
                        data = BitConverter.GetBytes((long)graph);
                    }
                }
                else if (graph is byte[])
                {
                    data = (byte[])graph;
                }
                else
                {
                    stream.Position += 4;
                    Type type = graph.GetType();

                    if (typeof(IEnumerable).IsAssignableFrom(type))
                    {
                        len += SerializeIEnumerable(stream, (IEnumerable)graph);
                    }
                    else
                    {
                        len += SerializeClass(stream, graph, type);
                    }
                }

                if (data != null)
                {
                    len = data.Length;
                    endPosition = len + startPosition + 4;
                }
                else
                {
                    endPosition = stream.Position;
                }
            }
            else
            {
                endPosition = startPosition + 4;
            }

            byte[] lenBuffer = BitConverter.GetBytes(len);
            stream.Position = startPosition;
            stream.Write(lenBuffer, 0, lenBuffer.Length);

            if (data != null)
            {
                stream.Write(data, 0, data.Length);
            }
            stream.Position = endPosition;
            return len + 4;
        }

        private int SerializeClass(Stream stream, object obj, Type type)
        {
            int len = 0;
            if (obj != null)
            {
                PropertyInfo[] propertyInfos = this.GetProperties(type);
                foreach (PropertyInfo property in propertyInfos)
                {
                    len += SerializeObject(stream, property.GetValue(obj, null));
                }
            }
            return len;
        }

        private int SerializeIEnumerable(Stream stream, IEnumerable param)
        {
            int len = 0;
            if (param != null)
            {
                foreach (object item in param)
                {
                    len += SerializeObject(stream, item);
                }
            }
            return len;
        }

        #endregion

        #region Deserialize


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Deserialize(byte[] data, int offset, Type type)
        {
            return Deserialize(type, data, ref offset);
        }
        private object Deserialize(Type type, byte[] datas, ref int offset)
        {
            dynamic obj = null;
            int len = BitConverter.ToInt32(datas, offset);
            offset += 4;
            if (len > 0)
            {
                if (type == stringType)
                {
                    obj = Encoding.UTF8.GetString(datas, offset, len);
                }
                else if (type == byteType)
                {
                    obj = datas[offset];
                }
                else if (type == boolType)
                {
                    obj = (BitConverter.ToBoolean(datas, offset));
                }
                else if (type == shortType)
                {
                    obj = (BitConverter.ToInt16(datas, offset));
                }
                else if (type == intType)
                {
                    obj = (BitConverter.ToInt32(datas, offset));
                }
                else if (type == longType)
                {
                    obj = (BitConverter.ToInt64(datas, offset));
                }
                else if (type == floatType)
                {
                    obj = (BitConverter.ToSingle(datas, offset));
                }
                else if (type == doubleType)
                {
                    obj = (BitConverter.ToDouble(datas, offset));
                }
                else if (type == decimalType)
                {
                    obj = (BitConverter.ToDouble(datas, offset));
                }
                else if (type == dateTimeType)
                {
                    obj = (new DateTime(long.Parse(Encoding.UTF8.GetString(datas, offset, len))));
                }
                else if (type.BaseType == typeof(Enum))
                {
                    Type enumType = Enum.GetUnderlyingType(type);

                    if (enumType == typeof(byte))
                    {
                        obj = Enum.ToObject(type, datas[offset]);
                    }
                    else if (enumType == typeof(short))
                    {
                        obj = Enum.ToObject(type, BitConverter.ToInt16(datas, offset));
                    }
                    else if (enumType == typeof(int))
                    {
                        obj = Enum.ToObject(type, BitConverter.ToInt32(datas, offset));
                    }
                    else
                    {
                        obj = Enum.ToObject(type, BitConverter.ToInt64(datas, offset));
                    }
                }
                else if (type == bytesType)
                {
                    byte[] data = new byte[len];
                    Buffer.BlockCopy(datas, offset, data, 0, len);
                    obj = data;
                }
                else if (type.IsClass)
                {
                    obj = DeserializeClass(type, datas, offset, len);
                }
                else
                {
                    throw new Exception("未定义的类型：" + type.ToString());
                }

            }
            offset += len;
            return obj;
        }

        private object DeserializeClass(Type type, byte[] datas, int offset, int length)
        {
            InstanceObject instanceObject = GetOrAddInstance(type);
            switch (instanceObject.instanceType)
            {
                case InstanceType.Class:
                    {
                        foreach (var item in instanceObject.Properties)
                        {
                            object obj = Deserialize(item.PropertyType, datas, ref offset);
                            item.SetValue(instanceObject.Instance, obj);
                        }
                        break;
                    }
                case InstanceType.List:
                    {

                        int index = offset;
                        while (offset - index < length && (length >= 4))
                        {
                            object obj = Deserialize(instanceObject.ArgTypes[0], datas, ref offset);
                            instanceObject.AddMethod.Invoke(instanceObject.Instance, new object[] { obj });
                        }
                        break;
                    }
                case InstanceType.Array:
                    {
                        int index = offset;
                        while (offset - index < length && (length >= 4))
                        {
                            object obj = Deserialize(instanceObject.ArgTypes[0], datas, ref offset);
                            instanceObject.AddMethod.Invoke(instanceObject.Instance, new object[] { obj });
                        }
                        instanceObject.ToArrayMethod.Invoke(instanceObject.Instance, null);
                        break;
                    }
                case InstanceType.Dictionary:
                    {
                        int index = offset;
                        while (offset - index < length && (length >= 4))
                        {
                            offset += 4;
                            object key = Deserialize(instanceObject.ArgTypes[0], datas, ref offset);
                            object value = Deserialize(instanceObject.ArgTypes[1], datas, ref offset);
                            if (key != null)
                            {
                                instanceObject.AddMethod.Invoke(instanceObject.Instance, new object[] { key, value });
                            }

                        }

                        break;
                    }
                default:
                    break;
            }


            return instanceObject.Instance;
        }


        #endregion
        private PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }
        private static readonly ConcurrentDictionary<string, InstanceObject> InstanceCache = new ConcurrentDictionary<string, InstanceObject>();

        private InstanceObject GetOrAddInstance(Type type)
        {
            if (type.IsArray && !type.IsGenericType)
            {
                type = Type.GetType($"System.Collections.Generic.List`1[[{type.FullName.Replace("[]", string.Empty)}]]");
                InstanceObject typeInfo = InstanceCache.GetOrAdd(type.FullName, (v) =>
                {
                    InstanceObject instanceObject = new InstanceObject();
                    instanceObject.Type = type;
                    instanceObject.ArgTypes = type.GetGenericArguments();
                    instanceObject.instanceType = InstanceType.Array;
                    instanceObject.Properties = this.GetProperties(type);
                    instanceObject.ProTypes = instanceObject.Properties.Select(a => a.PropertyType).ToArray();
                    instanceObject.AddMethod = type.GetMethod("Add");
                    instanceObject.ToArrayMethod = type.GetMethod("ToArray");
                    return instanceObject;
                });

                typeInfo.Instance = Activator.CreateInstance(type);
                return typeInfo;
            }
            else if (type.IsClass)
            {
                InstanceObject typeInfo = InstanceCache.GetOrAdd(type.FullName, (v) =>
                {
                    InstanceObject instanceObject = new InstanceObject();
                    instanceObject.Type = type;
                    instanceObject.Properties = this.GetProperties(type);
                    instanceObject.ProTypes = instanceObject.Properties.Select(a => a.PropertyType).ToArray();

                    if (type.IsGenericType)
                    {
                        instanceObject.AddMethod = type.GetMethod("Add");
                        instanceObject.ToArrayMethod = type.GetMethod("ToArray");

                        instanceObject.ArgTypes = type.GetGenericArguments();
                        type = type.GetGenericTypeDefinition().MakeGenericType(instanceObject.ArgTypes);

                        if (instanceObject.ArgTypes.Length == 1)
                        {
                            instanceObject.instanceType = InstanceType.List;
                        }
                        else
                        {
                            instanceObject.instanceType = InstanceType.Dictionary;
                        }
                    }
                    else
                    {
                        instanceObject.instanceType = InstanceType.Class;
                    }
                    return instanceObject;
                });
                typeInfo.Instance = Activator.CreateInstance(type);
                return typeInfo;
            }
            return null;
        }
    }
}

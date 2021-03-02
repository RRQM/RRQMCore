using System;
using System.Reflection;

namespace RRQMCore.Serialization
{
    internal class InstanceObject
    {
        internal InstanceType instanceType;
        internal Type Type;
        internal object Instance;
        internal Type[] ArgTypes;
        internal Type[] ProTypes;
        internal PropertyInfo[] Properties;
        internal MethodInfo AddMethod;
        internal MethodInfo ToArrayMethod;
    }
}
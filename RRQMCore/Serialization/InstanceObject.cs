using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Serialization
{
    class InstanceObject
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

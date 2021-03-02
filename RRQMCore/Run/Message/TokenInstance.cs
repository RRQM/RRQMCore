using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Run
{
    internal class TokenInstance
    {
        internal IMessage MessageObject { get; set; }
        internal MethodInfo MethodInfo { get; set; }
    }
}

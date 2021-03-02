using System.Reflection;

namespace RRQMCore.Run
{
    internal class TokenInstance
    {
        internal IMessage MessageObject { get; set; }
        internal MethodInfo MethodInfo { get; set; }
    }
}
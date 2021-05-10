using RRQMCore.Helper;
using System;

namespace RRQMCore.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            object o = "10".ParseToType(typeof(int));
            object oo = typeof(Program).GetDefault();
            object ooo = typeof(double).GetDefault();
        }
    }
}

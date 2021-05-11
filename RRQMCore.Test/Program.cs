using RRQMCore.ByteManager;
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

        private static void TestByteBlock()
        {
            BytePool bytePool = new BytePool(1024 * 1024 * 100, 1024 * 1024);

            //获取任意长度的空闲ByteBlock，如果没有空闲，则创建一个最大单元
            ByteBlock byteBlock1 = bytePool.GetByteBlock();

            //获取不小于64kb长度ByteBlock
            ByteBlock byteBlock2 = bytePool.GetByteBlock(1024 * 64);

            //获取64kb长度ByteBlock，且必须为64kb
            ByteBlock byteBlock3 = bytePool.GetByteBlock(1024 * 64, true);

            byteBlock1.Write(10);//写入byte
            byte[] buffer = new byte[1024];
            new Random().NextBytes(buffer);
            byteBlock1.Write(new byte[1024]);//写入byte[]

            byteBlock1.Dispose();
            byteBlock2.Dispose();
            byteBlock3.Dispose();//回收至内存池
        }
    }
}

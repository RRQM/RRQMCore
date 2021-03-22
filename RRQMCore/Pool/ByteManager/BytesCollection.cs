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
using System.Collections.Concurrent;
using System.Diagnostics;

namespace RRQMCore.ByteManager
{
    /// <summary>
    /// 字节块集合
    /// </summary>
    [DebuggerDisplay("Count = {bytes.Count}")]
    public class BytesCollection
    {
        internal long size;

        internal BytesCollection(long size)
        {
            this.size = size;
        }

        /// <summary>
        /// 所属字节池
        /// </summary>
        public BytePool BytePool { get; internal set; }

        private ConcurrentQueue<ByteBlock> bytes = new ConcurrentQueue<ByteBlock>();

        /// <summary>
        /// 获取当前实例中的空闲的Block
        /// </summary>
        /// <returns></returns>
        public ByteBlock Get()
        {
            ByteBlock byteBlock;
            this.bytes.TryDequeue(out byteBlock);
            if (byteBlock != null)
            {
                byteBlock.Using = true;
                byteBlock.Position = 0;
                byteBlock.length = 0;
            }
            return byteBlock;
        }

        /// <summary>
        /// 向当前集合添加Block
        /// </summary>
        /// <param name="byteBlock"></param>
        public void Add(ByteBlock byteBlock)
        {
            byteBlock.lengthChenged = false;
            byteBlock.BytesCollection = this;
            this.bytes.Enqueue(byteBlock);
        }
    }
}
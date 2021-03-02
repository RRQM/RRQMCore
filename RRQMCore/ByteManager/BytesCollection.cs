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
            }
            return byteBlock;
        }

        /// <summary>
        /// 向当前集合添加Block
        /// </summary>
        /// <param name="byteBlock"></param>
        public void Add(ByteBlock byteBlock)
        {
            byteBlock.BytesCollection = this;
            this.bytes.Enqueue(byteBlock);
        }
    }
}
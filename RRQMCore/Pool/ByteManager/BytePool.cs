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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMCore.ByteManager
{
    /// <summary>
    /// 字节池
    /// </summary>
    public class BytePool
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BytePool()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxSize">字节池最大值</param>
        /// <param name="maxBlockSize">单个Block最大值</param>
        public BytePool(long maxSize, int maxBlockSize)
        {
            this.MaxSize = maxSize;
            this.MaxBlockSize = maxBlockSize;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxSize">字节池最大值</param>
        public BytePool(long maxSize)
        {
            this.MaxSize = maxSize;
        }
        /// <summary>
        /// 块数量
        /// </summary>
        public int BlockCount { get; private set; }

        /// <summary>
        /// 允许的内存池最大值,默认为10M Byte
        /// </summary>
        public long MaxSize { get; set; } = 1024 * 1024 * 10;


        /// <summary>
        /// 单个块最大值，默认为1K Byte
        /// </summary>
        public int MaxBlockSize { get; set; }

        /// <summary>
        /// 当前内存池实际长度
        /// </summary>
        public long ActualSize { get; private set; }

        /// <summary>
        /// 已创建的块的最大值
        /// </summary>
        public long CreatedBlockSize { get; private set; }

        private BytesDictionary bytesDictionary = new BytesDictionary();
        private long id;
        /// <summary>
        /// 获取ByteBlock
        /// </summary>
        /// <param name="byteSize">长度</param>
        /// <param name="equalSize">要求长度相同</param>
        /// <returns></returns>
        public ByteBlock GetByteBlock(long byteSize, bool equalSize)
        {

            ByteBlock byteBlock;
            if (byteSize > MaxBlockSize)
            {
                //创建字节块，但是不进入管理池
                byteBlock = CreatByteBlock(byteSize, false);
            }
            else
            {
                if (CreatedBlockSize < byteSize)
                {
                    //请求长度大于已创建最大长度。
                    //重新创建
                    byteBlock = CreatByteBlock(byteSize);
                }
                else
                {
                    //搜索已创建集合
                    if (bytesDictionary.ContainsKey(byteSize))
                    {
                        byteBlock = bytesDictionary[byteSize].Get();
                        if (byteBlock != null)
                        {
                            byteBlock.ID = Interlocked.Increment(ref id);
                            return byteBlock;
                        }
                    }

                    if (!equalSize)
                    {
                        foreach (long size in bytesDictionary.Keys)
                        {
                            if (size > byteSize)
                            {
                                byteBlock = bytesDictionary[size].Get();
                                if (byteBlock != null)
                                {
                                    byteBlock.ID = Interlocked.Increment(ref id);
                                    return byteBlock;
                                }
                            }
                        }
                    }
                    //未搜索到
                    byteBlock = CreatByteBlock(byteSize);
                }
            }
            byteBlock.ID = Interlocked.Increment(ref id);
            return byteBlock;

        }

        /// <summary>
        /// 获取ByteBlock
        /// </summary>
        /// <param name="byteSize"></param>
        /// <returns></returns>
        public ByteBlock GetByteBlock(long byteSize)
        {
            return this.GetByteBlock(byteSize, false);
        }
        /// <summary>
        /// 获取任意长度的空闲ByteBlock，如果没有空闲，则创建一个最大单元
        /// </summary>
        /// <returns></returns>
        public ByteBlock GetByteBlock()
        {
            ByteBlock byteBlock;
            //搜索已创建集合
            foreach (long size in bytesDictionary.Keys)
            {
                byteBlock = bytesDictionary[size].Get();
                if (byteBlock != null)
                {
                    byteBlock.ID = Interlocked.Increment(ref id);
                    return byteBlock;
                }
            }
            //未搜索到
            byteBlock = CreatByteBlock(this.MaxBlockSize);
            byteBlock.ID = Interlocked.Increment(ref id);
            return byteBlock;
        }

        private ByteBlock CreatByteBlock(long byteSize, bool isBelongPool = true)
        {
            if (byteSize < 0)
            {
                throw new RRQMCore.Exceptions.RRQMException("申请内存的长度不能小于0");
            }
            ByteBlock byteBlock = new ByteBlock();
            byteBlock.Using = true;
            byteBlock.Buffer = new byte[byteSize];
            if (isBelongPool && MaxSize - ActualSize >= byteSize)
            {
                //创建
                lock (this)
                {
                    ActualSize += byteSize;
                    CreatedBlockSize = byteSize;
                    BlockCount++;
                }
                BytesCollection bytesCollection = bytesDictionary.GetOrAdd(byteSize, (v) => { return new BytesCollection(byteSize); });
                bytesCollection.BytePool = this;
                byteBlock.BytesCollection = bytesCollection;
                return byteBlock;
            }
            else
            {
                //创建,但不管理
                return byteBlock;
            }

        }

        internal void OnByteBlockRecycle(ByteBlock byteBlock)
        {
            if (byteBlock.lengthChenged)
            {
                this.ActualSize -= byteBlock.BytesCollection.size;
                this.ActualSize += byteBlock.Length;
            }
            byteBlock.Using = false;
            BytesCollection bytesCollection = bytesDictionary.GetOrAdd(byteBlock.Capacity, (v) => { return new BytesCollection(byteBlock.Length); });
            bytesCollection.BytePool = this;
            bytesCollection.Add(byteBlock);
        }
    }
}

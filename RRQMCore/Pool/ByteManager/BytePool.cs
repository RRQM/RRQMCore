//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RRQMCore.ByteManager
{
    /// <summary>
    /// 字节池
    /// </summary>
    public class BytePool : IEnumerable<ByteBlock>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BytePool() : this(1024 * 1024 * 100, 1024 * 1024)
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
        public BytePool(long maxSize) : this(maxSize, 1024 * 1024)
        {
            this.MaxSize = maxSize;
        }

        private static BytePool bytePool = new BytePool(1024 * 1024 * 512, 1024 * 1024 * 5);

        /// <summary>
        /// 默认内存池，
        /// 内存池最大512Mb，
        /// 单体最大5Mb。
        /// </summary>
        public static BytePool Default { get { return bytePool; } }

        /// <summary>
        /// 允许的内存池最大值,默认为100M Byte
        /// </summary>
        public long MaxSize { get; set; }

        /// <summary>
        /// 单个块最大值，默认为1Mb
        /// </summary>
        public int MaxBlockSize { get; set; }

        private long freeSize;

        /// <summary>
        /// 已创建的块的最大值
        /// </summary>
        public long CreatedBlockSize { get; private set; }

        private BytesDictionary bytesDictionary = new BytesDictionary();

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
                    BytesCollection bytesCollection;
                    //搜索已创建集合
                    if (bytesDictionary.TryGet(byteSize, out bytesCollection))
                    {
                        if (bytesCollection.TryGet(out byteBlock))
                        {
                            byteBlock.Using = true;
                            byteBlock.Position = 0;
                            byteBlock.length = 0;
                            freeSize -= byteBlock.Capacity;
                            return byteBlock;
                        }
                    }

                    if (!equalSize)
                    {
                        int len = bytesDictionary.Keys.Count;
                        for (int i = 0; i < len; i++)
                        {
                            if (bytesDictionary.Keys[i] > byteSize)
                            {
                                if (this.bytesDictionary.TryGet(bytesDictionary.Keys[i], out bytesCollection))
                                {
                                    if (bytesCollection.TryGet(out byteBlock))
                                    {
                                        byteBlock.Using = true;
                                        byteBlock.Position = 0;
                                        byteBlock.length = 0;
                                        freeSize -= byteBlock.Capacity;
                                        return byteBlock;
                                    }
                                }
                            }
                        }
                    }
                    //未搜索到
                    byteBlock = CreatByteBlock(byteSize);
                }
            }
            byteBlock.Using = true;
            byteBlock.Position = 0;
            byteBlock.length = 0;
            return byteBlock;
        }

        /// <summary>
        /// 获取ByteBlock
        /// </summary>
        /// <param name="byteSize"></param>
        /// <returns></returns>
        public ByteBlock GetByteBlock(long byteSize)
        {
            ByteBlock byteBlock = this.GetByteBlock(byteSize, false);

            if (byteBlock.Position > 0)
            {
            }
            return byteBlock;
        }

        /// <summary>
        /// 获取最大长度的ByteBlock
        /// </summary>
        /// <returns></returns>
        public ByteBlock GetByteBlock()
        {
            return this.GetByteBlock(this.MaxBlockSize, true);
        }

        private ByteBlock CreatByteBlock(long byteSize, bool isBelongPool = true)
        {
            if (byteSize < 0)
            {
                throw new RRQMException("申请内存的长度不能小于0");
            }
            ByteBlock byteBlock = new ByteBlock();
            byteBlock.Buffer = new byte[byteSize];
            if (isBelongPool)
            {
                BytesCollection bytesCollection;
                lock (this)
                {
                    if (!this.bytesDictionary.TryGet(byteSize, out bytesCollection))
                    {
                        bytesCollection = new BytesCollection(byteSize);
                        //this.bytesDictionary.Add(byteSize, bytesCollection);
                    }
                }

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
            BytesCollection bytesCollection;
            this.CreatedBlockSize = Math.Max(CreatedBlockSize, byteBlock.Capacity);
            if (MaxSize - freeSize >= byteBlock.Capacity)
            {
                freeSize += byteBlock.Capacity;
                if (this.bytesDictionary.TryGet(byteBlock.Capacity, out bytesCollection))
                {
                    bytesCollection.BytePool = this;
                    bytesCollection.Add(byteBlock);
                }
                else
                {
                    lock (this)
                    {
                        bytesCollection = new BytesCollection(byteBlock.Capacity);
                        this.bytesDictionary.Add(byteBlock.Capacity, bytesCollection);
                    }
                }
            }
            else
            {
                int len = this.bytesDictionary.Keys.Count;
                long size = 0;
                for (int i = 0; i < len; i++)
                {
                    if (this.bytesDictionary.TryGet(this.bytesDictionary.Keys[i], out BytesCollection collection))
                    {
                        size += collection.FreeSize;
                    }
                }

                this.freeSize = size;
                byteBlock.AbsoluteDispose();
            }
        }

        /// <summary>
        /// 返回迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ByteBlock> GetEnumerator()
        {
            List<ByteBlock> blocks = new List<ByteBlock>();
            foreach (var item in bytesDictionary.Keys)
            {
                if (bytesDictionary.TryGet(item, out BytesCollection bytesCollection))
                {
                    blocks.AddRange(bytesCollection.ToList());
                }
            }

            return blocks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            List<ByteBlock> blocks = new List<ByteBlock>();
            foreach (var item in bytesDictionary.Keys)
            {
                if (bytesDictionary.TryGet(item, out BytesCollection bytesCollection))
                {
                    blocks.AddRange(bytesCollection.ToList());
                }
            }

            return blocks.GetEnumerator();
        }
    }
}
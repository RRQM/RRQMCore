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
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMCore.ByteManager
{
    /// <summary>
    /// 字节块流
    /// </summary>
    public sealed class ByteBlock : Stream, IDisposable
    {
        internal ByteBlock()
        {
        }

        /// <summary>
        /// 该字节流块所属集合，若值为null则意味着该流块未被管理，可能会被GC
        /// </summary>
        public BytesCollection BytesCollection { get; internal set; }

        /// <summary>
        /// 字节实例
        /// </summary>
        public byte[] Buffer { get; internal set; }


        private bool holding;
        /// <summary>
        /// 表示持续性持有，为True时，Dispose将调用无效。
        /// </summary>
        public bool Holding
        {
            get { return holding; }
        }


        internal bool @using;

        /// <summary>
        /// 使用状态
        /// </summary>
        public bool Using
        {
            get { return @using; }
        }

        /// <summary>
        /// 可读取
        /// </summary>
        public override bool CanRead => this.@using;

        /// <summary>
        /// 支持查找
        /// </summary>
        public override bool CanSeek => this.@using;

        /// <summary>
        /// 可写入
        /// </summary>
        public override bool CanWrite => this.@using;

        /// <summary>
        /// 真实长度
        /// </summary>
        public override long Length { get { return length; } }

        /// <summary>
        /// Int真实长度
        /// </summary>
        public int Len { get { return (int)length; } }

        internal long length;

        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity => this.Buffer.Length;

        /// <summary>
        /// 流位置
        /// </summary>
        public override long Position { get; set; }

        /// <summary>
        /// 重新指定Buffer
        /// </summary>
        public void SetBuffer(byte[] buffer)
        {
            if (!this.@using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            if (buffer != null)
            {
                this.Buffer = buffer;
            }
        }

        /// <summary>
        /// 设置持续持有属性，当为False时，会自动调用Dispose。
        /// </summary>
        /// <param name="holding"></param>
        public void SetHolding(bool holding)
        {
            if (!this.@using)
            {
                throw new Exceptions.RRQMException("内存块已释放");
            }
            this.holding = holding;
            if (!holding)
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!this.@using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            int len = this.Buffer.Length - this.Position > count ? count : this.Buffer.Length - (int)this.Position;
            Array.Copy(this.Buffer, this.Position, buffer, offset, len);
            this.Position += len;
            return len;
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!this.@using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            if (this.Buffer.Length - this.Position < count)
            {
                byte[] newBuffer = new byte[this.Buffer.Length + count];
                Array.Copy(this.Buffer, newBuffer, this.Buffer.Length);
                this.Buffer = newBuffer;
            }
            Array.Copy(buffer, offset, Buffer, this.Position, count);
            this.Position += count;
            this.length += count;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public int Read(byte[] buffer)
        {
            return Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public void Write(byte[] buffer)
        {
            this.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <returns></returns>
        public void Write(byte byteBuffer)
        {
            if (!this.@using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            if (this.Buffer.Length - this.Position < 1)
            {
                byte[] newBuffer = new byte[this.Buffer.Length + 1024];
                Array.Copy(this.Buffer, newBuffer, this.Buffer.Length);
                this.Buffer = newBuffer;
            }
            this.Buffer[this.Position] = byteBuffer;
            this.Position += 1;
            this.length += 1;
        }

        /// <summary>
        /// 转换为有效内存
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            if (!this.@using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            byte[] buffer = new byte[this.Length];
            Array.Copy(this.Buffer, 0, buffer, 0, this.Length);
            return buffer;
        }

        /// <summary>
        /// 无实际效果
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// 设置流位置
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!this.@using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.Position = offset;
                    break;

                case SeekOrigin.Current:
                    this.Position = offset;
                    break;

                case SeekOrigin.End:
                    this.Position = this.Length + offset;
                    break;
            }
            return this.Position;
        }

        /// <summary>
        /// 设置实际长度
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            if (!this.@using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            if (value > this.Buffer.Length)
            {
                throw new RRQMCore.Exceptions.RRQMException("设置值超出容量");
            }
            this.length = value;
        }

        /// <summary>
        /// 回收资源
        /// </summary>
        public new void Dispose()
        {
            if (this.Holding)
            {
                return;
            }
            if (!this.@using)
            {
                throw new Exceptions.RRQMException("重复释放");
            }
            this.@using = false;

            if (this.BytesCollection != null)
            {
                this.BytesCollection.BytePool.OnByteBlockRecycle(this);
            }
        }

        /// <summary>
        /// 直接完全释放，游离该对象，等待GC
        /// </summary>
        public void AbsoluteDispose()
        {
            this.@using = false;
            this.Position = 0;
            this.length = 0;
            this.BytesCollection = null;
        }
    }
}
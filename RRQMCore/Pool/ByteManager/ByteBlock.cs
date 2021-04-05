//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System;
using System.IO;

namespace RRQMCore.ByteManager
{
    /// <summary>
    /// 字节块流
    /// </summary>
    public class ByteBlock : Stream, IDisposable
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

        /// <summary>
        /// 表示持续性持有，为True时，Dispose将调用无效。
        /// </summary>
        public bool Holding { get;private set; }


        /// <summary>
        /// 使用状态
        /// </summary>
        public bool Using { get; internal set; }

        /// <summary>
        /// 可读取
        /// </summary>
        public override bool CanRead => this.Using;

        /// <summary>
        /// 支持查找
        /// </summary>
        public override bool CanSeek => this.Using;

        /// <summary>
        /// 可写入
        /// </summary>
        public override bool CanWrite => this.Using;

        /// <summary>
        /// 流长度
        /// </summary>
        public override long Length { get { return length; } }

        internal long length;

        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity => this.Buffer.Length;

        /// <summary>
        /// 流位置
        /// </summary>
        public override long Position { get; set; }

        internal bool lengthChenged;

        /// <summary>
        /// 重新指定Buffer
        /// </summary>
        public void SetBuffer(byte[] buffer)
        {
            if (!this.Using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            if (buffer != null)
            {
                this.Buffer = buffer;
                this.lengthChenged = true;
            }
        }

        /// <summary>
        /// 设置持续持有属性，当为False时，会自动调用Dispose。
        /// </summary>
        /// <param name="enable"></param>
        public void SetHolding(bool enable)
        {
            if (!this.Using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            this.Holding = enable;
            if (!enable)
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
            if (!this.Using)
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
            if (!this.Using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            if (this.Buffer.Length - this.Position < count)
            {
                byte[] newBuffer = new byte[this.Buffer.Length+count];
                Array.Copy(this.Buffer, newBuffer, this.Buffer.Length);
                this.Buffer = newBuffer;
                this.lengthChenged = true;
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
            if (!this.Using)
            {
                throw new RRQMCore.Exceptions.RRQMException("内存块已释放");
            }
            if (this.Buffer.Length - this.Position < 1)
            {
                byte[] newBuffer = new byte[this.Buffer.Length + 1024];
                Array.Copy(this.Buffer, newBuffer, this.Buffer.Length);
                this.Buffer = newBuffer;
                this.lengthChenged = true;
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
            if (!this.Using)
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
            if (!this.Using)
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
            if (!this.Using)
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
            if (this.BytesCollection != null)
            {
                this.BytesCollection.BytePool.OnByteBlockRecycle(this);
            }
        }

    }
}
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
        /// 使用状态
        /// </summary>
        public bool Using { get; internal set; }

        /// <summary>
        /// 可读取
        /// </summary>
        public override bool CanRead => true;

        /// <summary>
        /// 支持查找
        /// </summary>
        public override bool CanSeek => true;

        /// <summary>
        /// 可写入
        /// </summary>
        public override bool CanWrite => true;

        /// <summary>
        /// 流长度
        /// </summary>
        public override long Length => this.Buffer.Length;

        /// <summary>
        /// 流位置
        /// </summary>
        public override long Position { get; set; }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
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
            if (this.Buffer.Length - this.Position < count)
            {
                byte[] newBuffer = new byte[this.Position + count];
                Array.Copy(this.Buffer, newBuffer, this.Buffer.Length);
                this.Buffer = newBuffer;
            }
            Array.Copy(buffer, offset, Buffer, this.Position, count);
            this.Position += count;
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
            if (this.Buffer.Length - this.Position < 1)
            {
                byte[] newBuffer = new byte[this.Position + 1];
                Array.Copy(this.Buffer, newBuffer, this.Buffer.Length);
                this.Buffer = newBuffer;
            }
            this.Buffer[this.Position] = byteBuffer;
            this.Position += 1;
        }

        /// <summary>
        /// 转换为有效内存
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            byte[] buffer = new byte[this.Position];
            Array.Copy(this.Buffer, 0, buffer, 0, this.Position);
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
        /// 该方法无意义
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
        }

        /// <summary>
        /// 回收资源
        /// </summary>
        public new void Dispose()
        {
            if (this.BytesCollection != null)
            {
                this.BytesCollection.BytePool.OnByteBlockRecycle(this);
            }
        }
    }
}
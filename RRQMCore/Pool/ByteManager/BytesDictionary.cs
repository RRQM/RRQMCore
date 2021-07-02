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
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RRQMCore.ByteManager
{
    /// <summary>
    /// 字节块集合字典索引。
    /// </summary>
    public class BytesDictionary
    {
        internal BytesDictionary()
        {
            this.bytesDic = new ConcurrentDictionary<long, BytesCollection>();
            this.keys = new List<long>();
        }

        private ConcurrentDictionary<long, BytesCollection> bytesDic;
        private List<long> keys;

        internal List<long> Keys { get { return this.keys; } }

        internal bool ContainsKey(long key)
        {
            return bytesDic.ContainsKey(key);
        }

        internal bool TryGet(long key, out BytesCollection bytesCollection)
        {
            return bytesDic.TryGetValue(key, out bytesCollection);
        }

        internal void Add(long key, BytesCollection bytesCollection)
        {
            if (this.bytesDic.TryAdd(key, bytesCollection))
            {
                this.keys.Add(key);
            }
        }
    }
}
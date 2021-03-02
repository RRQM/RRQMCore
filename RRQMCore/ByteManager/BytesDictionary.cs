using System.Collections.Concurrent;

namespace RRQMCore.ByteManager
{
    /// <summary>
    /// 字节块集合字典索引。
    /// </summary>
    public class BytesDictionary : ConcurrentDictionary<long, BytesCollection>
    {
    }
}
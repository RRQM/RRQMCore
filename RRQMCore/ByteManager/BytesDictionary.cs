using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.ByteManager
{
    /// <summary>
    /// 字节块集合字典索引。
    /// </summary>
    public class BytesDictionary : ConcurrentDictionary<long, BytesCollection>
    {

    }
}

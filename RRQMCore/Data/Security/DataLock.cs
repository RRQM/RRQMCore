using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RRQMCore.Data.Security
{
    /// <summary>
    /// 数据锁,用于加密或解密
    /// </summary>
    public static class DataLock
    {
        /// <summary>
        /// 使用3DES加密
        /// </summary>
        /// <param name="data">待加密字节</param>
        /// <param name="encryptKey">加密口令（长度为8）</param>
        /// <returns></returns>
        public static byte[] EncryptDES(byte[] data, string encryptKey)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = { 0x12, 0x34, 4, 0x78, 0x90, 255, 0xCD, 0xEF };
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                {
                    cStream.Write(data, 0, data.Length);
                    cStream.FlushFinalBlock();
                    return mStream.ToArray();
                }
            }
        }

        /// <summary>
        /// 使用3DES解密
        /// </summary>
        /// <param name="data">待解密字节</param>
        /// <param name="decryptionKey">解密口令（长度为8）</param>
        /// <returns></returns>
        public static byte[] DecryptDES(byte[] data, string decryptionKey)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptionKey);
            byte[] rgbIV = { 0x12, 0x34, 4, 0x78, 0x90, 255, 0xCD, 0xEF };
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                {
                    cStream.Write(data, 0, data.Length);
                    cStream.FlushFinalBlock();
                    return mStream.ToArray();
                }
            }
        }
    }
}
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace RRQMCore.IO
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public static class FileControler
    {
        /// <summary>
        /// 获得文件Hash值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileHash(string filePath)
        {
            try
            {
                HashAlgorithm hash = HashAlgorithm.Create();
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    byte[] HashValue = hash.ComputeHash(fileStream);
                    return BitConverter.ToString(HashValue).Replace("-", "");
                }
            }
            catch
            {
                return "0000000000000000000000000000000000000000";
            }
        }

        /// <summary>
        /// 获得流Hash值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetStreamHash(Stream stream)
        {
            try
            {
                HashAlgorithm hash = HashAlgorithm.Create();
                byte[] HashValue = hash.ComputeHash(stream);
                return BitConverter.ToString(HashValue).Replace("-", "");
            }
            catch
            {
                return "0000000000000000000000000000000000000000";
            }
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        private const int OF_READWRITE = 2;

        private const int OF_SHARE_DENY_NONE = 0x40;

        private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        /// <summary>
        /// 判断文件是否被已打开
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public static bool FileIsOpen(string fileFullName)
        {
            if (!File.Exists(fileFullName))
            {
                return false;
            }

            IntPtr handle = _lopen(fileFullName, OF_READWRITE | OF_SHARE_DENY_NONE);

            if (handle == HFILE_ERROR)
            {
                return true;
            }

            CloseHandle(handle);

            return false;
        }
    }
}
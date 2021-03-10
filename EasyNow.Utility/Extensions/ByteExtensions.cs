using System.Security.Cryptography;
using System.Text;

namespace EasyNow.Utility.Extensions
{
    public static class ByteExtensions
    {
        /// <summary>
        /// 得到MD5加密后的数据
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToMD5String(this byte[] bytes)
        {
            using (var md5=MD5.Create())
            {
                var s = md5.ComputeHash(bytes);
                var sb=new StringBuilder();
                s.Foreach(e => { sb.Append(e.ToString("X2")); });
                return sb.ToString();
            }
        }
    }
}
using System.Security.Cryptography;
using System.Text;

namespace Shunmai.Bxb.Utilities.Encryption
{
    /// <summary>
    /// MD5操作类，包含了MD5加密方法、MD5校验方法等。
    /// <para>[2018.08.26,fuzhi.zhao]</para>
    /// </summary>
    public class MD5Helper
    {

        /// <summary>生成MD5
        /// （英文字母为小写，32位）（默认使用UTF-8编码）
        /// </summary>
        /// <param name="source">待加密字符串</param>
        /// <param name="encodeName">编码方式（该参数可选，默认值为UTF-8）</param>
        /// <returns></returns>
        public static string Encrypt(string source, string encodeName = "UTF-8")
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] buffer = System.Text.Encoding.GetEncoding(encodeName).GetBytes(source);
                buffer = md5.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                foreach (byte item in buffer)
                {
                    sb.Append(item.ToString("x").PadLeft(2, '0'));
                }
                return sb.ToString();
            }
        }

        /// <summary> 验证MD5字符串是否正确，严格校验大小写。 </summary>
        /// <param name="source">源字符串，即明文</param>
        /// <param name="valueEncrypted">待验证的MD5字符串</param>
        /// <returns></returns>
        public static bool Verify(string source, string valueEncrypted)
        {
            return valueEncrypted.Equals(Encrypt(source));
        }
    }
}

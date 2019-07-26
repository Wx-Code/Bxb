using Shunmai.Bxb.Common.Exceptions;
using Shunmai.Bxb.Utilities.Check;
using System;

namespace Shunmai.Bxb.Api.Admin.Models
{
    public sealed class Token
    {
        private const string SEPERATOR = "|";

        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId { get; private set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expires { get; private set; }
        /// <summary>
        /// 加密密钥
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 初始化 <see cref="Token"/> 新实例
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <param name="expires">Token 过期时间</param>
        /// <param name="key">加密密钥</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Token(string userId, DateTime expires, string key)
        {
            Check.Empty(userId, nameof(userId));
            Check.CreaterThanCurrentTime(expires, nameof(expires));
            Check.Empty(key, nameof(key));

            UserId = userId;
            Expires = expires;
            Key = key;
        }

        /// <summary>
        /// 初始化 <see cref="Token"/> 新实例
        /// </summary>
        /// <param name="encryptedToken">加密后的 Token 字符串</param>
        /// <param name="key">解密密钥</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidTokenException"></exception>
        public Token(string encryptedToken, string key)
        {
            Check.Empty(encryptedToken, nameof(encryptedToken));
            Check.Empty(key, nameof(key));

            Key = key;
            DecryptToken(encryptedToken);
        }

        private void DecryptToken(string encryptedToken)
        {
            try
            {
                var original = Utilities.Helpers.Encrypt.AesDecrypt(encryptedToken, Key);
                var portions = original.Split(SEPERATOR);
                if (portions.Length != 2 || !DateTime.TryParse(portions[1], out DateTime expires))
                {
                    throw new InvalidTokenException();
                }

                UserId = portions[0];
                Expires = expires;
            }
            catch
            {
                throw new InvalidTokenException();
            }
        }

        /// <summary>
        /// 获取加密后的 Token 字符串
        /// </summary>
        /// <returns></returns>
        public string Encrypt()
        {
            var s = $"{UserId}{SEPERATOR}{Expires.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
            return Utilities.Helpers.Encrypt.AesEncrypt(s, Key);
        }

    }
}

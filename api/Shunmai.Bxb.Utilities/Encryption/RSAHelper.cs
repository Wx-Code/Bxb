using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Shunmai.Bxb.Utilities.Encryption
{
    /// <summary>
    /// RSA 加密工具类
    /// </summary>
    public static class RSAHelper
    {
        /// <summary>
        /// 采用 PKCS#1 格式的公钥对源字符串进行 RSA 加密，返回密文的 BASE64 格式的字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string EncryptWithPKCS1Format(string source, string publicKey)
        {
            var pr = new PemReader(new StringReader(publicKey));
            var pemObj = (RsaKeyParameters)pr.ReadObject();
            var rsaParams = new RSAParameters();
            rsaParams.Modulus = pemObj.Modulus.ToByteArrayUnsigned();
            rsaParams.Exponent = pemObj.Exponent.ToByteArrayUnsigned();

            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(rsaParams);
                var data = Encoding.UTF8.GetBytes(source);
                var bytes = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA1);
                return Convert.ToBase64String(bytes);
            }
        }
       
    }
}

using Aliyun.OSS;
using System;
using System.IO;


namespace Shunmai.Bxb.Utils.Helpers
{
    public class UploadFileToOssHelper
    {
        /// <summary>
        /// 上传文件到Ali Oss服务通用方法
        /// </summary>
        /// <param name="accessKeyId">key</param>
        /// <param name="accessKeySecret">Secret</param>
        /// <param name="endpoint"></param>
        /// <param name="bucketName">Oss服务的存储空间名称</param>
        /// <param name="onlineurl">要在线上查看的地址</param>
        /// <param name="filestream">文件流</param>       
        public static string PutObjectFromFile(string onlineurl,Stream filestream,string suffix,
                                               string website,string accessKeyId,string accessKeySecret,
                                               string endpoint,string bucketName)
        {
            Check.Check.Empty(onlineurl,nameof(onlineurl));
            Check.Check.Empty(suffix, nameof(suffix));
            Check.Check.Empty(website, nameof(website));
            Check.Check.Empty(accessKeyId, nameof(accessKeyId));
            Check.Check.Empty(accessKeySecret, nameof(accessKeySecret));
            Check.Check.Empty(endpoint, nameof(endpoint));
            Check.Check.Empty(bucketName, nameof(bucketName));

            OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            string key = Guid.NewGuid().ToString("N");
            string onlinePath = onlineurl + key + suffix;
            client.PutObject(bucketName, onlinePath, filestream);                
            return website+"/"+onlinePath;
        }
    }
}

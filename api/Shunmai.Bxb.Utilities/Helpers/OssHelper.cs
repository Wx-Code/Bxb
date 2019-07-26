using Aliyun.OSS;
using Shunmai.Bxb.Utilities.Check;
using System;
using System.IO;


namespace Shunmai.Bxb.Utils.Helpers
{
    public class OssHelper
    {
        /// <summary>
        /// 
        /// </summary>

        public int MyProperty { get; set; }

        /// <summary>
        /// 上传文件到Ali Oss服务通用方法
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <param name="fileStream">文件流</param>
        /// <param name="dirName">文件在服务器上保存的目录名称</param>
        /// <param name="filename">文件扩展名</param>
        /// <param name="website">浏览上传后的文件的主机地址</param>
        /// <param name="accessKeyId">key</param>
        /// <param name="accessKeySecret">Secret</param>
        /// <param name="endpoint"></param>
        /// <param name="bucketName">Oss服务的存储空间名称</param>
        /// <returns></returns>
        public static string UploadFile(
            string filename
            , Stream fileStream
            , string dirName
            , string website
            , string accessKeyId
            , string accessKeySecret
            , string endpoint
            , string bucketName
        )
        {
            Check.Empty(filename, nameof(filename));
            Check.Null(fileStream, nameof(fileStream));
            Check.Empty(dirName, nameof(dirName));
            Check.Empty(website, nameof(website));
            Check.Empty(accessKeyId, nameof(accessKeyId));
            Check.Empty(accessKeySecret, nameof(accessKeySecret));
            Check.Empty(endpoint, nameof(endpoint));
            Check.Empty(bucketName, nameof(bucketName));

            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            var guid = Guid.NewGuid().ToString("N");
            var filePath = dirName + guid + Path.GetExtension(filename);
            client.PutObject(bucketName, filePath, fileStream);
            return website + "/" + filePath;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Common.Models
{
    public class AliOssServiceConfig
    {
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        //上传Oss服务后，图片查看地址的域名
        public string Website { get; set; }
        public string Endpoint { get; set; }
        public string BucketName { get; set; }
    }
}

using System.ComponentModel;

namespace Shunmai.Bxb.Common.Enums
{
    /// <summary>
    /// 阿里图床服务器目录类型（用于区别不同业务的图片在服务器上存储的目录）
    /// </summary>
    public enum OssFileDirType
    {
        /// <summary>
        /// 用户实名认证的身份证图
        /// </summary>
        [Description("IdCard/")]
        UserIdCard = 0,

        /// <summary>
        /// 轮播图广告图
        /// </summary>
        [Description("RotatingFigure/")]
        RotatingFigure = 1,

        /// <summary>
        /// 资讯图片
        /// </summary>
        [Description("News/")]
        News = 2,

        /// <summary>
        /// 商品相关图片
        /// </summary>
        [Description("Product/")]
        Product = 3,

        /// <summary>
        /// 商品海报相关图片
        /// </summary>
        [Description("Product/PosterImg/")]
        ProductPoster = 4,

        /// <summary>
        /// 企业相关图片
        /// </summary>
        [Description("Company/")]
        Company = 5,

        /// <summary>
        /// 活动
        /// </summary>
        [Description("Game/ActivityImg/")]
        GameActivity=6,

        /// <summary>
        /// 微信二维码
        /// </summary>
        [Description("Bxb/Qrcode/")]
        PersonalWechatQrCode = 7,
    }
}

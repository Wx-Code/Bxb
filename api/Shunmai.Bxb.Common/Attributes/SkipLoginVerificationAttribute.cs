using System;

namespace Shunmai.Bxb.Common.Attributes
{
    /// <summary>
    /// 通过向 Controller 或者 Action 添加此特性来跳过登录验证
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class SkipLoginVerificationAttribute : Attribute
    {

    }
}

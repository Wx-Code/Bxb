using Shunmai.Bxb.Common.Constants;

namespace Shunmai.Bxb.Common.Attributes
{
    /// <summary>
    /// 用于验证给定值是否为一个合法的移动电话号码
    /// </summary>
    public class MobilePhoneAttribute : PatternAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return $"The field '{name}' is not a valid mobile phone number.";
        }

        public MobilePhoneAttribute() : base(Patterns.MOBILE_PHONE)
        {

        }
    }
}

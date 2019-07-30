using Shunmai.Bxb.Utilities.Helpers;
using System;

namespace Shunmai.Bxb.Utilities.Sms
{
    public class DefaultSms : ISmsProvider
    {
        public const string API_FORMAT = "http://222.73.117.138:7891/mt?un=N13661252777&pw=792765&da={1}&sm={0}&dc=15&rd=1";
        public const string CONTENT_FORMAT = "【狂抖】您的验证码为：{0}，请在10分钟内输入。为保障您的账号安全，万万不能告诉其他人哟！";

        public string CreateCode(int count)
        {
            var code = SmsLogic.GetRandomint(count);

            return code;
        }

        public ResponseEntity Send(string phoneNo, int count = 4)
        {
            var code = SmsLogic.GetRandomint(count);
            var content = string.Format(CONTENT_FORMAT, code);

            var result = Send(phoneNo, content);
            if (result == null) return result;

            result.Code = code;

            return result;
        }

        public ResponseEntity Send(string phoneNo, string content)
        {
            var hex = SmsLogic.GetHex(content);
            var url = string.Format(API_FORMAT, hex, phoneNo);

            var result = ApiRequestHelper.Get<string>(url, null, null, false);
            if (result != null && long.Parse(result.Substring(result.IndexOf("=") + 1)) > 0)
            {
                ResponseEntity entity = new ResponseEntity
                {
                    Content = content,
                    PhoneNo = phoneNo,
                    SendTime = DateTime.Now
                };
                return entity;
            }

            return null;
        }

        public ResponseEntity SendCode(string phoneNo, string code)
        {
            var content = string.Format(CONTENT_FORMAT, code);
            var result = Send(phoneNo, content);
            if (result == null) return result;
            result.Code = code;
            return result;
        }

    }
}

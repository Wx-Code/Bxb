using System;
using System.Text;

namespace Shunmai.Bxb.Utilities.Sms
{
    internal class SmsLogic
    {
        internal static string GetHex(string content)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(content);

            string[] strArr = new string[bytes.Length];

            System.Text.StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)

            {

                sb.Append(bytes[i].ToString("x"));

            }

            return sb.ToString();
        }

        internal static string GetRandomint(int codeCount = 4)
        {
            Random random = new Random();
            StringBuilder sbmin = new StringBuilder();
            StringBuilder sbmax = new StringBuilder();
            for (int i = 0; i < codeCount; i++)
            {
                sbmin.Append("1");
                sbmax.Append("9");
            }
            return random.Next(Convert.ToInt32(sbmin.ToString()), Convert.ToInt32(sbmax.ToString())).ToString();
        }
    }
}

using System;
using System.Reflection;

namespace Shunmai.Bxb.Utilities.Sms
{
    public class SmsFactory 
    {
        public static ISmsProvider Create(string typeName=nameof(DefaultSms))
        {
            var className = $"{Assembly.GetExecutingAssembly().GetName().Name}.Sms.{typeName}";
            var type = Type.GetType(className);
            var sms=Activator.CreateInstance(type) as ISmsProvider;

            return sms;
        }
    }
}

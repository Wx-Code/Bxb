using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Shunmai.Bxb.Utilities
{
    /// <summary>
    /// 解决小数反序列化为整数抛出异常的问题
    /// 使用方法 JsonConvert.DeserializeObject<T>(value, new DecimalJsonConverter())
    /// </summary>
    internal class DecimalJsonConverter : JsonConverter
    {
        public DecimalJsonConverter()
        {
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var token = JToken.Load(reader);
                if (token.Type == JTokenType.Integer)
                {
                    return token.ToObject(objectType);
                }
                return Convert.ChangeType(token.ToString(), objectType);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int) 
                || objectType == typeof(long) 
                || objectType == typeof(short)
                || objectType == typeof(ushort)
                || objectType == typeof(uint)
                || objectType == typeof(ulong)
                || objectType == typeof(sbyte)
                ;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

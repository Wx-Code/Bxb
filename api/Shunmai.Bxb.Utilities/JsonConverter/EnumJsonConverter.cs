using Newtonsoft.Json;
using Shunmai.Bxb.Utilities.Extenssions;
using System;

namespace Shunmai.Bxb.Utilities
{
    /// <summary>
    /// 主要用于解决将 C# 枚举序列化时，默认的序列化器取的是枚举名称，而非枚举的整型值的问题
    /// </summary>
    /// <author>谭光洪/author>
    /// <since>2018-12-25</since>
    public class EnumJsonConverter : JsonConverter
    {
        public EnumJsonConverter()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            var t = objectType.IsNullableType()
                ? Nullable.GetUnderlyingType(objectType)
                : objectType;

            return t.IsEnum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将枚举的整型值写入目标流中
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(Convert.ToInt32(value).ToString());
        }
    }
}

using Shunmai.Bxb.Utilities.Extenssions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Shunmai.Bxb.Utilities.Helpers
{
    public static class Enums
    {
        /// <summary>
        /// 把枚举转换为键值对集合
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="getText">获得值得文本e => e.GetDescription()</param>
        /// <returns>以枚举值为key，枚举文本为value的键值对集合</returns>
        public static Dictionary<int, string> ToDictionary(Type enumType, Func<Enum, string> getText)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("传入的参数必须是枚举类型！", "enumType");
            }
            Dictionary<int, string> enumDic = new Dictionary<int, string>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (Enum enumValue in enumValues)
            {
                int key = Convert.ToInt32(enumValue);
                string value = getText(enumValue);
                enumDic.Add(key, value);
            }
            return enumDic;
        }

        /// <summary>
        /// 把枚举转换为键值对集合，默认读取描述
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>以枚举值为key，枚举文本为value的键值对集合</returns>
        public static Dictionary<int, string> GetValueDescDict(Type enumType)
        {
            return ToDictionary(enumType, e => e.GetDescription());
        }

        ///// <summary>
        ///// 扩展方法，获得枚举的Description
        ///// </summary>
        ///// <param name="value">枚举值</param>
        ///// <param name="nameInstead">当枚举值没有定义DescriptionAttribute，是否使用枚举名代替，默认是使用</param>
        ///// <returns>枚举的Description</returns>
        //public static string GetDescription(this Enum value, bool nameInstead = true)
        //{
        //    Type type = value.GetType();
        //    string name = Enum.GetName(type, value);
        //    if (name == null) return null;
        //    var field = type.GetField(name);
        //    var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        //    if (attribute == null && nameInstead) return name;
        //    return attribute == null ? string.Empty : attribute.Description;
        //}
    }
}

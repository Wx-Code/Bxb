using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shunmai.Bxb.Utilities.Helpers
{
    public static class ReflectHelper
    {
        /// <summary>
        /// 获取给定枚举上绑定的特性实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static T GetEnumAttributeOrNull<T>(Enum @enum, bool inherit = false) where T: Attribute
        {
            try
            {
                var type = @enum.GetType();
                var member = type.GetMember(@enum.ToString())[0];
                var attrs = member.GetCustomAttributes(typeof(T), inherit);
                return attrs.FirstOrDefault() as T;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T GetPropertyValue<T>(object instance, string propertyName)
        {
            if (instance == null)
            {
                return default(T);
            }
            try
            {
                var type = instance.GetType();
                var prop = type.GetProperty(propertyName);
                return (T)prop.GetValue(instance);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}

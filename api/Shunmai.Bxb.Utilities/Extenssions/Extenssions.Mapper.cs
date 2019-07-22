using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;

namespace Shunmai.Bxb.Utilities.Extenssions
{
    /// <summary>
    /// 对象映射
    /// </summary>
    public static partial class Extenssions
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object Sync = new object();

        /// <summary>
        /// 将源对象映射到目标对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="destination">目标对象</param>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination, params Expression<Func<TDestination, object>>[] expressions)
        {
            return MapTo<TDestination>(source, destination, expressions);
        }

        /// <summary>
        /// 将源对象映射到目标对象
        /// </summary>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源对象</param>
        public static TDestination MapTo<TDestination>(this object source, params Expression<Func<TDestination, object>>[] expressions) where TDestination : new()
        {
            return MapTo(source, new TDestination(), expressions);
        }

        /// <summary>
        /// 将源对象映射到目标对象
        /// </summary>
        private static TDestination MapTo<TDestination>(object source, TDestination destination, params Expression<Func<TDestination, object>>[] expressions)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            var sourceType = GetType(source);
            var destinationType = GetType(destination);
            var ignoreMembers = GetIgnoreMembers(expressions);
            var map = GetMap(sourceType, destinationType, ignoreMembers);
            if (map != null)
                return Mapper.Map(source, destination);
            lock (Sync)
            {
                map = GetMap(sourceType, destinationType, ignoreMembers);
                if (map != null)
                    return Mapper.Map(source, destination);
                InitMaps(sourceType, destinationType, ignoreMembers);
            }
            return Mapper.Map(source, destination);
        }

        private static List<string> GetIgnoreMembers<TDestination, TSelector>(Expression<Func<TDestination, TSelector>>[] expressions)
        {
            var ignoreMembers = new List<string>();
            if (expressions != null)
            {
                foreach (var exp in expressions)
                {
                    var expression = (MemberExpression)exp.Body;
                    ignoreMembers.Add(expression.Member.Name);
                }
            }

            return ignoreMembers;
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        private static Type GetType(object obj)
        {
            var type = obj.GetType();
            if ((obj is System.Collections.IEnumerable) == false)
                return type;
            if (type.IsArray)
                return type.GetElementType();
            var genericArgumentsTypes = type.GetTypeInfo().GetGenericArguments();
            if (genericArgumentsTypes == null || genericArgumentsTypes.Length == 0)
                throw new ArgumentException("泛型类型参数不能为空");
            return genericArgumentsTypes[0];
        }

        /// <summary>
        /// 获取映射配置
        /// </summary>
        private static TypeMap GetMap(Type sourceType, Type destinationType, IEnumerable<string> ignoreMembers = null)
        {
            try
            {
                return Mapper.Configuration.FindTypeMapFor(sourceType, destinationType);
            }
            catch (InvalidOperationException)
            {
                lock (Sync)
                {
                    try
                    {
                        return Mapper.Configuration.FindTypeMapFor(sourceType, destinationType);
                    }
                    catch (InvalidOperationException)
                    {
                        InitMaps(sourceType, destinationType, ignoreMembers);
                    }
                    return Mapper.Configuration.FindTypeMapFor(sourceType, destinationType);
                }
            }
        }

        /// <summary>
        /// 初始化映射配置
        /// </summary>
        private static void InitMaps(Type sourceType, Type destinationType, IEnumerable<string> ignoreMembers = null)
        {
            Action createMap = () => Mapper.Initialize(config => {
                var mapping = config.CreateMap(sourceType, destinationType);
                if (ignoreMembers != null)
                {
                    foreach (var m in ignoreMembers)
                    {
                        mapping.ForMember(m, opts => opts.Ignore());
                    }
                }
            });

            try
            {
                var maps = Mapper.Configuration.GetAllTypeMaps();
                ClearConfig();
                createMap();
                foreach (var map in maps)
                    Mapper.Configuration.RegisterTypeMap(map);
            }
            catch (InvalidOperationException)
            {
                createMap();
            }
        }

        /// <summary>
        /// 清空配置
        /// </summary>
        private static void ClearConfig()
        {
            var typeMapper = typeof(Mapper).GetTypeInfo();
            var configuration = typeMapper.GetDeclaredField("_configuration");
            configuration.SetValue(null, null, BindingFlags.Static, null, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 将源集合映射到目标集合，可指定需要忽略的属性
        /// </summary>
        /// <example>
        ///     如需要忽略 DestModel 中的 A、B 属性，则可以如下调用此方法：
        ///         list.MapToList<DestModel>(
        ///              p => p.A,
        ///              p => p.B
        ///         );
        /// </example>
        /// <typeparam name="TDestination">目标元素类型,范例：Sample,不要加List</typeparam>
        /// <param name="source">源集合</param>
        public static List<TDestination> MapToList<TDestination>(this System.Collections.IEnumerable source, params Expression<Func<TDestination, object>>[] ignoreMemberSelectors)
             where TDestination : new()
        {
            var iterator = source.GetEnumerator();
            var list = new List<TDestination>();
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                var target = o.MapTo(ignoreMemberSelectors);
                list.Add(target);
            }

            return list;
        }
    }
} 
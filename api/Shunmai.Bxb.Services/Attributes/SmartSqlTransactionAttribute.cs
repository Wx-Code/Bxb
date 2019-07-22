using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using SmartSql;
using SmartSql.Exceptions;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Services.Attributes
{
    /// <summary>
    /// 自定义 AOP 事务特性类
    /// </summary>
    /// <example>
    ///     [SmartSqlTransaction]
    ///     public bool Method(...)
    ///     {
    ///     }
    /// </example>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SmartSqlTransactionAttribute : AbstractInterceptorAttribute
    {
        public string Alias { get; set; } = SmartSqlBuilder.DEFAULT_ALIAS;
        public IsolationLevel Level { get; set; } = IsolationLevel.Unspecified;
        /// <summary>
        /// 指示是否当且仅当方法的返回值为 <see cref="true"/> 时，方才提交事务
        /// </summary>
        public bool CommitWhenReturnTrue { get; set; } = true;

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var sessionStore = context.ServiceProvider.GetSessionStore(Alias);
            if (sessionStore == null)
            {
                throw new SmartSqlException($"can not find SmartSql instance by Alias:{Alias}.");
            }
            var inTransaction = sessionStore.LocalSession?.Transaction != null;
            if (inTransaction)
            {
                await next.Invoke(context); return;
            }

            using (sessionStore)
            using (var dbSession = sessionStore.Open())
            {
                try
                {
                    dbSession.BeginTransaction(Level);
                    await next.Invoke(context);

                    var canCommit = true;
                    if (CommitWhenReturnTrue)
                    {
                        canCommit = context.ServiceMethod.ReturnType == typeof(bool) && true == (bool)context.ReturnValue;
                    }

                    if (canCommit)
                    {
                        dbSession.CommitTransaction();
                    }
                    else
                    {
                        dbSession.RollbackTransaction();
                    }
                }
                catch (Exception ex)
                {
                    dbSession.RollbackTransaction();
                    throw ex;
                }
            }
        }
    }
}

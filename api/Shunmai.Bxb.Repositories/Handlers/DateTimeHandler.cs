using SmartSql.Data;
using SmartSql.Exceptions;
using SmartSql.TypeHandlers;
using System;
using System.Data;

namespace Shunmai.Bxb.Repositories.Handlers
{
    public class DateTimeHandler: AbstractTypeHandler<DateTime, string>
    {
        private object GetSetParameterValue(object datetime)
        {
            if (datetime == null)
            {
                return DBNull.Value;
            }
            if (datetime is DateTime)
            {
                return ((DateTime)datetime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            return datetime.ToString();
        }

        public new void SetParameter(IDataParameter dataParameter, object parameterValue)
        {
            dataParameter.DbType = DbType.DateTime;
            dataParameter.Value = GetSetParameterValue(parameterValue);
        }

        public override DateTime GetValue(DataReaderWrapper dataReader, int columnIndex, Type targetType)
        {
            var strVal = dataReader.GetString(columnIndex);
            if (DateTime.TryParse(strVal, out var datetime))
            {
                return datetime;
            }
            var colName = dataReader.GetName(columnIndex);
            throw new SmartSqlException($"Column Name:{colName} String:{strVal} can not convert to DateTime");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Test.Common.Models;
using Shunmai.Bxb.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Shunmai.Bxb.Test.Common
{
    public class BxbContext : DbContext
    {
        private readonly string _connStr;

        public BxbContext(string connectionString)
        {
            _connStr = connectionString;
        }

        public static BxbContext FromSmartSqlConfig()
        {
            var document = new XmlDocument();
            document.Load(PathHelper.MapPath("SmartSqlMapConfig.xml"));
            var connectionString = document
                    .DocumentElement
                    .GetElementsByTagName("Write")
                    .Item(0)
                    .Attributes["ConnectionString"]
                    .Value;
            return new BxbContext(connectionString);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connStr);
        }

        public void Truncate(string table)
        {
            Database.ExecuteSqlCommand("TRUNCATE TABLE `" + table + "`;");
        }

        public virtual DbSet<UserExt> User { get; set; }
        public virtual DbSet<UserLogExt> UserLog { get; set; }
        public virtual DbSet<SmsLogExt> SmsLog { get; set; }
        public virtual DbSet<SmsCode> SmsVerificationCode { get; set; }
        public virtual DbSet<TradeHallExt> TradeHall { get; set; }
        public virtual DbSet<TradeHallLogExt> TradeHallLog { get; set; }
        public virtual DbSet<TradeOrderExt> TradeOrder { get; set; }
        public virtual DbSet<TradeOrderLogExt> TradeOrderLog { get; set; }
    }
}

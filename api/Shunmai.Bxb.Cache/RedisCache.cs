using Newtonsoft.Json;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Cache.Exceptions;
using Shunmai.Bxb.Utilities.Check;
using Shunmai.Bxb.Utilities.Extenssions;
using Shunmai.Bxb.Utilities.Helpers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Shunmai.Bxb.Cache
{
    public class RedisCache : ICache
    {
        private const string DEFAULT_CONFIG_FILE_NAME = "RedisCacheConfig.xml";
        private readonly IDatabase _db;

        public RedisCache() : this(DEFAULT_CONFIG_FILE_NAME) { }

        public RedisCache(string configFile)
        {
            var filename = configFile ?? DEFAULT_CONFIG_FILE_NAME;
            var cacheFileName = PathHelper.MapPath(filename);
            if (!File.Exists(cacheFileName))
            {
                throw new FileNotFoundException("Redis config file not found", filename);
            }

            var xml = new XmlDocument();
            xml.Load(cacheFileName);

            var node = xml.SelectSingleNode("//dataSource");
            var connectionString = node?.Attributes["connectionString"]?.Value;
            if (connectionString.IsEmpty())
            {
                throw new ConnectionStringException("Redis connection string not found on node 'dataSource'");
            }

            //var success = ThreadPool.SetMinThreads(32, 32);
            //if (!success)
            //{

            //}

            var connector = ConnectionMultiplexer.Connect(connectionString);
            AddRegisterEvent(connector);  //添加注册事件
            _db = connector.GetDatabase();
        }

        #region 注册事件

        /// <summary>
        /// 添加注册事件
        /// </summary>
        private static void AddRegisterEvent(IConnectionMultiplexer connector)
        {
            connector.ConnectionRestored += ConnMultiplexer_ConnectionRestored;
            connector.ConnectionFailed += ConnMultiplexer_ConnectionFailed;
            connector.ErrorMessage += ConnMultiplexer_ErrorMessage;
            connector.ConfigurationChanged += ConnMultiplexer_ConfigurationChanged;
            connector.HashSlotMoved += ConnMultiplexer_HashSlotMoved;
            connector.InternalError += ConnMultiplexer_InternalError;
            connector.ConfigurationChangedBroadcast += ConnMultiplexer_ConfigurationChangedBroadcast;
        }

        /// <summary>
        /// 重新配置广播时（通常意味着主从同步更改）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChangedBroadcast(object sender, EndPointEventArgs e)
        {

        }

        /// <summary>
        /// 发生内部错误时（主要用于调试）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_InternalError(object sender, InternalErrorEventArgs e)
        {
            throw e.Exception;
        }

        /// <summary>
        /// 更改集群时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {

        }

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChanged(object sender, EndPointEventArgs e)
        {

        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ErrorMessage(object sender, RedisErrorEventArgs e)
        {

        }

        /// <summary>
        /// 物理连接失败时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {

        }

        /// <summary>
        /// 建立物理连接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {

        }

        #endregion 注册事件  


        #region string操作
        public virtual bool Set(string key, object data, TimeSpan? expires)
        {
            Check.Empty(key, nameof(key));

            var json = JsonConvert.SerializeObject(data);
            return _db.StringSet(key, json, expires);
        }

        public virtual string Get(string key)
        {
            Check.Empty(key, nameof(key));

            string value = _db.StringGet(key);
            if (value.IsEmpty())
            {
                return null;
            }
            return value;
        }

        public virtual T Get<T>(string key)
        {
            Check.Empty(key, nameof(key));

            string value = _db.StringGet(key);
            if (value.IsEmpty())
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public virtual bool Remove(string key)
        {
            Check.Empty(key, nameof(key));

            return _db.KeyDelete(key);
        }

        #endregion

        #region List 操作

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public string ListLeftPop(string redisKey)
        {
            return _db.ListLeftPop(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public string ListRightPop(string redisKey)
        {
            return _db.ListRightPop(redisKey);
        }

        /// <summary>
        /// 移除列表指定键上与该值相同的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListRemove(string redisKey, string redisValue)
        {
            return _db.ListRemove(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListRightPush(string redisKey, string redisValue)
        {
            return _db.ListRightPush(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListLeftPush(string redisKey, string redisValue)
        {
            return _db.ListLeftPush(redisKey, redisValue);
        }

        /// <summary>
        /// 返回列表上该键的长度，如果不存在，返回 0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public long ListLength(string redisKey)
        {
            return _db.ListLength(redisKey);
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public IEnumerable<string> ListRange(string redisKey)
        {
            RedisValue[] list = _db.ListRange(redisKey);

            foreach (RedisValue item in list)
            {
                yield return item.ToString();
            }
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string redisKey)
        {
            return Deserialize<T>(_db.ListLeftPop(redisKey));
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public T ListRightPop<T>(string redisKey)
        {
            return Deserialize<T>(_db.ListRightPop(redisKey));
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListRightPush<T>(string redisKey, T redisValue)
        {
            return _db.ListRightPush(redisKey, Serialize(redisValue));
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListLeftPush<T>(string redisKey, T redisValue)
        {
            return _db.ListLeftPush(redisKey, Serialize(redisValue));
        }

        #region List-async

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> ListLeftPopAsync(string redisKey)
        {
            return await _db.ListLeftPopAsync(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> ListRightPopAsync(string redisKey)
        {
            return await _db.ListRightPopAsync(redisKey);
        }

        /// <summary>
        /// 移除列表指定键上与该值相同的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRemoveAsync(string redisKey, string redisValue)
        {
            return await _db.ListRemoveAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync(string redisKey, string redisValue)
        {
            return await _db.ListRightPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync(string redisKey, string redisValue)
        {
            return await _db.ListLeftPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 返回列表上该键的长度，如果不存在，返回 0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string redisKey)
        {
            return await _db.ListLengthAsync(redisKey);
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RedisValue>> ListRangeAsync(string redisKey)
        {
            return await _db.ListRangeAsync(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string redisKey)
        {
            return Deserialize<T>(await _db.ListLeftPopAsync(redisKey));
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string redisKey)
        {
            return Deserialize<T>(await _db.ListRightPopAsync(redisKey));
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync<T>(string redisKey, T redisValue)
        {
            return await _db.ListRightPushAsync(redisKey, Serialize(redisValue));
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync<T>(string redisKey, T redisValue)
        {
            return await _db.ListLeftPushAsync(redisKey, Serialize(redisValue));
        }

        #endregion List-async

        #endregion List 操作


        public bool SetAdd(string key, string data)
        {
            Check.Empty(key, nameof(key));

            return _db.SetAdd(key, data);
        }

        public long SetLength(string redisKey)
        {
            return _db.SetLength(redisKey);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string Serialize(object obj)
        {
            if (obj == null)
                return null;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static T Deserialize<T>(string data)
        {
            if (data == null)
                return default(T);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
        }
    }
}

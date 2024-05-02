//using System.Net.Http.Json;
//using System.Text;

//namespace Cache.DisributedMemoryRedis.Helpers
//{
//    public class RedisCacheManager : IDistributedCacheManager
//    {
//        private readonly RedisClient _redisClient;

//        public RedisCacheManager(RedisClient redisServer)
//        {
//            this._redisClient = redisServer;
//        }

//        public T Get<T>(string key)
//        {
//            var utf8String = Encoding.UTF8.GetString(Get(key));
//            var result = JsonContent.DeserializeObject<T>(utf8String);
//            return result;
//        }

//        public byte[] Get(string key)
//        {
//            return _redisClient.RedisCache.Get(key);
//        }

//        public void Set(string key, object value)
//        {
//            var serializedObject = JsonConvert.SerializeObject(value);
//            var utf8String = Encoding.UTF8.GetBytes(serializedObject);
//            _redisClient.RedisCache.Set(key, utf8String);
//        }

//        public void Refresh(string key)
//        {
//            _redisClient.RedisCache.Refresh(key);
//        }

//        public bool Any(string key)
//        {
//            return _redisClient.RedisCache.Get(key) != null;
//        }

//        public void Remove(string key)
//        {
//            _redisClient.RedisCache.Remove(key);
//        }
//    }
//}

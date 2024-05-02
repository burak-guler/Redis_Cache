using StackExchange.Redis;


namespace Cache.DisributedMemoryRedis.Helpers
{
    public class RedisClient
    {
        ConnectionMultiplexer connectionMultiplexer;
        public void Connect() => connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
        public IDatabase GetDb(int db) => connectionMultiplexer.GetDatabase(db);
    }
}

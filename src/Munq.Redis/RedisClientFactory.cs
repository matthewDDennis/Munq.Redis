namespace Munq.Redis
{
    public class RedisClientFactory
    {
        public RedisClient Create(RedisClientConfig config)
        {
            return new RedisClient(new RedisSocketConnection(config));
        }
    }
}

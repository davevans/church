using System.Collections;
using System.Collections.Generic;
using Church.Common.Logging;
using Church.Common.Settings;
using ServiceStack.Redis;

namespace Church.Common.Cache
{
    public class RedisCache : ICache
    {
        private readonly ILogWriter _debug;
        private readonly RedisSettings _settings;
        public RedisCache(ISettingsProvider settingsProvider, ILogger logger)
        {
            _debug = logger.With(GetType(), LogLevel.Debug);
            _settings = settingsProvider.GetSetting<RedisSettings>();
        }

        private RedisClient GetClient()
        {
            return new RedisClient(_settings.Host, _settings.Port);
        }

        public T Get<T>(object id)
        {
            using (IRedisClient client = GetClient())
            {
                return client.GetById<T>(id);
            }
        }

        public IList<T> GetMany<T>(ICollection ids)
        {
            using (IRedisClient client = GetClient())
            {
                return client.GetByIds<T>(ids);
            }
        }

        public T Set<T>(T obj)
        {
            using (IRedisClient client = new RedisClient())
            {
                return client.Store(obj);
            }
        }

        public void SetMany<T>(IList<T> objs)
        {
            using (IRedisClient client = new RedisClient())
            {
                client.StoreAll(objs);
            }
        }

        public void DeleteAll<T>()
        {
            using (IRedisClient client = new RedisClient())
            {
                client.DeleteAll<T>();
            }
        }
    }
}

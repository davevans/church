using Church.Common.Settings;

namespace Church.Common.Cache
{
    public class RedisSettings : ISetting
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}

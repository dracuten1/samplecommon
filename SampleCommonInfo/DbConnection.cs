using Newtonsoft.Json;

namespace SampleCommonInfo
{
    public static class GlobalResouce{
        public static DbConnection DbConnection { get; } = new DbConnection
        {
            DataBase = "etl",
            Host = "db-etl.yedda.link",
            Port = 5432,
            Username = "wigo",
            Password = "s8uZSCeHhUuxC1BGWl6j"
        };
    }
    public class DbConnection
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("engine")]
        public string Engine { get; set; }
        [JsonProperty("host")]
        public string Host { get; set; }
        [JsonProperty("port")]
        public int Port { get; set; }
        [JsonProperty("dbname")]
        public string DataBase { get; set; }
        [JsonProperty("dbInstanceIdentifier")]
        public string DBInstanceIdentifier { get; set; }

        public int MinumumPoolSize { get; set; } = 5;
        public int MaximumPoolSize { get; set; } = 30;
        public int ConnectionIdleLifetime { get; set; } = 300;
        [JsonProperty("schema")] public string DefaultSchema { get; set; } = "public";
        public override string ToString()
        {
            return $"Host={Host};" +
                   $"Database={DataBase};" +
                   $"Username={Username};" +
                   $"Password='{Password}';" +
                   $"Minimum Pool Size={MinumumPoolSize};" +
                   $"Maximum Pool Size={MaximumPoolSize};" +
                   $"Connection Idle Lifetime={ConnectionIdleLifetime}";
        }
    }
}

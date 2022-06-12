using System;

namespace Hangfire.Helper
{
    public class HangfireConfig
    {
        public static readonly string Title = "HangfireConfig";
        public string? RealmDbPath { get; set; }
        public string? AzureDB { get; set; }
        public string? AzureDocument { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? AzureCosmosUrl { get; set; }
        public string? AzureCosmosSecret { get; set; }
        public string? AzureCosmosDbName { get; set; }
        public string? AzureCosmosCollectionName { get; set; }
        public string? MongoDBUrl { get; set; }
        public string? MongoDBName { get; set; }
        public bool MongoDBCheckConnection { get; set; }
        public string? MongoDBPrefix { get; set; }
        public string? DashboardPath { get; set; }

        public string? PostgreSqlConnectionString { get; set; }
        public string? SQLServerConnectionString { get; set; }
        public string? RedisConnectionString { get; set; }

    }
}

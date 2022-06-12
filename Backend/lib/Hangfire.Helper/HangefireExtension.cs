using System;
using Hangfire;
using HGLib = Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.CosmosDB;
using Hangfire.PostgreSql;
using Npgsql;
using Hangfire.Pro.Redis;
using Hangfire.Tags.Pro.Redis;

namespace Hangfire.Helper
{
 
    public static class HangefireExtension
    {
        private static IConfiguration Configuration { get; set; }
        private static HangfireConfig HangfireConfig { get; set; }
        private const string defaultUsername = "admin";
        private const string defaultPassword = "admin";


        public static IServiceCollection AddHangfireWithSQLServer(this IServiceCollection services, IConfiguration configuration)
        {
            Configuration = configuration;
            HangfireConfig = Configuration.GetSection(HangfireConfig.Title).Get<HangfireConfig>();

            services.AddHangfire(x => x.UseSqlServerStorage(HangfireConfig.SQLServerConnectionString, new HGLib.SqlServer.SqlServerStorageOptions() { SchemaName = "hangifre" }));
            return services;
        }

        public static IServiceCollection AddHangfireWithPostgreSQL(this IServiceCollection services, IConfiguration configuration)
        {
            //var filter = new SubmitExamCounterAttribute();
            //Configuration = configuration;
            HangfireConfig = Configuration.GetSection(HangfireConfig.Title).Get<HangfireConfig>();
            services.AddHangfire((config) => config
 
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSerializerSettings(new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })
                .UsePostgreSqlStorage(HangfireConfig.PostgreSqlConnectionString, new PostgreSqlStorageOptions() { SchemaName = "hangfire" })
            );
            return services;
        }

        public static IServiceCollection AddHangfireWithMongoDB(this IServiceCollection services, IConfiguration configuration)
        {

            Configuration = configuration;
            HangfireConfig = Configuration.GetSection(HangfireConfig.Title).Get<HangfireConfig>();

            var mongUrlBuilder = new MongoUrlBuilder(HangfireConfig.MongoDBUrl);
            var MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new DropMongoMigrationStrategy(),
                BackupStrategy = new NoneMongoBackupStrategy()
            };

            MongoClient mongoClient = new MongoClient(mongUrlBuilder.ToMongoUrl());


            services.AddHangfire(config => config
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               
               .UseSerializerSettings(new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })
               .UseMongoStorage(mongoClient, HangfireConfig.MongoDBName, new MongoStorageOptions
               {
                   MigrationOptions = MigrationOptions,
                   InvisibilityTimeout = TimeSpan.FromMinutes(5),
                   JobExpirationCheckInterval = TimeSpan.FromMinutes(5),
                   Prefix = HangfireConfig.MongoDBPrefix,
                   CheckConnection = HangfireConfig.MongoDBCheckConnection
               })
               );

            return services;
        }

        public static IServiceCollection AddHangfireWithCosmosMongo(this IServiceCollection services, IConfiguration configuration)
        {

            Configuration = configuration;
            HangfireConfig = Configuration.GetSection(HangfireConfig.Title).Get<HangfireConfig>();

            services.AddHangfire(config =>
            {  
                var mongoUrlBuilder = new MongoUrlBuilder(HangfireConfig.MongoDBUrl) { DatabaseName = HangfireConfig.MongoDBName };
                var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());
                var options = new CosmosStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        BackupStrategy = new NoneMongoBackupStrategy(),
                        MigrationStrategy = new DropMongoMigrationStrategy(),
                    },
                    InvisibilityTimeout = TimeSpan.FromMinutes(5),
                    JobExpirationCheckInterval = TimeSpan.FromMinutes(5),
                    Prefix = HangfireConfig.MongoDBPrefix,
                    CheckConnection = HangfireConfig.MongoDBCheckConnection,
                    QueuePollInterval = TimeSpan.FromSeconds(15)
                };
                config.UseColouredConsoleLogProvider(HGLib.Logging.LogLevel.Info);
                config.UseCosmosStorage(mongoClient, mongoUrlBuilder.DatabaseName, options);
            });
            return services;
        }

        public static IServiceCollection AddHangfireWithRedis(this IServiceCollection services, IConfiguration configuration)
        {
            Configuration = configuration;
            HangfireConfig = Configuration.GetSection(HangfireConfig.Title).Get<HangfireConfig>();
            services.AddHangfire(config => {
                RedisStorageOptions options = new RedisStorageOptions
                {
                    Prefix = "hangfire"
                };
                config.UseRedisStorage(HangfireConfig.RedisConnectionString, options);
                config.UseTagsWithRedis();
            });
           
            return services;
        }

        public static IServiceCollection AddEncourseHangfire(this IServiceCollection services, IConfiguration configuration, string machinePrefix) {

            string hangfireType = configuration["HangfireConfig:Type"].ToLower();

            if (hangfireType == HangfireSupportType.CosmosMongo.ToLower())
            {
                services.AddHangfireWithCosmosMongo(configuration);
            }
            else if (hangfireType == HangfireSupportType.Mongo.ToLower())
            {
                services.AddHangfireWithMongoDB(configuration);
            }
            else if (hangfireType == HangfireSupportType.SQLServer.ToLower())
            {
                services.AddHangfireWithSQLServer(configuration);
            }
            else if (hangfireType == HangfireSupportType.PostgreSQL.ToLower())
            {
                services.AddHangfireWithPostgreSQL(configuration);
            }
            else if (hangfireType == HangfireSupportType.Redis.ToLower())
            {
                services.AddHangfireWithRedis(configuration);
            }

            if (string.IsNullOrEmpty(machinePrefix)) {
                throw new Exception("machine prefix cannot null or empty");
            }

            QueueName.MachinePrefix = machinePrefix;
            return services;
        }

        public static IServiceCollection AddEncourseHangfireServer(this IServiceCollection services, string[] queues = null, int workerCount = 0)
        {

            services.AddHangfireServer(ops => {
                ops.Queues = queues;
                if (workerCount != 0) ops.WorkerCount = workerCount;
            });

            return services;
        }

        public static IApplicationBuilder UseEncourseHangfireDashboard(this IApplicationBuilder app)
        {
           

            var basicAuth = new BasicAuthAuthorizationUser()
            {
                Login = string.IsNullOrEmpty(HangfireConfig?.Username) ? defaultUsername : HangfireConfig.Username,
                PasswordClear = string.IsNullOrEmpty(HangfireConfig?.Password) ? defaultPassword : HangfireConfig.Password
            };

            var opts = new DashboardOptions
            {
                Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                {
                    RequireSsl = false,
                    SslRedirect = false,
                    LoginCaseSensitive = true,
                    Users = new [] { basicAuth }
                }) }
            };

            string dashboardPath = string.IsNullOrEmpty(HangfireConfig.DashboardPath) ? "/hangfire" : HangfireConfig.DashboardPath;
            app.UseHangfireDashboard(dashboardPath, opts);
            return app;
        }


        //[Obsolete]
        //public static IApplicationBuilder UseHangfire(this IApplicationBuilder app, BackgroundJobServerOptions op = null)
        //{
        //    var basicAuth = new BasicAuthAuthorizationUser()
        //    {
        //        Login = string.IsNullOrEmpty(HangfireConfig?.Username) ? defaultUsername : HangfireConfig.Username,
        //        PasswordClear = string.IsNullOrEmpty(HangfireConfig?.Password) ? defaultPassword : HangfireConfig.Password
        //    };

        //    var opts = new DashboardOptions
        //    {
        //        Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
        //        {
        //            RequireSsl = false,
        //            SslRedirect = false,
        //            LoginCaseSensitive = true,
        //            Users = new [] { basicAuth }
        //        }) }
        //    };

        //    app.UseHangfireServer(op);
        //    app.UseHangfireDashboard("/hangfire", opts);
        //    return app;
        //}




        //[Obsolete]
        //public static IApplicationBuilder UseEncourseHangfireServer(this IApplicationBuilder app, BackgroundJobServerOptions op = null)
        //{
        //    app.UseHangfireServer(op);
        //    return app;
        //}
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        private static string _connectionString;

        public DatabaseContext CreateDbContext()
        {
            return CreateDbContext(null);
        }

        public DatabaseContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.Local.DB.json")
                    .Build();

            if (args != null && args.Length > 0)
            {
                _connectionString = args[0];
            }
            else
            {
                _connectionString = config["DBConnectionString"];

                if (string.IsNullOrEmpty(_connectionString))
                {
                    LoadConnectionString();
                }
            }

            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseNpgsql(_connectionString);

            return new DatabaseContext(builder.Options);
        }

        private static void LoadConnectionString()
        {
            // _connectionString = "";
        }
    }
}

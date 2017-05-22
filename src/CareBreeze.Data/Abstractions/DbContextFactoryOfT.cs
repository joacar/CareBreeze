using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;

namespace CareBreeze.Data
{
    public abstract class DbContextFactory<T> : IDbContextFactory<T> where T : DbContext
    {
        public string BasePath { get; protected set; }

        public T Create()
        {
            var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment");

            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName);
        }

        public T Create(DbContextFactoryOptions options)
            => Create(options.ContentRootPath, options.EnvironmentName);

        private T Create(string basePath, string environmentName)
        {
            BasePath = basePath;
            var configuration = Configuration(basePath, environmentName);
            var connectionString = ConnectionString(configuration.Build());
            return Create(connectionString);
        }

        private T Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"{nameof(connectionString)} is null or empty",
                      nameof(connectionString));
            }
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            return Configure(connectionString, optionsBuilder);
        }

        protected virtual IConfigurationBuilder Configuration(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();
            return builder;
        }

        protected abstract string ConnectionString(IConfigurationRoot configuration);

        protected abstract T Configure(string connectionString, DbContextOptionsBuilder<T> builder);


        T IDbContextFactory<T>.Create(DbContextFactoryOptions options)
            => Create(options.ContentRootPath, options.EnvironmentName);
    }
}

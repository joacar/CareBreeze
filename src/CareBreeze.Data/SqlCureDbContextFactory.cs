using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace CareBreeze.Data
{
    public class SqlCureDbContextFactory : DbContextFactory<CareBreezeDbContext>
    {
        protected override CareBreezeDbContext Configure(string connection, DbContextOptionsBuilder<CareBreezeDbContext> builder)
        {
            builder.UseSqlServer(connection);
            return new CareBreezeDbContext(builder.Options);
        }

        protected override string ConnectionString(IConfigurationRoot configuration)
        {
            return configuration.GetConnectionString("CareBreeze");
        }
    }
}

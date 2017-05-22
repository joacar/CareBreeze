using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CareBreeze.Data.Tests
{
    public class MockCareBreezeDbContextFactory : DbContextFactory<CareBreezeDbContext>
    {
        protected override CareBreezeDbContext Configure(string connection, DbContextOptionsBuilder<CareBreezeDbContext> builder)
        {
            builder.UseSqlServer(connection, o => o.MigrationsAssembly("CareBreeze.Data"));
            return new CareBreezeDbContext(builder.Options);
        }

        protected override string ConnectionString(IConfigurationRoot configuration)
        {
            return configuration.GetConnectionString("CareBreeze");
        }
    }
}

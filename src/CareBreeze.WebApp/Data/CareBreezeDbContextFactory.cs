using CareBreeze.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CareBreeze.WebApp
{
    public class CareBreezeDbContextFactory : DbContextFactory<CareBreezeDbContext>
    {
        protected override CareBreezeDbContext Configure(string connection, DbContextOptionsBuilder<CareBreezeDbContext> builder)
        {
            builder.UseSqlServer(connection, options => options.MigrationsAssembly("CareBreeze.Data"));
            return new CareBreezeDbContext(builder.Options);
        }

        protected override string ConnectionString(IConfigurationRoot configuration)
        {
            return configuration.GetConnectionString("CareBreeze");
        }
    }
}

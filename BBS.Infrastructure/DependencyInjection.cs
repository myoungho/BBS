using BBS.Domain.Repositories;
using BBS.Infrastructure.Data;
using BBS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BBS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<BbsContext>(options =>
                options.UseInMemoryDatabase("Bbs"));
        }
        else
        {
            services.AddDbContext<BbsContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}

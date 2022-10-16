using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace TreeStructureAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        public static void ConfigureSQLContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(opts =>
            {
                opts.UseNpgsql(configuration.GetConnectionString("sqlConnection"), builder =>
                {
                    builder.MigrationsAssembly("TreeStructureAPI");
                });
            });
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<INodeRepository, NodeRepository>();
        }
    }
}

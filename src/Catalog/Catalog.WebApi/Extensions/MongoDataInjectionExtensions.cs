using Catalog.Application.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Data.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.WebApi.Extensions
{
    public static class MongoDataInjectionExtensions
    {
        public static IServiceCollection AddMongo(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MongoContextOptions>(configuration.GetSection(nameof(MongoContextOptions)));

            services.AddTransient<MongoContext>();

            services.AddTransient<IProductRepository, MongoProductRepository>();

            return services;
        }
    }
}

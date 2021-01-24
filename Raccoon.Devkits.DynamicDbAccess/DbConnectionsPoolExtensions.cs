using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess
{
    public static class DbConnectionsPoolExtensions
    {
        public static IServiceCollection AddDbConnectionsPool(this IServiceCollection services,string connectionString,int maxConnections=64)
        {
            services.TryAddSingleton<DefaultObjectPoolProvider>();
            services.TryAddSingleton<ObjectPool<IDbConnection>>(serviceProvider =>
            {
                var provider = serviceProvider.GetRequiredService<DefaultObjectPoolProvider>();
                provider.MaximumRetained = maxConnections;
                var policy = new DbConnectionPooledObjectPolicy(connectionString);
                return provider.Create(policy);
            });
            return services;
        }
    }
}

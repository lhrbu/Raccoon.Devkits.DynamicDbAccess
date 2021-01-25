using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess
{
    public static class DynamicDbAccessExtensions
    {
        public static IServiceCollection AddDynamicAccessService<TDynamicDbAccessService>(
            this IServiceCollection services,
            string connectionString,
            Func<string, IDbConnection> connectionHandler,
            int maxConnections = 64)
            where TDynamicDbAccessService:DynamicDbAccessService
        {
            services.AddDbConnectionsPool<TDynamicDbAccessService>(connectionString, connectionHandler, maxConnections);
            services.TryAddScoped<EntityTypeLoader>();
            services.TryAddScoped<TDynamicDbAccessService>();
            return services;
        }
    }
}

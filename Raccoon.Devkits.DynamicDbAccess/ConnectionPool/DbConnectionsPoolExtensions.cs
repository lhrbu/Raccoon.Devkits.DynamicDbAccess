using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess.ConnectionPool
{
    internal static class DbConnectionsPoolExtensions
    {
        public static IServiceCollection AddDbConnectionsPool<TDynamicDbAccessService>(
            this IServiceCollection services,
            string connectionString,
            Func<string, IDbConnection> connectHandler,
            int maxConnections = 64)
            where TDynamicDbAccessService : DynamicDbAccessService
        {
            services.TryAddSingleton(serviceProvider =>
            {
                //var provider = serviceProvider.GetRequiredService<DefaultObjectPoolProvider>();
                // provider.MaximumRetained = maxConnections;
                DbConnectionPooledObjectPolicy policy = new(connectionString, connectHandler);
                return new DbConnectionPool<TDynamicDbAccessService>(policy, maxConnections);

            });
            return services;
        }

        //public static DbConnectionsPool<TDynamicDbAccessService> CreatePool<TDynamicDbAccessService>(DbConnectionPooledObjectPolicy policy)
        //{
        //    if (policy == null)
        //    {
        //        throw new ArgumentNullException("policy");
        //    }

        //    if (typeof(IDisposable).IsAssignableFrom(typeof(TDynamicDbAccessService)))
        //    {
        //        return new DisposableObjectPool<T>(policy, MaximumRetained);
        //    }

        //    return new DefaultObjectPool<T>(policy, MaximumRetained);
        //}
    }
}

using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess.ConnectionPool
{
    public class DbConnectionPool<TDynamicDbAccessService> : DefaultObjectPool<IDbConnection>
        where TDynamicDbAccessService : DynamicDbAccessService
    {
        public DbConnectionPool(DbConnectionPooledObjectPolicy policy)
            : base(policy) { }
        public DbConnectionPool(DbConnectionPooledObjectPolicy policy, int maximumRetained)
            : base(policy, maximumRetained) { }
    }
}

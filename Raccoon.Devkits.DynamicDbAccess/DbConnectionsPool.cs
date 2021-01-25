using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess
{
    public class DbConnectionsPool<TDynamicDbAccessService>:DefaultObjectPool<IDbConnection>
    {
        public DbConnectionsPool(DbConnectionPooledObjectPolicy<TDynamicDbAccessService> policy)
            : base(policy) { }
        public DbConnectionsPool(DbConnectionPooledObjectPolicy<TDynamicDbAccessService> policy, int maximumRetained)
            :base(policy, maximumRetained){ }
    }
}

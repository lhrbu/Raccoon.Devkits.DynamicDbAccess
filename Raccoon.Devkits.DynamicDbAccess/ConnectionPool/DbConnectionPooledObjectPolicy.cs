using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess.ConnectionPool
{
    public class DbConnectionPooledObjectPolicy : PooledObjectPolicy<IDbConnection>
    {
        private readonly string _connectionString;
        private readonly Func<string, IDbConnection> _connectHandler;
        public DbConnectionPooledObjectPolicy(
            string connectionString,
            Func<string, IDbConnection> connectHandler)
        {
            _connectionString = connectionString;
            _connectHandler = connectHandler;
        }
        public override IDbConnection Create() => _connectHandler.Invoke(_connectionString);

        public override bool Return(IDbConnection obj) => true;
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess
{
    public class DbConnectionPooledObjectPolicy : PooledObjectPolicy<IDbConnection>
    {
        private readonly string _connectionString;
        public DbConnectionPooledObjectPolicy(string connectionString)
        { _connectionString = connectionString; }
        public override IDbConnection Create() => new NpgsqlConnection(_connectionString);

        public override bool Return(IDbConnection obj) => true;
    }
}

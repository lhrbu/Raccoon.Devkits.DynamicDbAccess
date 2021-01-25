using Microsoft.Extensions.ObjectPool;
using Raccoon.Devkits.DynamicDbAccess.ConnectionPool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess.UnitTest
{
    public class DefaultDbDynamicAccessService:DynamicDbAccessService
    {
        public DefaultDbDynamicAccessService(
            DbConnectionPool<DefaultDbDynamicAccessService> connectionsPool) : base(connectionsPool) { }
    }
}

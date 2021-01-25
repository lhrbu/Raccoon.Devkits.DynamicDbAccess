using Microsoft.Extensions.ObjectPool;
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
            ObjectPool<IDbConnection> connectionsPool,
            EntityTypeLoader typeLoader) : base(connectionsPool, typeLoader) { }
    }
}

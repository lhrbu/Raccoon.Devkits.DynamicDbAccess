using Raccoon.Devkits.DynamicDbAccess.ConnectionPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess.TestWebAPIServer
{
    public class WebAPIDynamicDbAccessService:DynamicDbAccessService
    {
        public WebAPIDynamicDbAccessService(DbConnectionPool<WebAPIDynamicDbAccessService> connectionsPool) 
            : base(connectionsPool) { }
    }
}

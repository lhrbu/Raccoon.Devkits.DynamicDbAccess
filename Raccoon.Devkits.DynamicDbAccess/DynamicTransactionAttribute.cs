using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DynamicTransactionAttribute:Attribute
    {
        public IsolationLevel IsolationLevel { get; }
        public DynamicTransactionAttribute(IsolationLevel isolationLevel=IsolationLevel.Unspecified)
        { IsolationLevel = IsolationLevel; }
    }
}

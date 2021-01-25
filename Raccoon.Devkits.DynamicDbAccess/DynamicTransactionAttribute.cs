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
        public Type DynamicDbAccessServiceType { get; }
        public IsolationLevel IsolationLevel { get; }
        public DynamicTransactionAttribute(
            Type dynamicDbAccessServiceType,
            IsolationLevel isolationLevel=IsolationLevel.Unspecified)
        {
            DynamicDbAccessServiceType = dynamicDbAccessServiceType;
            IsolationLevel = IsolationLevel; 
        }
    }
}

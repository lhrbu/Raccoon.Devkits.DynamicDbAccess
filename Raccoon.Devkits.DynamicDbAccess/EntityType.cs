using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess
{
    public record EntityType(string AssemblyFile,string Namespace,string Name)
    {
        public string FullName => $"{Namespace}.{Name}";
    }
}

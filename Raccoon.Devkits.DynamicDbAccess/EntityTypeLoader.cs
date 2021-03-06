﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess
{
    public static class EntityTypeLoader
    {
        public static Type Get(EntityType entityType)
        {
            if(!_loadedTypesDict.ContainsKey(entityType.FullName))
            {
                Type type = Assembly.LoadFrom(entityType.AssemblyFile)!
                    .GetType(entityType.FullName, true)!;
                if (_loadedTypesDict.TryAdd(entityType.FullName, type)) { return type; }
                else { throw new InvalidOperationException($"Failed to add pair({entityType.FullName},{type})."); }
                
            }
            else { return _loadedTypesDict[entityType.FullName]; }
        }

        public static Type? SearchInCache(string fullName)
        {
            _loadedTypesDict.TryGetValue(fullName, out Type? type);
            return type;
        }
        private static readonly ConcurrentDictionary<string, Type> _loadedTypesDict = new();
    }
}

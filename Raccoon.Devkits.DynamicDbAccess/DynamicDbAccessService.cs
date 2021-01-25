using Dapper.Contrib.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess
{
    public abstract class DynamicDbAccessService:IDisposable
    {
        private readonly ObjectPool<IDbConnection> _connectionsPool;
        private readonly IDbConnection _connection;
        public Guid Id { get; } = Guid.NewGuid();
        public DynamicDbAccessService(
            ObjectPool<IDbConnection> connectionsPool)
        {
            _connectionsPool = connectionsPool;
            _connection = _connectionsPool.Get();
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if(_connection.State is not ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection.BeginTransaction(isolationLevel);
        }
        public void Dispose()
        {
            _connectionsPool.Return(_connection);
        }

        public async ValueTask<object> GetByIdAsync(dynamic id,EntityType entityType)
        {
            Type type = EntityTypeLoader.Get(entityType);
            MethodInfo methodInfo = GetGenericMethodInfoFromSqlMapper("GetAsync", type);
            dynamic result = methodInfo.Invoke(null, new object?[] { _connection, id, null, null })!;
            return await result;
        }

        public async ValueTask<IEnumerable<object>> GetAllAsync(EntityType entityType)
        {
            Type type = EntityTypeLoader.Get(entityType);
            MethodInfo methodInfo = GetGenericMethodInfoFromSqlMapper("GetAllAsync", type);
            dynamic result = methodInfo.Invoke(null, new[] { _connection, null, null })!;
            return ((await result) as IEnumerable<object>)!;
        }

        public async ValueTask<int> PostAsync(JsonElement entity,EntityType entityType,IDbTransaction? transaction=null)
        {
            Type type = EntityTypeLoader.Get(entityType);
            MethodInfo methodInfo = GetGenericMethodInfoFromSqlMapper("InsertAsync", type);
            object tentity = await DeserializeJsonElementAsync(entity, type);
            dynamic result = methodInfo.Invoke(null, new[] { _connection, tentity, transaction, null, null })!;
            return await result;
        }

        public async ValueTask<int> PostManyAsync(JsonElement entities,EntityType entityType,IDbTransaction? transaction=null)
        {
            Type type = EntityTypeLoader.Get(entityType);
            Type arrayType = type.MakeArrayType();
            MethodInfo methodInfo = GetGenericMethodInfoFromSqlMapper("InsertAsync", arrayType);
            object tentities = await DeserializeJsonElementAsync(entities, arrayType);
            dynamic result = methodInfo.Invoke(null, new[] { _connection, tentities, transaction, null, null })!;
            return await result;
        }

        public async ValueTask<bool> PutAsync(JsonElement entity,EntityType entityType,IDbTransaction? transaction=null)
        {
            Type type = EntityTypeLoader.Get(entityType);
            MethodInfo methodInfo = GetGenericMethodInfoFromSqlMapper("UpdateAsync", type);
            object tentity = await DeserializeJsonElementAsync(entity, type);
            dynamic result = methodInfo.Invoke(null, new[] { _connection, tentity, transaction, null })!;
            return await result;
        }

        public async ValueTask<bool> DeleteAsync(JsonElement entity,EntityType entityType,IDbTransaction? transaction=null)
        {
            Type type = EntityTypeLoader.Get(entityType);
            MethodInfo methodInfo = GetGenericMethodInfoFromSqlMapper("DeleteAsync", type);
            object tentity = await DeserializeJsonElementAsync(entity, type);
            dynamic result = methodInfo.Invoke(null, new[] { _connection, tentity, transaction, null })!;
            return await result;
        }

        private MethodInfo GetGenericMethodInfoFromSqlMapper(string methodName,Type genericType)=>
            typeof(SqlMapperExtensions).GetMethod(methodName,
                BindingFlags.Public | BindingFlags.Static)!.MakeGenericMethod(genericType);

        private async ValueTask<object> DeserializeJsonElementAsync(JsonElement value, Type type)
        {
            ArrayBufferWriter<byte> buffer = new();
            await using Utf8JsonWriter writer = new(buffer);
            value.WriteTo(writer);
            await writer.FlushAsync();
            return (JsonSerializer.Deserialize(buffer.WrittenSpan, type))!;
        }
    }
}

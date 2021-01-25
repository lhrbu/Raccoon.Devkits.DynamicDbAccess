using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Raccoon.Devkits.DynamicDbAccess.UnitTest
{
    public class DynamicDbAccessServiceTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EntityType _entityType = new("EntityTypes/Project.dll",
            "LabCMS.TestReportDomain.EntityAssemblySample", "Project");
        public DynamicDbAccessServiceTest()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDynamicAccessService<DefaultDbDynamicAccessService>("Host=localhost;Username=postgres;Database=usage_records",
                conn => new NpgsqlConnection(conn));
            _serviceProvider = services.BuildServiceProvider();
        }

        public JsonElement CreateRandomJsonElement()
        {
            var obj = new
            {
                no = Guid.NewGuid().ToString(),
                name = Guid.NewGuid().ToString(),
                name_in_fin = Guid.NewGuid().ToString(),
            };
            var str = JsonSerializer.Serialize(obj);
            return JsonDocument.Parse(str).RootElement;
        }

        private JsonElement CreateRandomJsonElements(int count)
        {
            List<object> items = new(count);
            foreach(int index in Enumerable.Range(0, count))
            {
                var obj = new
                {
                    no = Guid.NewGuid().ToString(),
                    name = Guid.NewGuid().ToString(),
                    name_in_fin = Guid.NewGuid().ToString(),
                };
                items.Add(obj);
            }
            var str = JsonSerializer.Serialize(items);
            return JsonDocument.Parse(str).RootElement;
        }

        public DefaultDbDynamicAccessService AccessService => _serviceProvider.GetRequiredService<DefaultDbDynamicAccessService>();
        public Type TestType => _serviceProvider.GetRequiredService<EntityTypeLoader>().Get(_entityType);

        [Fact]
        public async Task GetAsyncTest()
        {
            var obj = new
            {
                no = Guid.NewGuid().ToString(),
                name = Guid.NewGuid().ToString(),
                name_in_fin = Guid.NewGuid().ToString(),
            };
            var str = JsonSerializer.Serialize(obj);
            await AccessService.PostAsync(JsonDocument.Parse(str).RootElement, _entityType);
            dynamic obj2 = await AccessService.GetByIdAsync(obj.no, _entityType);
            Assert.Equal(obj.no, obj2.no);
            Assert.Equal(obj.name, obj2.name);
            Assert.Equal(obj.name_in_fin, obj2.name_in_fin);
        }

        [Fact]
        public async Task GetAllAsyncTest()
        {
            var items = (await AccessService.GetAllAsync(_entityType)).ToArray();
            Type type = items[0].GetType();
            Assert.Equal(_entityType.FullName, type.FullName);
        }

        [Fact]
        public async Task PostAsyncTest()
        {
            int count = await AccessService.PostAsync(CreateRandomJsonElement(), _entityType);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task PostManyAsyncTest()
        {
            int count = await AccessService.PostManyAsync(CreateRandomJsonElements(3), _entityType);
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task PutAsyncTest()
        {
            var obj = new
            {
                no = Guid.NewGuid().ToString(),
                name = Guid.NewGuid().ToString(),
                name_in_fin = Guid.NewGuid().ToString(),
            };
            var str = JsonSerializer.Serialize(obj);
            int count = await AccessService.PostAsync(JsonDocument.Parse(str).RootElement, _entityType);
            var obj2 = new
            {
                no = obj.no,
                name = $"Updated:{DateTimeOffset.Now.ToString()}",
                name_in_fin = obj.name_in_fin
            };
            var str2 = JsonSerializer.Serialize(obj2);
            bool result = await AccessService.PutAsync(JsonDocument.Parse(str2).RootElement, _entityType);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var obj = CreateRandomJsonElement();
            await AccessService.PostAsync(obj, _entityType);
            bool result = await AccessService.DeleteAsync(obj, _entityType);
            Assert.True(result);
        }
    }
}

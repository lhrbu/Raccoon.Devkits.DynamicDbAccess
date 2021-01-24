using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Raccoon.Devkits.DynamicDbAccess.TestWebAPIServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DynamicDbAccessService _accessService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            DynamicDbAccessService accessService)
        {
            _logger = logger;
            _accessService = accessService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost]
        [DynamicTransaction]
        public async Task TestAsync()
        {
            var obj = CreateRandomJsonElement();
            await _accessService.PostAsync(obj, _entityType);
        }

        private readonly EntityType _entityType = new("EntityTypes/Project.dll",
            "LabCMS.TestReportDomain.EntityAssemblySample", "Project");

        private JsonElement CreateRandomJsonElement()
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
    }
}

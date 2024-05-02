using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;

namespace Cache.RedisExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IDistributedCache _distributedCache;

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public WeatherForecastController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public List<string> Get()
        {
            List<string> dataList = new List<string>();
            string serializedCacheData;

            var cacheKey = "summaries";
            var cacheData = _distributedCache.Get(cacheKey);
            if (cacheData != null)
            {
                serializedCacheData = Encoding.UTF8.GetString(cacheData);
                dataList = JsonSerializer.Deserialize<List<string>>(serializedCacheData);
            }
            else
            {
                dataList = Summaries.ToList();
                serializedCacheData = JsonSerializer.Serialize(dataList);
                cacheData = Encoding.UTF8.GetBytes(serializedCacheData);
                _distributedCache.Set(cacheKey, cacheData, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(2),
                });
            }

            return dataList;
        }
    }
}

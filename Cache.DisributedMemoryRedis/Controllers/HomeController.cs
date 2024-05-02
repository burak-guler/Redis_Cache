using Cache.DisributedMemoryRedis.Helpers;
using Cache.DisributedMemoryRedis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Diagnostics;

namespace Cache.DisributedMemoryRedis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private RedisClient _redisClient;

        public HomeController(ILogger<HomeController> logger, RedisClient redisClient)
        {
            _logger = logger;
            _redisClient = redisClient;
        }

        
        public IActionResult Index()
        {
            IDatabase database = _redisClient.GetDb(1);

            database.StringSet("name", "burak");
            
            return View();
        }

        public IActionResult Privacy()
        {
            IDatabase database = _redisClient.GetDb(1);
            string value_get = database.StringGet("name");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

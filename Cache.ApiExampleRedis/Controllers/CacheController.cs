﻿using Cache.ApiExampleRedis.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Cache.ApiExampleRedis.Controllers
{
    [ApiController]
    [Route("api")]
    public class CacheController : Controller
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        [HttpGet("cache")]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(await _cacheService.GetValueAsync(key));
        }

        [HttpPost("cache")]
        public async Task<IActionResult> Post([FromBody] CacheRequestModel model)
        {
            await _cacheService.SetValueAsync(model.Key, model.Value);
            return Ok();
        }
        [HttpDelete("cache")]
        public async Task<IActionResult> Delete(string key)
        {
            await _cacheService.Clear(key);
            return Ok();
        }
    }
}

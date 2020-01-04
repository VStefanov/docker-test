using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyTestApp.Api;
using MyTestApp.Api.Models;
using ServiceStack.Redis;

namespace Api.MyTestApp.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CalculationsController : ControllerBase
    {
     
        private readonly ILogger<CalculationsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly RedisManagerPool redisManager;
        private readonly AppDbContext _context;

        public CalculationsController(ILogger<CalculationsController> logger, IConfiguration configuration, AppDbContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
            var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
            var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");
            redisManager = new RedisManagerPool($"{redisHost}:{redisPort}");
        }

        [HttpGet]
        [Route("Status")]
        public List<string> Status()
        {
            return new List<string>
            {
                "Redis Host:" + Environment.GetEnvironmentVariable("REDIS_HOST"),
                "Redis Port:" + Environment.GetEnvironmentVariable("REDIS_PORT"),
                "Postgres Host:" + Environment.GetEnvironmentVariable("POSTGRES_HOST"),
                "Postgres Port:" + Environment.GetEnvironmentVariable("POSTGRES_PORT"),
                "Postgres User:" + Environment.GetEnvironmentVariable("POSTGRES_USER"),
                "Postgres Pass:" + Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"),
                "Postgres Database:" + Environment.GetEnvironmentVariable("POSTGRES_DATABASE")
            };
        }

        [HttpGet]
        [Route("GetKeyValuePairFromRedis")]
        public Dictionary<string,string> GetKeyValuePairFromRedis()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            using (var client = redisManager.GetClient())
            {
                var currentKeys = client.GetAllKeys();
                foreach (var key in currentKeys)
                {
                    result.Add(key,client.Get<string>(key));
                }
            }

            return result;
        }

        [HttpPost]
        [Route("AddRedisKey")]
        public IActionResult AddRedisKey(DbModel model)
        {
            using (var client = redisManager.GetClient())
            {
                client.Add(model.Index.ToString(), string.Empty);
            }

            return this.Ok();
        }

        [HttpGet]
        [Route("GetPostgreKeys")]
        public List<string> GetPostgreKeys()
        {
            return _context
                 .Indexes
                 .Select(i=>i.Index)
                 .ToList();
               
        }

        [HttpPost]
        [Route("AddPostgreKey")]
        public void AddPostgreKey(DbModel model)
        {
            _context.Add(new IndexModel
            {
                Index = model.Index.ToString()
            });
            _context.SaveChanges();
            _context.Dispose();
        }

     


    }
}

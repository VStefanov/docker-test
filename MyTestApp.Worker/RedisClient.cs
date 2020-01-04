using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Worker.MyTestApp.Worker
{
    public class RedisClient
    {
        private readonly RedisManagerPool redisManager;

        public RedisClient(string host, int port)
        {
            redisManager = new RedisManagerPool($"{host}:{port}");
        }

        
        public List<string> GetNonCalculatedIndexes()
        {
            List<string> result = new List<string>();
            using (var client = redisManager.GetClient())
            {
                var currentKeys = client.GetAllKeys();
                foreach (var key in currentKeys)
                {
                    string value = client.Get<string>(key);
                    if(string.IsNullOrEmpty(value))
                    {
                        result.Add(key);
                    }
                }
            }

            return result;
        }
     
        public void SetValueForIndex(string index,int value)
        {
            using (var client = redisManager.GetClient())
            {
                client.Set(index, value.ToString());
            }
        }
    }
}

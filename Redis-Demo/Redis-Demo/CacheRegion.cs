using System;

namespace Redis_Demo
{
    public class CacheRegion
    {
        public string HashMapName { get; set; }
        public string HashKey { get; set; }

        public CacheRegion(string hashMapName, string hashKey)
        {
            Check.Requires<ArgumentNullException>(!string.IsNullOrEmpty(hashMapName), nameof(hashMapName));
            Check.Requires<ArgumentNullException>(!string.IsNullOrEmpty(hashKey), nameof(hashKey));
            HashMapName = hashMapName;
            HashKey = hashKey;
        }
    }
}

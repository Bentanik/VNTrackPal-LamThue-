namespace VNTrackPal.Infrastructure.Caching;

public class ResponseCacheService
    (IDistributedCache distributedCache,
    IConnectionMultiplexer connectionMultiplexer
    )
    : IResponseCacheService
{

    public async Task DeleteCacheResponseAsync(string cacheKey)
    {
        await distributedCache.RemoveAsync(cacheKey);
    }

    public async Task<string> GetCacheResponseAsync(string cacheKey)
    {
        var cacheResponse = await distributedCache.GetStringAsync(cacheKey);
        return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
    }

    public async Task<List<T>> GetListAsync<T>(string cacheKey)
    {
        var database = connectionMultiplexer.GetDatabase();
        var serializedList = await database.StringGetAsync(cacheKey);

        if (serializedList.IsNullOrEmpty)
            return null;

        return JsonConvert.DeserializeObject<List<T>>(serializedList);
    }

    public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
    {
        if (response == null) return;
        var serializerResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        });
        await distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeOut,
        });
    }

    public async Task SetCacheResponseNoTimeoutAsync(string cacheKey, object response)
    {
        if (response == null) return;
        var serializerResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        });
        await distributedCache.SetStringAsync(cacheKey, serializerResponse);
    }

    public async Task SetListAsync<T>(string cacheKey, List<T> list, TimeSpan timeOut)
    {
        if (list == null || !list.Any()) return;

        var database = connectionMultiplexer.GetDatabase();
        var serializedList = JsonConvert.SerializeObject(list, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        });

        // Store the serialized list as a string in Redis
        await database.StringSetAsync(cacheKey, serializedList, timeOut);
    }
}

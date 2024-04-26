using Authentication.ApiKeys.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Common
{
    public class CacheService : IApiKeysCacheService
    {
        private static readonly TimeSpan _cacheKeysTimeToLive = new(1, 0, 0);

        private readonly IMemoryCache _memoryCache;
        private readonly IClientsService _clientsService;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IMemoryCache memoryCache, IClientsService clientsService, ILogger<CacheService> logger)
        {
            _memoryCache = memoryCache;
            _clientsService = clientsService;
            _logger = logger;
        }

        public async ValueTask<string?> GetOwnerIdFromApiKey(string apiKey)
        {
            _logger.LogInformation($"key from request {apiKey}");
            if (!_memoryCache.TryGetValue<Dictionary<string, Guid>>("Authentication_ApiKeys", out var internalKeys))
            {
                internalKeys = await _clientsService.GetActiveClients();

                _memoryCache.Set("Authentication_ApiKeys", internalKeys, _cacheKeysTimeToLive);
            }

            if (!internalKeys.TryGetValue(apiKey, out var clientId))
            {
                return null;
            }

            return clientId.ToString();
        }

        public async Task InvalidateApiKey(string apiKey)
        {
            if (_memoryCache.TryGetValue<Dictionary<string, Guid>>("Authentication_ApiKeys", out var internalKeys))
            {
                if (internalKeys.ContainsKey(apiKey))
                {
                    internalKeys.Remove(apiKey);
                    _memoryCache.Set("Authentication_ApiKeys", internalKeys);
                }
            }

            await _clientsService.InvalidateApiKey(apiKey);
        }
    }
}

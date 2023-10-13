using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChikoRokoBot.AntiBotNotify.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ChikoRokoBot.AntiBotNotify.Decorators
{
	public class AntiBotPictureManagerCacheDecorator : IAntiBotPictureManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IAntiBotPictureManager _decoratee;

        public AntiBotPictureManagerCacheDecorator(
            IMemoryCache memoryCache,
            IAntiBotPictureManager decoratee)
		{
            _memoryCache = memoryCache;
            _decoratee = decoratee;
        }

        public Task<IList<string>> GetPicturesUrl() =>
            _memoryCache.GetOrCreateAsync("GetPicturesUrl", (cacheEntry) => {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(1);
                return _decoratee.GetPicturesUrl();
            });
    }
}


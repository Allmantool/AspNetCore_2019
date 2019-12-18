using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Northwind.BusinessLogicServices.Interfaces.Models.Caches;

namespace Northwind.BusinessLogicServices.Interfaces
{
    public interface IImagesCachingService
    {
        Tuple<ImageCacheEntry, byte[]> GetImageFromCacheByKey(string key);

        ImageCacheEntry SetImageToCache(string key, ImageCacheEntry imageCacheEntry, byte[] imageAsBytes, ImageCacheOptions opts);
    }
}

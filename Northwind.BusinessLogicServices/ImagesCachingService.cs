using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models.Caches;

namespace Northwind.BusinessLogicServices
{
    public class ImagesCachingService : IImagesCachingService
    {
        private PostEvictionDelegate _callback;
        private IMemoryCache _memoryCache;

        public ImagesCachingService(IMemoryCache cache)
        {
            _memoryCache = cache;

            _callback = (key, value, reason, state) =>
            {
                var imageEntry = value as ImageCacheEntry;

                this.RemoveImageFromFolder(imageEntry);
            };
        }

        public Tuple<ImageCacheEntry, byte[]> GetImageFromCacheByKey(string key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return this.GetImageFromCacheByKeyInternal(key);
        }

        public ImageCacheEntry SetImageToCache(string key, ImageCacheEntry imageCacheEntry, byte[] imageAsBytes, ImageCacheOptions opts)
        {
            if (imageCacheEntry is null)
            {
                throw new ArgumentNullException(nameof(imageCacheEntry));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return ((MemoryCache)_memoryCache).Count == opts.MaxCountOfCachedImages ? null : this.SetImageToCacheInternal(key, imageCacheEntry, imageAsBytes, opts);
        }

        private ImageCacheEntry SetImageToCacheInternal(string key, ImageCacheEntry imageCacheEntry, byte[] imageAsBytes, ImageCacheOptions opts)
        {
            var entryOptions = new MemoryCacheEntryOptions();
            
            entryOptions.RegisterPostEvictionCallback(_callback);
            entryOptions.SetSlidingExpiration(TimeSpan.FromMilliseconds(opts.ExpirationTimeSpanInMilliseconds));

            imageCacheEntry.FullPath = opts.PathToCacheDirectory;

            var entered = _memoryCache.Set<ImageCacheEntry>(key, imageCacheEntry, entryOptions);

            return this.SaveImageToFolder(entered, imageAsBytes) ? entered : null;
        }

        private Tuple<ImageCacheEntry, byte[]> GetImageFromCacheByKeyInternal(string key)
        {
            var entry = _memoryCache.TryGetValue<ImageCacheEntry>(key, out ImageCacheEntry imageCacheEntry) ?
                            imageCacheEntry : null;

            if (entry != null)
            {
                var imageAsBytes = this.GetImageFromFolder(entry);

                return new Tuple<ImageCacheEntry, byte[]>(entry, imageAsBytes);
            }

            return null;
        }

        private byte[] GetImageFromFolder(ImageCacheEntry imageCacheEntry)
        {
            var path = Path.Combine(imageCacheEntry.FullPath, imageCacheEntry.FileName);

            if (File.Exists(path))
            {
                using var stream = File.Open(path, FileMode.Open);

                var imageAsBytes = new byte[stream.Length];

                stream.Read(imageAsBytes, 0, imageAsBytes.Length);

                return imageAsBytes;
            }

            return null;
        }

        private void RemoveImageFromFolder(ImageCacheEntry imageCacheEntry)
        {
            var path = Path.Combine(imageCacheEntry.FullPath,imageCacheEntry.FileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private bool SaveImageToFolder(ImageCacheEntry imageCacheEntry, byte[] imageAsBytes)
        {
            try
            {
                var path = Path.Combine(imageCacheEntry.FullPath, imageCacheEntry.FileName);

                if (!File.Exists(path))
                {
                    using var stream = File.Open(path, FileMode.CreateNew);

                    stream.Write(imageAsBytes, 0, imageAsBytes.Length);
                }

                return true;
            }
            catch (IOException ex)
            {
                return false;
            }
        }
    }
}

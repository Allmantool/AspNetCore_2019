using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.BusinessLogicServices.Interfaces.Models.Caches
{
    public class ImageCacheOptions
    {
        public string PathToCacheDirectory { get; set; }

        public int MaxCountOfCachedImages { get; set; }

        public int ExpirationTimeSpanInMilliseconds { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Northwind.BusinessLogicServices.Interfaces.Models.Caches;
using Northwind.Web.MVC.Middlewares;

namespace Northwind.Web.MVC.Utilities.Extensions
{
    public static class ImagesCacheMiddlewareExtensions
    {
        public static IApplicationBuilder UseImagesCaching(
            this IApplicationBuilder builder, ImageCacheOptions opts)
        {
            return builder.UseMiddleware<ImagesCacheMiddleware>(opts);
        }
    }
}

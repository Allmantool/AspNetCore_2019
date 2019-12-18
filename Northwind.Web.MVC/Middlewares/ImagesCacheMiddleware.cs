using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IO;
using Microsoft.Net.Http.Headers;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models.Caches;
using Northwind.Common.Extensions;
using Northwind.Web.MVC.Utilities;
using Northwind.Web.MVC.Utilities.Extensions;
using Northwind.Web.MVC.Utilities.Extensions.StaticFilesExtensions;

namespace Northwind.Web.MVC.Middlewares
{
    public class ImagesCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private ImageCacheOptions _opts;

        public ImagesCacheMiddleware(RequestDelegate next, ImageCacheOptions opts)
        {
            _next = next;
            _opts = opts;
        }

        public async Task InvokeAsync(HttpContext httpContext, IImagesCachingService imagesCachingService)
        {
            
            
            var key = httpContext.Request.Path.ToUriComponent();

            var imageInfo = imagesCachingService.GetImageFromCacheByKey(key);

            var imageCacheEntry = imageInfo?.Item1;
            var imageAsBytes = imageInfo?.Item2;

            if (imageCacheEntry != null)
            {
                var result = this.GenerateImageResult(imageCacheEntry, imageAsBytes);

                if (result != null)
                {
                    await httpContext.ExecuteResultAsync<FileStreamResult>(result);
                }
            }
            else
            {
                var originalBody = httpContext.Response.Body;

                try
                {
                    await using (var stream = new MemoryStream())
                    {
                        
                        httpContext.Response.Body = stream;

                        await _next(httpContext);

                        var contentType = httpContext.Response.ContentType;

                        if (contentType != null)
                        {
                            var extension = FileExtensions.GetExtensionByContentType(contentType);

                            if (extension != null)
                            {
                                if (ImageExtensions.IsImageFormatOrExtension(extension))
                                {
                                    var contentDisposition =
                                        new ContentDisposition(
                                            httpContext.Response.Headers[HeaderNames.ContentDisposition][0]);

                                    var imageAsBytesToSave = stream.ToArray();

                                    var imageToSet = new ImageCacheEntry
                                    {
                                        FileName = contentDisposition.FileName,
                                    };

                                    imagesCachingService.SetImageToCache(key, imageToSet, imageAsBytesToSave, _opts);
                                }
                            }
                        }

                        stream.Position = 0;
                        await stream.CopyToAsync(originalBody);
                    }
                }
                finally
                {
                    httpContext.Response.Body = originalBody;
                }
            }
        }

        private FileStreamResult GenerateImageResult(ImageCacheEntry image, byte[] imageAsBytes)
        {
            var fileExtension = image.FileName.Substring(image.FileName.LastIndexOf('.'));
            var contentType = FileExtensions.GetContentTypeByExtension(fileExtension);

            var stream = new MemoryStream(imageAsBytes);

            var result = new FileStreamResult(stream, contentType) {FileDownloadName = image.FileName};

            return result;
        }
    }
}

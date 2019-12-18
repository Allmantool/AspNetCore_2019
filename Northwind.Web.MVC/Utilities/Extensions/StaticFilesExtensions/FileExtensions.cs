using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace Northwind.Web.MVC.Utilities.Extensions.StaticFilesExtensions
{
    public static class FileExtensions
    {
        private static FileExtensionContentTypeProvider _provider = new FileExtensionContentTypeProvider();

        public static string GetContentType(string pathToFile)
        {
            var contentType =
                _provider.TryGetContentType(pathToFile, out string resolvedContentType)
                    ? resolvedContentType
                    : ContentTypes.ApplicationOctetStream;

            return contentType;
        }

        public static string GetContentTypeByExtension(string fileExtension)
        {
            if (fileExtension is null)
            {
                throw new ArgumentOutOfRangeException(nameof(fileExtension));
            }

            var contentType = _provider.Mappings.FirstOrDefault(mt => mt.Key.Contains(fileExtension)).Value;

            return contentType;
        }

        public static string GetExtensionByContentType(string contentType)
        {
            if (contentType is null)
            {
                return null;
            }

            var extensions = _provider.Mappings.Where(mt =>
                mt.Value.Contains(contentType, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            if (!extensions.Any())
            {
                return null;
            }

            var result = extensions.Select(kv => kv.Key).First();

            return result;
        }
    }
}

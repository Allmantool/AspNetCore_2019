using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Northwind.Web.MVC.Components
{
    public class BreadcrumbsNavigation : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _host;
        private string _scheme;

        public BreadcrumbsNavigation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
            var host = _httpContextAccessor.HttpContext.Request.Host.Host;
            var port = _httpContextAccessor.HttpContext.Request.Host.Port;
            var path = _httpContextAccessor.HttpContext.Request.Path.Value;

            var uriBuilder = new UriBuilder(scheme, host, port.Value, path);

            var result = this.PrepareNavigationEntries(uriBuilder.Uri);

            return View(result);
        }

        public IEnumerable<BreadcrumbsNavigationEntry> PrepareNavigationEntries(Uri uri)
        {
            var uriPath = uri.GetComponents(UriComponents.Path, UriFormat.UriEscaped);

            var navEntries = new List<BreadcrumbsNavigationEntry>();

            navEntries.Add(
                new BreadcrumbsNavigationEntry {NavigationPathPartLink = uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped), NavigationPathPartName = "Home"});
            var splittedPath = uriPath.Split('/');

            foreach (var pathPart in splittedPath)
            {
                var splitted = uriPath.Split('/');

                var aggregatedRelatedLink = splitted.Take(splitted.ToList().IndexOf(pathPart) + 1)
                                            .Aggregate((first, second) => first + '/' + second);

                var absoluteLink = 
                    uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped) + '/' + aggregatedRelatedLink;

                navEntries.Add(new BreadcrumbsNavigationEntry
                    {NavigationPathPartLink = absoluteLink, NavigationPathPartName = pathPart});
            }

            return navEntries;
        }

        public struct BreadcrumbsNavigationEntry {

            public string NavigationPathPartName { get; set; }
            
            public string NavigationPathPartLink { get; set; }
        }
    }
}

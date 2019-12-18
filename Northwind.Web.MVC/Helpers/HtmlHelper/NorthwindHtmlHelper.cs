using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Northwind.Web.MVC.Helpers.HtmlHelper
{
    public static class NorthwindHtmlHelper
    {
        public static IHtmlContent NorthwindImageLink(this IHtmlHelper htmlHelper,int imageId, string linkText)
        {
            return htmlHelper.ActionLink(linkText, "GetImage", "Categories", new {categoryId = imageId});
        }
    }
}

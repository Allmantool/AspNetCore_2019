using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Northwind.Web.MVC.Helpers.TagHelper
{
    [HtmlTargetElement(TargetElementName, Attributes= NorthwindIdAttributeName, TagStructure=TagStructure.NormalOrSelfClosing)]
    public class NorthwindIdTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        private const string TargetElementName = "a";
        private const string NorthwindIdAttributeName = "northwind-id";
        private const string Href = "href";

        [HtmlAttributeName(NorthwindIdAttributeName)]
        public string CategoryId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (output.Attributes.ContainsName(Href))
            {
                throw new InvalidOperationException(
                    $"Can not apply {NorthwindIdAttributeName} for tag {TargetElementName} cause it's attribute already defined exists. Attribute name : {Href}");
            }

            output.Attributes.SetAttribute("href", $"Categories/GetImage?categoryId={this.CategoryId}");
        }

    }
}

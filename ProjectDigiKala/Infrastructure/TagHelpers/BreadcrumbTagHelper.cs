using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ProjectDigiKala.Models.Breadcrumb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Infrastructure.TagHelpers
{
    [HtmlTargetElement("nav", Attributes = "page-breadcrumbs")]
    public class BreadcrumbTagHelper : TagHelper
    {
        public IEnumerable<Breadcrumb> PageBreadcrumbs { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder mainContent = new TagBuilder("ol");
            mainContent.Attributes["class"] = "breadcrumb mt-3";
            foreach (var item in PageBreadcrumbs)
            {
                TagBuilder breadcrumbItem = new TagBuilder("li");
                TagBuilder link;

                if (!string.IsNullOrEmpty(item.Url))
                {
                    link = new TagBuilder("a");
                    link.InnerHtml.Append(item.Title);
                    link.Attributes["href"] = item.Url;
                    breadcrumbItem.InnerHtml.AppendHtml(link);
                    breadcrumbItem.Attributes["class"] = "breadcrumb-item";
                }
                else
                {
                    breadcrumbItem.InnerHtml.Append(item.Title);
                    breadcrumbItem.Attributes["class"] = "breadcrumb-item active";
                }

                mainContent.InnerHtml.AppendHtml(breadcrumbItem);
            }

            output.Attributes.SetAttribute("aria-label", "breadcrumb");
            output.Content.AppendHtml(mainContent);
        }
    }
}

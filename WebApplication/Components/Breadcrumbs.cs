using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models.Settings;
using WebApplication.ViewModels;

namespace WebApplication.Components
{
    public class Breadcrumbs : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var res = GetBreadcrumbs();
            return View(res ?? new List<SitemapElement>());
        }

        private IEnumerable<SitemapElement> GetBreadcrumbs()
        {
            var i = 0;
            var result = XElement.Load("sitemap.xml")
                .Descendants("mvcSiteMapNode")
                .FirstOrDefault(x =>
                    x.Attribute("action")?.Value == Request.RouteValues["action"]?.ToString()
                    &&
                    x.Attribute("controller")?.Value == Request.RouteValues["controller"]?.ToString())?
                .AncestorsAndSelf()
                .Select(
                    x =>
                    {
                        var t = x.Attributes().ToList();
                        return new SitemapElement()
                        {
                            ActionName = t.FirstOrDefault(x => x.Name == "action").Value,
                            ControllerName = t.FirstOrDefault(x => x.Name == "controller").Value,
                            Title = t.FirstOrDefault(x => x.Name == "title").Value,
                            ParentLevel = i++
                        };
                    })
                .OrderByDescending(x => x.ParentLevel)
                .ToList();
            return result;
        }
    }
}

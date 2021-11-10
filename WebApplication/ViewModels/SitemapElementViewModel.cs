using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class SitemapElementViewModel
    {
        public SitemapElementViewModel()
        {
            SitemapElements = new List<SitemapElement>();
        }

        public IEnumerable<SitemapElement> SitemapElements { get; set; }
    }
}

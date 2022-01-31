using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class SitemapElement
    {
        public string Title { get; init; }
        public string ControllerName { get; init; }
        public string ActionName { get; init; }
        public int ParentLevel { get; init; }
    }
}

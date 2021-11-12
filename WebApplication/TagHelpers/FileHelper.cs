using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.TagHelpers
{
    public static class FileHelper
    {
        public static HtmlString NorthwindImageLink(this IHtmlHelper html, int imageID, string linkText)
        {
            var s = $@"<a href=""/images/{imageID}"">{linkText}</a>";
            return new HtmlString(s);
        }
    }
}

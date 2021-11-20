using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApplication.TagHelpers
{
    public class NorthwindTagHelper : TagHelper
    {
        public int ImageID { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var s = $@"/images/{ImageID}";
            output.Attributes.SetAttribute("href", s);
        }
    }
}

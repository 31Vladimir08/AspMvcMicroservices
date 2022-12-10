namespace WebApplication.Models.Settings
{
    public class SitemapElement
    {
        public string Title { get; init; }
        public string ControllerName { get; init; }
        public string ActionName { get; init; }
        public int ParentLevel { get; init; }
    }
}

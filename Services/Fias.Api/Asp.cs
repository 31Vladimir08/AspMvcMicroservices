namespace Fias.Api
{
    public class Asp
    {
        public static string GetAspDirectoryTempPath()
        {
            return Path.Combine(Path.GetTempPath(), "ASPNETCORE_TEMP");
        }

        public static string GetAspDirectoryQueryTempPath()
        {
            return Path.Combine(GetAspDirectoryTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
        }
    }
}

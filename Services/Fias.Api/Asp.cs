namespace Fias.Api
{
    public class Asp
    {
        public static string GetTempPath()
        {
            return Path.Combine(Path.GetTempPath(), "ASPNETCORE_TEMP");
        }
    }
}

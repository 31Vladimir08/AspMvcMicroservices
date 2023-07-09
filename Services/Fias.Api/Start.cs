namespace Fias.Api
{
    public class Start
    {
        public static void DeleteTempDirectory()
        {
            var filePath = Asp.GetAspDirectoryTempPath();
            if (Directory.Exists(filePath))
                Directory.Delete(filePath, true);
        }
    }
}

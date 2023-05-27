namespace Fias.Api.Models.File
{
    public class TempFile
    {
        public TempFile(string fullFilePath, string originFileName)
        {
            FullFilePath = fullFilePath;
            OriginFileName = originFileName;
        }

        public string FullFilePath { get; }
        public string OriginFileName { get; }
    }
}

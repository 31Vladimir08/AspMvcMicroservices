namespace Fias.Api.Models.File
{
    public class TempFile
    {
        public TempFile(string fullFilePath, string originFileName, long lenght = 0)
        {
            FullFilePath = fullFilePath;
            OriginFileName = originFileName;
            Lenght = lenght;
        }

        public string FullFilePath { get; }
        public string OriginFileName { get; }
        public long Lenght { get; }
    }
}

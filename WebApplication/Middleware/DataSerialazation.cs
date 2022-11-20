using System;

namespace WebApplication.Middleware
{
    public class FileInCasheDataSerialazation : IEquatable<FileInCasheDataSerialazation>
    {
        public string CategoryID { get; set; }

        public DateTime DateOfLastReading { get; set; }

        public bool Equals(FileInCasheDataSerialazation other)
        {
            return other != null && this.CategoryID.Equals(other.CategoryID);
        }
    }
}

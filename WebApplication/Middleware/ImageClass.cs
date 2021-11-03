using System;

namespace WebApplication.Middleware
{
    public class ImageClass : IEquatable<ImageClass>
    {
        public string CategoryID { get; set; }

        public DateTime DateOfLastReading { get; set; }

        public bool Equals(ImageClass other)
        {
            return other != null && this.CategoryID.Equals(other.CategoryID);
        }
    }
}

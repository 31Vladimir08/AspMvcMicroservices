using System;

namespace WebApplication.Middleware
{
    public class Image : IEquatable<Image>
    {
        public string CategoryID { get; set; }

        public DateTime DateOfLastReading { get; set; }

        public bool Equals(Image other)
        {
            return other != null && this.CategoryID.Equals(other.CategoryID);
        }
    }
}

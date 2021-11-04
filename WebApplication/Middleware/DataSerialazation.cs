using System;

namespace WebApplication.Middleware
{
    public class DataSerialazation : IEquatable<DataSerialazation>
    {
        public string CategoryID { get; set; }

        public DateTime DateOfLastReading { get; set; }

        public bool Equals(DataSerialazation other)
        {
            return other != null && this.CategoryID.Equals(other.CategoryID);
        }
    }
}

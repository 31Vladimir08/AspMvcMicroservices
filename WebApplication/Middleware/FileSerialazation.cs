using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebApplication.Middleware
{
    [XmlRoot(ElementName = "FileSerialazation")]
    public class FileSerialazation
    {
        public FileSerialazation()
        {
            Pictures = new List<DataSerialazation>();
        }

        public List<DataSerialazation> Pictures { get; set; }
    }
}

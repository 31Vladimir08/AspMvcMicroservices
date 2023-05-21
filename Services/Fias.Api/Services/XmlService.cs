using System.Xml.Serialization;

using Fias.Api.Interfaces.Services;

namespace Fias.Api.Services
{
    public class XmlService : IXmlService
    {
        public T? DeserializeFiasXml<T>(FileStream xmlFile) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            var theObject = serializer.Deserialize(xmlFile) as T;
            return theObject;
        }
    }
}

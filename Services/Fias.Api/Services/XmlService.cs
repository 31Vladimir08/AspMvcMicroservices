using System.Text.RegularExpressions;
using System.Xml.Serialization;

using Fias.Api.Enums;
using Fias.Api.Interfaces.Services;
using Fias.Api.Interfaces.XmlModels;

namespace Fias.Api.Services
{
    public class XmlService : IXmlService
    {
        private Dictionary <string, XmlModelType> _attributes;

        public XmlService()
        {
            _attributes = GetXmlDictionary();
        }

        public T? DeserializeFiasXml<T>(FileStream xmlFile) where T : class, IXmlModel
        {
            var serializer = new XmlSerializer(typeof(T));
            var theObject = serializer.Deserialize(xmlFile) as T;
            return theObject;
        }

        public XmlModelType GetXmlModelTypeFromXmlFile(string xmlFileName)
        {
            var fileName = GetXmlKeyFromXmlFileName(xmlFileName);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new NotImplementedException();
            }

            var isTypeExist =_attributes.TryGetValue(fileName, out var type);
            return isTypeExist ? type : XmlModelType.Unknown;
        }

        private Dictionary<string, XmlModelType> GetXmlDictionary()
        {
            return new Dictionary<string, XmlModelType>()
            {
                { "AS_ADDR_OBJ", XmlModelType.AddrObj },
                { "AS_ADDR_OBJ_PARAMS", XmlModelType.AddrObjParams },
                { "AS_HOUSES", XmlModelType.Houses },
                { "AS_HOUSES_PARAMS", XmlModelType.HousesParams },
                { "AS_PARAM_TYPES", XmlModelType.ParamTypes },
            };
        }

        private string? GetXmlKeyFromXmlFileName(string fileName)
        {
            var regex = new Regex("^([^0-9]+)");
            var matches = regex.Matches(fileName);
            return matches.FirstOrDefault()?.Value?.TrimEnd('_');
        }
    }
}

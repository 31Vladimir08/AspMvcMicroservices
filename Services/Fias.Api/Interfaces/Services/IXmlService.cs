using Fias.Api.Enums;
using Fias.Api.Interfaces.XmlModels;

namespace Fias.Api.Interfaces.Services
{
    public interface IXmlService
    {
        T? DeserializeFiasXml<T>(FileStream xmlFile) where T : class, IXmlModel;
        XmlModelType GetXmlModelTypeFromXmlFile(string xmlFileName);
    }
}

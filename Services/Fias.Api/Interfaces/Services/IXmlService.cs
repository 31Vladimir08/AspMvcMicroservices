using Fias.Api.Contexts;
using Fias.Api.Enums;
using Fias.Api.Interfaces.XmlModels;
using Fias.Api.Models.File;

namespace Fias.Api.Interfaces.Services
{
    public interface IXmlService
    {
        T? DeserializeFiasXml<T>(FileStream xmlFile) where T : class, IXmlModel;
        T? DeserializeFiasXml<T>(string xml) where T : class, IXmlModel;
        XmlModelType GetXmlModelTypeFromXmlFile(string xmlFileName);
        Task InsertToDbFromXmlFileAsync(TempFile tempXml, bool isRestoreDb = false);
        Task RemoveAllXmlTableAsync();
    }
}

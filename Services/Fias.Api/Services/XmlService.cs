using System.Text.RegularExpressions;
using System.Xml.Serialization;

using AutoMapper;

using Fias.Api.Enums;
using Fias.Api.Interfaces.Repositories;
using Fias.Api.Interfaces.Services;
using Fias.Api.Interfaces.XmlModels;
using Fias.Api.Models.File;
using Fias.Api.Entities;
using Fias.Api.Models.FiasModels.XmlModels.AddrObj;
using Fias.Api.Models.FiasModels.XmlModels.Houses;
using Fias.Api.Models.FiasModels.XmlModels.ParamTypes;
using Fias.Api.Models.FiasModels.XmlModels.HousesParams;
using Fias.Api.Contexts;

namespace Fias.Api.Services
{
    public class XmlService : IXmlService
    {
        private Dictionary <string, XmlModelType> _attributes;
        private readonly IMapper _mapper;
        private readonly IBaseRepository _baseRepository;

        public XmlService(
            IMapper mapper,
            IBaseRepository baseRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _baseRepository = baseRepository;
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

        public async Task RemoveAllXmlTableAsync()
        {
            await _baseRepository.DeleteAllEntitiesAsync<HouseEntity>();
            await _baseRepository.DeleteAllEntitiesAsync<HouseParamsEntity>();
            await _baseRepository.DeleteAllEntitiesAsync<ParamTypesEntity>();
            await _baseRepository.DeleteAllEntitiesAsync<AddrObjEntity>();
            await _baseRepository.DeleteAllEntitiesAsync<AddrObjParamEntity>();
        }

        public async Task InsertToDbFromXmlFileAsync(TempFile tempXml, bool isRestoreDb = false)
        {
            if (string.IsNullOrWhiteSpace(tempXml.FullFilePath))
                return;
            var xmlModelType = GetXmlModelTypeFromXmlFile(tempXml.OriginFileName);
            using (var file = new FileStream(tempXml.FullFilePath, FileMode.Open, FileAccess.Read))
            {
                switch (xmlModelType)
                {
                    case XmlModelType.Houses:
                        {
                            var model = DeserializeFiasXml<HOUSES>(file);
                            var entities = model?.HOUSE?.AsParallel().Select(_mapper.Map<HouseEntity>).ToList();
                            model = null;
                            await InsertsOrUpdatesAsync(entities, isRestoreDb);
                            break;
                        }
                    case XmlModelType.HousesParams:
                        {
                            var model = DeserializeFiasXml<PARAMS>(file);
                            var entities = model?.PARAM?.AsParallel().Select(_mapper.Map<HouseParamsEntity>).ToList();
                            model = null;
                            await InsertsOrUpdatesAsync(entities, isRestoreDb);
                            break;
                        }

                    case XmlModelType.ParamTypes:
                        {
                            var model = DeserializeFiasXml<PARAMTYPES>(file);
                            var entities = model?.PARAMTYPE?.AsParallel().Select(_mapper.Map<ParamTypesEntity>).ToList();
                            model = null;
                            await InsertsOrUpdatesAsync(entities, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AddrObj:
                        {
                            var model = DeserializeFiasXml<ADDRESSOBJECTS>(file);
                            var entities = model?.OBJECT?.AsParallel().Select(_mapper.Map<AddrObjEntity>).ToList();
                            model = null;
                            await InsertsOrUpdatesAsync(entities, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AddrObjParams:
                        {
                            var model = DeserializeFiasXml<Models.FiasModels.XmlModels.AddrObjParams.PARAMS>(file);
                            var entities = model?.PARAM?.AsParallel().Select(_mapper.Map<AddrObjParamEntity>).ToList();
                            model = null;
                            await InsertsOrUpdatesAsync(entities, isRestoreDb);
                            break;
                        }
                }
            }
        }

        private async Task InsertsOrUpdatesAsync<TEntity>(List<TEntity>? entities, bool isRestoreDb = false) where TEntity : BaseEntity
        {
            if (isRestoreDb)
            {
                await _baseRepository.InsertsAsync(entities);
            }
            else
            {
                await _baseRepository.InsertsOrUpdatesAsync(entities);
            }
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

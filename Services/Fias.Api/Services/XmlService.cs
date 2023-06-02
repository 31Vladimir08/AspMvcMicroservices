using System.Text.RegularExpressions;
using System.Xml.Serialization;

using AutoMapper;

using Fias.Api.Enums;
using Fias.Api.Interfaces.Repositories;
using Fias.Api.Interfaces.Services;
using Fias.Api.Interfaces.XmlModels;
using Fias.Api.Models.File;
using Fias.Api.Entities;
using Fias.Api.Models.FiasModels.XmlModels.Houses;
using Fias.Api.Models.FiasModels.XmlModels.ParamTypes;
using Fias.Api.Models.FiasModels.XmlModels.HousesParams;
using System.Xml;
using Fias.Api.Contexts;
using System.Reflection;
using Fias.Api.Exceptions;
using Fias.Api.Extensions;
using Fias.Api.Models.FiasModels.XmlModels.AddrObj;
using Fias.Api.Models.FiasModels.XmlModels.AddrObjParams;

namespace Fias.Api.Services
{
    public class XmlService : IXmlService
    {
        private Dictionary <string, XmlModelType> _attributes;
        private readonly IMapper _mapper;
        private readonly IBaseRepository _baseRepository;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<XmlService> _loger;

        public XmlService(
            IMapper mapper,
            IBaseRepository baseRepository,
            ILogger<XmlService> loger,
            AppDbContext dbContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _loger = loger ?? throw new ArgumentNullException(nameof(loger));
            _attributes = GetXmlDictionary();
        }

        public T? DeserializeFiasXml<T>(FileStream xmlFile) where T : class, IXmlModel
        {
            var serializer = new XmlSerializer(typeof(T));
            var theObject = serializer.Deserialize(xmlFile) as T;
            return theObject;
        }

        public T? DeserializeFiasXml<T>(string xml) where T : class, IXmlModel
        {
            var ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(xml))
            {
                return ser.Deserialize(sr) as T;
            }
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
            _loger.LogInformation($"delete all from xml table: START");
            await _baseRepository.DeleteAllEntitiesAsync<HouseEntity>();
            await _baseRepository.DeleteAllEntitiesAsync<HouseParamEntity>();
            await _baseRepository.DeleteAllEntitiesAsync<ParamTypesEntity>();
            await _baseRepository.DeleteAllEntitiesAsync<AddrObjEntity>();
            await _baseRepository.DeleteAllEntitiesAsync<AddrObjParamEntity>();
            _loger.LogInformation($"delete all from xml table: FINISH");
        }

        public async Task InsertToDbFromXmlFileAsync(TempFile tempXml, bool isRestoreDb = false)
        {
            if (string.IsNullOrWhiteSpace(tempXml.FullFilePath))
                return;
            var xmlModelType = GetXmlModelTypeFromXmlFile(tempXml.OriginFileName);

            using (var reader = XmlReader.Create(tempXml.FullFilePath, new XmlReaderSettings() { Async = true }))
            {
                switch (xmlModelType)
                {
                    case XmlModelType.Houses:
                        {
                            await ReadXmlWriteDbAsync<HouseModel, HouseEntity>(reader, isRestoreDb);
                            break;
                        }
                    case XmlModelType.HousesParams:
                        {
                            await ReadXmlWriteDbAsync<HouseParamModel, HouseParamEntity>(reader, isRestoreDb);
                            break;
                        }

                    case XmlModelType.ParamTypes:
                        {
                            await ReadXmlWriteDbAsync<ParamTypesModel, ParamTypesEntity>(reader, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AddrObj:
                        {
                            await ReadXmlWriteDbAsync<AddrObjModel, AddrObjEntity>(reader, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AddrObjParams:
                        {
                            await ReadXmlWriteDbAsync<AddrObjParamModel, AddrObjParamEntity>(reader, isRestoreDb);
                            break;
                        }
                }
            }
                /*using (var file = new FileStream(tempXml.FullFilePath, FileMode.Open, FileAccess.Read))
                {
                    switch (xmlModelType)
                    {
                        case XmlModelType.Houses:
                            {
                                var model = DeserializeFiasXml<HOUSES>(file);
                                var entities = model?.HOUSE?.AsParallel().Select(_mapper.Map<HouseEntity>).ToList();
                                model = null;
                                //await InsertsOrUpdatesAsync(entities, isRestoreDb);
                                break;
                            }
                        case XmlModelType.HousesParams:
                            {
                                var model = DeserializeFiasXml<PARAMS>(file);
                                var entities = model?.PARAM?.AsParallel().Select(_mapper.Map<HouseParamsEntity>).ToList();
                                model = null;
                                //await InsertsOrUpdatesAsync(entities, isRestoreDb);
                                break;
                            }

                        case XmlModelType.ParamTypes:
                            {
                                var model = DeserializeFiasXml<PARAMTYPES>(file);
                                var entities = model?.PARAMTYPE?.AsParallel().Select(_mapper.Map<ParamTypesEntity>).ToList();
                                model = null;
                                //await InsertsOrUpdatesAsync(entities, isRestoreDb);
                                break;
                            }
                        case XmlModelType.AddrObj:
                            {
                                var model = DeserializeFiasXml<ADDRESSOBJECTS>(file);
                                var entities = model?.OBJECT?.AsParallel().Select(_mapper.Map<AddrObjEntity>).ToList();
                                model = null;
                                //await InsertsOrUpdatesAsync(entities, isRestoreDb);
                                break;
                            }
                        case XmlModelType.AddrObjParams:
                            {
                                var model = DeserializeFiasXml<Models.FiasModels.XmlModels.AddrObjParams.PARAMS>(file);
                                var entities = model?.PARAM?.AsParallel().Select(_mapper.Map<AddrObjParamEntity>).ToList();
                                model = null;
                                //await InsertsOrUpdatesAsync(entities, isRestoreDb);
                                break;
                            }
                    }
                }*/
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

        private async Task InsertOrUpdateAsync<TEntity>(TEntity? entity, bool isRestoreDb = false) where TEntity : BaseEntity
        {
            if (isRestoreDb)
            {
                await _baseRepository.InsertAsync(entity);
            }
            else
            {
                await _baseRepository.InsertOrUpdateAsync(entity);
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

        private string? GetXmlRoot<T>() where T: class, IXmlRowModel
        {
            var type = typeof(T);
            XmlRootAttribute? atribut = type.GetCustomAttribute(typeof(XmlRootAttribute), false) as XmlRootAttribute;
            return atribut?.ElementName;
        }

        private T GetHouseModelFromReader<T>(XmlReader reader) where T: class, IXmlRowModel
        {
            var type = typeof(T);
            var ob = Activator.CreateInstance(type) as T ?? throw new UserException("БАНАНА");
            var properties = ob.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(XmlAttributeAttribute), false) is not XmlAttributeAttribute pr)
                    continue;
                var value = reader.GetAttribute(pr.AttributeName);
                if (string.IsNullOrWhiteSpace(value))
                    continue;
                property.SetValueType(ob, value);
            }

            return ob;
        }

        private async Task ReadXmlWriteDbAsync<TModel, TEntity>(XmlReader reader, bool isRestoreDb = false)
            where TModel : class, IXmlRowModel
            where TEntity : BaseEntity
        {
            var xmlRoot = GetXmlRoot<TModel>();
            _loger.LogInformation($"Update db Table: START");
            _baseRepository.SetIdentityInsert<TEntity>(true);
            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            if (reader.LocalName == xmlRoot)
                            {
                                var model = GetHouseModelFromReader<TModel>(reader);
                                var entity = _mapper.Map<TEntity>(model);
                                await InsertOrUpdateAsync(entity, isRestoreDb);
                                break;
                            }

                            break;
                        }
                }
            }

            await _dbContext.SaveChangesAsync();
            _baseRepository.SetIdentityInsert<TEntity>(false);
            _loger.LogInformation($"Update db Table: FINISH");
        }
    }
}

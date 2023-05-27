using AutoMapper;

using Fias.Api.Entities;
using Fias.Api.Models.FiasModels.XmlModels.AddrObj;
using Fias.Api.Models.FiasModels.XmlModels.AddrObjParams;
using Fias.Api.Models.FiasModels.XmlModels.Houses;
using Fias.Api.Models.FiasModels.XmlModels.ParamTypes;

namespace Fias.Api.AutoMapperProfile
{
    public class AutoMapProfiler : Profile
    {
        public AutoMapProfiler()
        {
            CreateProfile();
        }

        private void CreateProfile()
        {
            CreateMap<AddrObjEntity, ADDRESSOBJECTSOBJECT>()
                .ForMember(x => x.ID, s => s.MapFrom(x => x.Id))
                .ForMember(x => x.OBJECTID, s => s.MapFrom(x => x.ObjectId))
                .ForMember(x => x.OBJECTGUID, s => s.MapFrom(x => x.ObjectGuid))
                .ForMember(x => x.CHANGEID, s => s.MapFrom(x => x.ChangeId))
                .ForMember(x => x.NAME, s => s.MapFrom(x => x.Name))
                .ForMember(x => x.TYPENAME, s => s.MapFrom(x => x.TypeName))
                .ForMember(x => x.LEVEL, s => s.MapFrom(x => x.Level))
                .ForMember(x => x.OPERTYPEID, s => s.MapFrom(x => x.OperTypeId))
                .ForMember(x => x.PREVID, s => s.MapFrom(x => x.PrevId))
                .ForMember(x => x.NEXTID, s => s.MapFrom(x => x.NextId))
                .ForMember(x => x.NEXTIDSpecified, s => s.Ignore())
                .ForMember(x => x.UPDATEDATE, s => s.MapFrom(x => x.UpdateDate))
                .ForMember(x => x.STARTDATE, s => s.MapFrom(x => x.StartDate))
                .ForMember(x => x.ENDDATE, s => s.MapFrom(x => x.EndDate))
                .ForMember(x => x.ISACTUAL, s => s.MapFrom(x => x.IsActual))
                .ForMember(x => x.ISACTIVE, s => s.MapFrom(x => x.IsActive));
            CreateMap<ADDRESSOBJECTSOBJECT, AddrObjEntity>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.ID))
                .ForMember(x => x.ObjectId, s => s.MapFrom(x => x.OBJECTID))
                .ForMember(x => x.ObjectGuid, s => s.MapFrom(x => x.OBJECTGUID))
                .ForMember(x => x.ChangeId, s => s.MapFrom(x => x.CHANGEID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.NAME))
                .ForMember(x => x.TypeName, s => s.MapFrom(x => x.TYPENAME))
                .ForMember(x => x.Level, s => s.MapFrom(x => x.LEVEL))
                .ForMember(x => x.OperTypeId, s => s.MapFrom(x => x.OPERTYPEID))
                .ForMember(x => x.PrevId, s => s.MapFrom(x => x.PREVID))
                .ForMember(x => x.NextId, s => s.MapFrom(x => x.NEXTID))
                .ForMember(x => x.UpdateDate, s => s.MapFrom(x => x.UPDATEDATE))
                .ForMember(x => x.StartDate, s => s.MapFrom(x => x.STARTDATE))
                .ForMember(x => x.EndDate, s => s.MapFrom(x => x.ENDDATE))
                .ForMember(x => x.IsActual, s => s.MapFrom(x => x.ISACTUAL))
                .ForMember(x => x.IsActive, s => s.MapFrom(x => x.ISACTIVE));

            CreateMap<AddrObjParamEntity, PARAMSPARAM>()
                .ForMember(x => x.ID, s => s.MapFrom(x => x.Id))
                .ForMember(x => x.OBJECTID, s => s.MapFrom(x => x.ObjectId))
                .ForMember(x => x.CHANGEID, s => s.MapFrom(x => x.ChangeId))
                .ForMember(x => x.CHANGEIDEND, s => s.MapFrom(x => x.ChangeIdEnd))
                .ForMember(x => x.TYPEID, s => s.MapFrom(x => x.TypeId))
                .ForMember(x => x.VALUE, s => s.MapFrom(x => x.Value))
                .ForMember(x => x.UPDATEDATE, s => s.MapFrom(x => x.UpdateDate))
                .ForMember(x => x.STARTDATE, s => s.MapFrom(x => x.StartDate))
                .ForMember(x => x.ENDDATE, s => s.MapFrom(x => x.EndDate));
            CreateMap<PARAMSPARAM, AddrObjParamEntity>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.ID))
                .ForMember(x => x.ObjectId, s => s.MapFrom(x => x.OBJECTID))
                .ForMember(x => x.ChangeId, s => s.MapFrom(x => x.CHANGEID))
                .ForMember(x => x.ChangeIdEnd, s => s.MapFrom(x => x.CHANGEIDEND))
                .ForMember(x => x.TypeId, s => s.MapFrom(x => x.TYPEID))
                .ForMember(x => x.Value, s => s.MapFrom(x => x.VALUE))
                .ForMember(x => x.UpdateDate, s => s.MapFrom(x => x.UPDATEDATE))
                .ForMember(x => x.StartDate, s => s.MapFrom(x => x.STARTDATE))
                .ForMember(x => x.EndDate, s => s.MapFrom(x => x.ENDDATE));

            CreateMap<HouseEntity, HOUSESHOUSE>()
                .ForMember(x => x.ID, s => s.MapFrom(x => x.Id))
                .ForMember(x => x.OBJECTID, s => s.MapFrom(x => x.ObjectId))
                .ForMember(x => x.OBJECTGUID, s => s.MapFrom(x => x.ObjectGuid))
                .ForMember(x => x.CHANGEID, s => s.MapFrom(x => x.ChangeId))
                .ForMember(x => x.HOUSENUM, s => s.MapFrom(x => x.HouseNum))
                .ForMember(x => x.HOUSETYPE, s => s.MapFrom(x => x.HouseType))
                .ForMember(x => x.OPERTYPEID, s => s.MapFrom(x => x.OperTypeId))
                .ForMember(x => x.PREVID, s => s.MapFrom(x => x.PrevId))
                .ForMember(x => x.NEXTID, s => s.MapFrom(x => x.NextId))
                .ForMember(x => x.NEXTIDSpecified, s => s.Ignore())
                .ForMember(x => x.UPDATEDATE, s => s.MapFrom(x => x.UpdateDate))
                .ForMember(x => x.STARTDATE, s => s.MapFrom(x => x.StartDate))
                .ForMember(x => x.ENDDATE, s => s.MapFrom(x => x.EndDate))
                .ForMember(x => x.ISACTUAL, s => s.MapFrom(x => x.IsActual))
                .ForMember(x => x.ISACTIVE, s => s.MapFrom(x => x.IsActive))
                .ForMember(x => x.ADDNUM1, s => s.MapFrom(x => x.AddNum1))
                .ForMember(x => x.ADDNUM1Specified, s => s.MapFrom(x => x.AddNum1Specified))
                .ForMember(x => x.ADDTYPE1, s => s.MapFrom(x => x.AddType1))
                .ForMember(x => x.ADDTYPE1Specified, s => s.MapFrom(x => x.AddType1Specified));
            CreateMap<HOUSESHOUSE, HouseEntity>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.ID))
                .ForMember(x => x.ObjectId, s => s.MapFrom(x => x.OBJECTID))
                .ForMember(x => x.ObjectGuid, s => s.MapFrom(x => x.OBJECTGUID))
                .ForMember(x => x.ChangeId, s => s.MapFrom(x => x.CHANGEID))
                .ForMember(x => x.HouseNum, s => s.MapFrom(x => x.HOUSENUM))
                .ForMember(x => x.HouseType, s => s.MapFrom(x => x.HOUSETYPE))
                .ForMember(x => x.OperTypeId, s => s.MapFrom(x => x.OPERTYPEID))
                .ForMember(x => x.PrevId, s => s.MapFrom(x => x.PREVID))
                .ForMember(x => x.NextId, s => s.MapFrom(x => x.NEXTID))
                .ForMember(x => x.UpdateDate, s => s.MapFrom(x => x.UPDATEDATE))
                .ForMember(x => x.StartDate, s => s.MapFrom(x => x.STARTDATE))
                .ForMember(x => x.EndDate, s => s.MapFrom(x => x.ENDDATE))
                .ForMember(x => x.IsActual, s => s.MapFrom(x => x.ISACTUAL))
                .ForMember(x => x.IsActive, s => s.MapFrom(x => x.ISACTIVE))
                .ForMember(x => x.AddNum1, s => s.MapFrom(x => x.ADDNUM1))
                .ForMember(x => x.AddNum1Specified, s => s.MapFrom(x => x.ADDNUM1Specified))
                .ForMember(x => x.AddType1, s => s.MapFrom(x => x.ADDTYPE1))
                .ForMember(x => x.AddType1Specified, s => s.MapFrom(x => x.ADDTYPE1Specified));

            CreateMap<HouseParamsEntity, Models.FiasModels.XmlModels.HousesParams.PARAMSPARAM>()
                .ForMember(x => x.ID, s => s.MapFrom(x => x.Id))
                .ForMember(x => x.OBJECTID, s => s.MapFrom(x => x.ObjectId))
                .ForMember(x => x.CHANGEID, s => s.MapFrom(x => x.ChangeId))
                .ForMember(x => x.CHANGEIDEND, s => s.MapFrom(x => x.ChangeIdEnd))
                .ForMember(x => x.TYPEID, s => s.MapFrom(x => x.TypeId))
                .ForMember(x => x.VALUE, s => s.MapFrom(x => x.Value))
                .ForMember(x => x.UPDATEDATE, s => s.MapFrom(x => x.UpdateDate))
                .ForMember(x => x.STARTDATE, s => s.MapFrom(x => x.StartDate))
                .ForMember(x => x.ENDDATE, s => s.MapFrom(x => x.EndDate));
            CreateMap<Models.FiasModels.XmlModels.HousesParams.PARAMSPARAM, HouseParamsEntity>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.ID))
                .ForMember(x => x.ObjectId, s => s.MapFrom(x => x.OBJECTID))
                .ForMember(x => x.ChangeId, s => s.MapFrom(x => x.CHANGEID))
                .ForMember(x => x.ChangeIdEnd, s => s.MapFrom(x => x.CHANGEIDEND))
                .ForMember(x => x.TypeId, s => s.MapFrom(x => x.TYPEID))
                .ForMember(x => x.Value, s => s.MapFrom(x => x.VALUE))
                .ForMember(x => x.UpdateDate, s => s.MapFrom(x => x.UPDATEDATE))
                .ForMember(x => x.StartDate, s => s.MapFrom(x => x.STARTDATE))
                .ForMember(x => x.EndDate, s => s.MapFrom(x => x.ENDDATE));

            CreateMap<ParamTypesEntity, PARAMTYPESPARAMTYPE>()
                .ForMember(x => x.ID, s => s.MapFrom(x => x.Id))
                .ForMember(x => x.NAME, s => s.MapFrom(x => x.Name))
                .ForMember(x => x.DESC, s => s.MapFrom(x => x.Desc))
                .ForMember(x => x.CODE, s => s.MapFrom(x => x.Code))
                .ForMember(x => x.ISACTIVE, s => s.MapFrom(x => x.IsActive))
                .ForMember(x => x.UPDATEDATE, s => s.MapFrom(x => x.UpdateDate))
                .ForMember(x => x.STARTDATE, s => s.MapFrom(x => x.StartDate))
                .ForMember(x => x.ENDDATE, s => s.MapFrom(x => x.EndDate));
            CreateMap<PARAMTYPESPARAMTYPE, ParamTypesEntity>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.ID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.NAME))
                .ForMember(x => x.Desc, s => s.MapFrom(x => x.DESC))
                .ForMember(x => x.Code, s => s.MapFrom(x => x.CODE))
                .ForMember(x => x.IsActive, s => s.MapFrom(x => x.ISACTIVE))
                .ForMember(x => x.UpdateDate, s => s.MapFrom(x => x.UPDATEDATE))
                .ForMember(x => x.StartDate, s => s.MapFrom(x => x.STARTDATE))
                .ForMember(x => x.EndDate, s => s.MapFrom(x => x.ENDDATE));
        }
    }
}

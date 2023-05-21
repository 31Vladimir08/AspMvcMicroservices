namespace Fias.Api.Entities
{
    public class HouseEntity
    {
        public uint Id { get; set; }
        
        public uint ObjectId { get; set; }
        
        public string ObjectGuid { get; set; }
        
        public uint ChangeId { get; set; }
        
        public string HouseNum { get; set; }
        
        public byte HouseType { get; set; }
        
        public byte OperTypeId { get; set; }
        
        public uint PrevId { get; set; }
        
        public uint NextId { get; set; }
        
        public bool NextIdSpecified { get; set; }
        
        public DateTime UpdateDate { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public byte IsActual { get; set; }
        
        public byte IsActive { get; set; }
        
        public byte AddNum1 { get; set; }
        
        public bool AddNum1Specified { get; set; }
        
        public byte AddType1 { get; set; }
        
        public bool AddType1Specified { get; set; }
    }
}

namespace Fias.Api.Entities
{
    public class AddrObjParamEntity
    {
        public uint Id { get; set; }
        
        public uint ObjectId { get; set; }
        
        public uint ChangeId { get; set; }
       
        public uint ChangeIdEnd { get; set; }
        
        public byte TypeId { get; set; }
        
        public string Value { get; set; }
       
        public DateTime UpdateDate { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
    }
}

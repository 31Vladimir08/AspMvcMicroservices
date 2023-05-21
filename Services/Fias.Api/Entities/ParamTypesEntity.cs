namespace Fias.Api.Entities
{
    public class ParamTypesEntity
    {
        public byte Id { get; set; }
        
        public string Name { get; set; }
        
        public string Desc { get; set; }
        
        public string Code { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime UpdateDate { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }        
    }
}

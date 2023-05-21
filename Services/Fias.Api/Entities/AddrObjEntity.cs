namespace Fias.Api.Entities
{
    public class AddrObjEntity
    {
        public uint Id { get; set; }

        public uint ObjectId { get; set; }

        public string ObjectGuid { get; set; }

        public uint ChangeId { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }

        public byte Level { get; set; }

        public byte OperTypeId { get; set; }

        public uint PrevId { get; set; }

        public uint NextId { get; set; }

        public bool NextIdSpecified { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public byte IsActual { get; set; }

        public byte IsActive { get; set; }
    }
}

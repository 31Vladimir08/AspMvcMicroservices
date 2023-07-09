using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fias.Api.Entities
{
    public class AddrObjEntity : BaseRegionEntity
    {
        public uint ObjectId { get; set; }

        public string ObjectGuid { get; set; }

        public uint ChangeId { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }

        public byte Level { get; set; }

        public byte OperTypeId { get; set; }

        public uint PrevId { get; set; }

        public uint NextId { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public byte IsActual { get; set; }

        public byte IsActive { get; set; }
    }

    public class AddrObjEntitySQLiteConfig : IEntityTypeConfiguration<AddrObjEntity>
    {
        public void Configure(EntityTypeBuilder<AddrObjEntity> builder)
        {
            builder.ToTable("AS_ADDR_OBJ")
                .HasKey(x => x.Id);
            builder.HasIndex(x => x.ObjectId);

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .HasColumnType("INTEGER");
            builder.Property(s => s.ObjectId)
                .HasColumnName("OBJECTID")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.ObjectGuid)
                .HasColumnName("OBJECTGUID")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.ChangeId)
                .HasColumnName("CHANGEID")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.Name)
                .HasColumnName("NAME")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.TypeName)
                .HasColumnName("TYPENAME")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.Level)
                .HasColumnName("LEVEL")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.OperTypeId)
                .HasColumnName("OPERTYPEID")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.PrevId)
                .HasColumnName("PREVID")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.NextId)
                .HasColumnName("NEXTID")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.UpdateDate)
                .HasColumnName("UPDATEDATE")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired(); 
            builder.Property(s => s.RegionCode)
                .HasColumnName("REGIONCODE")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.StartDate)
                .HasColumnName("STARTDATE")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.EndDate)
                .HasColumnName("ENDDATE")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.IsActual)
                .HasColumnName("ISACTUAL")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.IsActive)
                .HasColumnName("ISACTIVE")
                .HasColumnType("INTEGER")
                .IsRequired();
        }
    }

    public class AddrObjEntityMSSQLConfig : IEntityTypeConfiguration<AddrObjEntity>
    {
        public void Configure(EntityTypeBuilder<AddrObjEntity> builder)
        {
            builder.ToTable("AS_ADDR_OBJ", "dbo")
                .HasKey(x => x.Id);
            builder.HasIndex(x => x.ObjectId);

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .HasColumnType("INT");
            builder.Property(s => s.ObjectId)
                .HasColumnName("OBJECTID")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.RegionCode)
                .HasColumnName("REGIONCODE")
                .HasColumnType("NVARCHAR(3)")
                .HasMaxLength(3)
                .IsRequired();
            builder.Property(s => s.ObjectGuid)
                .HasColumnName("OBJECTGUID")
                .HasColumnType("NVARCHAR(200)")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.ChangeId)
                .HasColumnName("CHANGEID")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.Name)
                .HasColumnName("NAME")
                .HasColumnType("NVARCHAR(200)")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.TypeName)
                .HasColumnName("TYPENAME")
                .HasColumnType("NVARCHAR(200)")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.Level)
                .HasColumnName("LEVEL")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.OperTypeId)
                .HasColumnName("OPERTYPEID")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.PrevId)
                .HasColumnName("PREVID")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.NextId)
                .HasColumnName("NEXTID")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.UpdateDate)
                .HasColumnName("UPDATEDATE")
                .HasColumnType("DATETIME")
                .IsRequired();
            builder.Property(s => s.StartDate)
                .HasColumnName("STARTDATE")
                .HasColumnType("DATETIME")
                .IsRequired();
            builder.Property(s => s.EndDate)
                .HasColumnName("ENDDATE")
                .HasColumnType("DATETIME")
                .IsRequired();
            builder.Property(s => s.IsActual)
                .HasColumnName("ISACTUAL")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.IsActive)
                .HasColumnName("ISACTIVE")
                .HasColumnType("INT")
                .IsRequired();
        }
    }
}

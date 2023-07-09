using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fias.Api.Entities
{
    public class HouseParamEntity : BaseRegionEntity
    {
        public uint ObjectId { get; set; }
        
        public uint ChangeId { get; set; }
        
        public uint ChangeIdEnd { get; set; }
        
        public byte TypeId { get; set; }
        
        public string Value { get; set; }
        
        public DateTime UpdateDate { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
    }

    public class HouseParamsEntitySQLiteConfig : IEntityTypeConfiguration<HouseParamEntity>
    {
        public void Configure(EntityTypeBuilder<HouseParamEntity> builder)
        {
            builder.ToTable("AS_HOUSES_PARAMS")
                .HasKey(x => x.Id);
            builder.HasIndex(x => x.ObjectId);

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .HasColumnType("INTEGER");
            builder.Property(s => s.ObjectId)
                .HasColumnName("OBJECTID")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.ChangeId)
                .HasColumnName("CHANGEID")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.ChangeIdEnd)
                .HasColumnName("CHANGEIDEND")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.TypeId)
                .HasColumnName("TYPEID")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.Value)
                .HasColumnName("VALUE")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.UpdateDate)
                .HasColumnName("UPDATEDATE")
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
            builder.Property(s => s.RegionCode)
                .HasColumnName("REGIONCODE")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
        }
    }

    public class HouseParamsEntityMSSQLConfig : IEntityTypeConfiguration<HouseParamEntity>
    {
        public void Configure(EntityTypeBuilder<HouseParamEntity> builder)
        {
            builder.ToTable("AS_HOUSES_PARAMS", "dbo")
                .HasKey(x => x.Id);
            builder.HasIndex(x => x.ObjectId);

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .HasColumnType("INT");
            builder.Property(s => s.ObjectId)
                .HasColumnName("OBJECTID")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.ChangeId)
                .HasColumnName("CHANGEID")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.ChangeIdEnd)
                .HasColumnName("CHANGEIDEND")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.TypeId)
                .HasColumnName("TYPEID")
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(s => s.Value)
                .HasColumnName("VALUE")
                .HasColumnType("NVARCHAR(200)")
                .HasMaxLength(200)
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
            builder.Property(s => s.RegionCode)
                .HasColumnName("REGIONCODE")
                .HasColumnType("NVARCHAR(3)")
                .HasMaxLength(3)
                .IsRequired();
        }
    }
}

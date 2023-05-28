using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fias.Api.Entities
{
    public class ParamTypesEntity : BaseEntity
    {
        public string Name { get; set; }
        
        public string Desc { get; set; }
        
        public string Code { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime UpdateDate { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }        
    }

    public class ParamTypesEntitySQLiteConfig : IEntityTypeConfiguration<ParamTypesEntity>
    {
        public void Configure(EntityTypeBuilder<ParamTypesEntity> builder)
        {
            builder.ToTable("AS_PARAM_TYPES");
            builder.HasIndex(x => x.Id)
                .IsUnique();

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .HasColumnType("INTEGER");
            builder.Property(s => s.Name)
                .HasColumnName("NAME")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.Desc)
                .HasColumnName("DESC")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.Code)
                .HasColumnName("CODE")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.IsActive)
                .HasColumnName("ISACTIVE")
                .HasColumnType("INTEGER")
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
        }
    }

    public class ParamTypesEntityMSSQLConfig : IEntityTypeConfiguration<ParamTypesEntity>
    {
        public void Configure(EntityTypeBuilder<ParamTypesEntity> builder)
        {
            builder.ToTable("AS_PARAM_TYPES");
            builder.HasIndex(x => x.Id)
                .IsUnique();

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .HasColumnType("INT");
            builder.Property(s => s.Name)
                .HasColumnName("NAME")
                .HasColumnType("NVARCHAR(200)")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.Desc)
                .HasColumnName("DESC")
                .HasColumnType("NVARCHAR(200)")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.Code)
                .HasColumnName("CODE")
                .HasColumnType("NVARCHAR(200)")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.IsActive)
                .HasColumnName("ISACTIVE")
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
        }
    }
}

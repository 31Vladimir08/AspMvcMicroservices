using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fias.Api.Interfaces.Entities;

namespace Fias.Api.Entities
{
    public class ParamTypesEntity : IEntity
    {
        public uint Id { get; set; }
        
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
            builder.ToTable("AS_PARAM_TYPES")
                .HasKey(x => x.Id);

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .HasColumnType("INTEGER");
            builder.Property(s => s.Name)
                .HasColumnName("NAME")
                .HasColumnType("TEXT")
                .IsRequired();
            builder.Property(s => s.Desc)
                .HasColumnName("DESC")
                .HasColumnType("TEXT")
                .IsRequired();
            builder.Property(s => s.Code)
                .HasColumnName("CODE")
                .HasColumnType("TEXT")
                .IsRequired();
            builder.Property(s => s.IsActive)
                .HasColumnName("ISACTIVE")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.UpdateDate)
                .HasColumnName("UPDATEDATE")
                .HasColumnType("TEXT")
                .IsRequired();
            builder.Property(s => s.StartDate)
                .HasColumnName("STARTDATE")
                .HasColumnType("TEXT")
                .IsRequired();
            builder.Property(s => s.EndDate)
                .HasColumnName("ENDDATE")
                .HasColumnType("TEXT")
                .IsRequired();
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fias.Api.Entities
{
    public class AddrObjParamEntity : BaseEntity
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

    public class AddrObjParamEntitySQLiteConfig : IEntityTypeConfiguration<AddrObjParamEntity>
    {
        public void Configure(EntityTypeBuilder<AddrObjParamEntity> builder)
        {
            builder.ToTable("AS_ADDR_OBJ_PARAMS")
                .HasKey(x => x.Id);
            builder.HasIndex(x => x.Id)
                /*.IsUnique()*/;

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
        }
    }
}
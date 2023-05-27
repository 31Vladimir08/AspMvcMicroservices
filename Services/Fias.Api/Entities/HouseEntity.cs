﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fias.Api.Interfaces.Entities;

namespace Fias.Api.Entities
{
    public class HouseEntity : IEntity
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

    public class HouseEntitySQLiteConfig : IEntityTypeConfiguration<HouseEntity>
    {
        public void Configure(EntityTypeBuilder<HouseEntity> builder)
        {
            builder.ToTable("AS_HOUSES")
                .HasKey(x => x.Id);
            builder.HasIndex(x => x.ObjectId);
            builder.HasIndex(x => x.ObjectGuid);

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
            builder.Property(s => s.HouseNum)
                .HasColumnName("HOUSENUM")
                .HasColumnType("TEXT")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(s => s.HouseType)
                .HasColumnName("HOUSETYPE")
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(s => s.OperTypeId)
                .HasColumnName("LEVEL")
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
                .IsRequired();
            builder.Property(s => s.StartDate)
                .HasColumnName("STARTDATE")
                .HasColumnType("TEXT")
                .IsRequired();
            builder.Property(s => s.EndDate)
                .HasColumnName("ENDDATE")
                .HasColumnType("TEXT")
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
}

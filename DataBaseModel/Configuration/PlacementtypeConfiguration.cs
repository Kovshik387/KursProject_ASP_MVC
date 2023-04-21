using DataBaseModel.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModel.Configuration
{
    public class PlacementtypeConfiguration : IEntityTypeConfiguration<Entity.Placementtype>
    {
        public PlacementtypeConfiguration() : base() { }

        public virtual void Configure(EntityTypeBuilder<Placementtype> entity)
        {
            entity.HasKey(e => e.IdType).HasName("placementtype_pkey");

            entity.ToTable("placementtype");

            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.Type)
                .HasMaxLength(15)
                .HasColumnName("type");
        }
    }
}

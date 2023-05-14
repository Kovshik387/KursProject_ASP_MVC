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
    public class TenantConfiguration : IEntityTypeConfiguration<Entity.Tenant>
    {
        public TenantConfiguration() : base() { }

        public virtual void Configure(EntityTypeBuilder<Tenant> entity)
        {
            entity.HasKey(e => e.IdT).HasName("tenant_pkey");

            entity.ToTable("tenant");

            entity.Property(e => e.IdT).HasColumnName("id_t");
            entity.Property(e => e.IdU).HasColumnName("id_u");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.IdUNavigation).WithMany(p => p.Tenants)
                .HasForeignKey(d => d.IdU)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tenant_id_u_fkey");
        }
    }
}

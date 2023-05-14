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
    public class RenterConfiguration : IEntityTypeConfiguration<Entity.Renter>
    {
        public RenterConfiguration() : base() { }

        public virtual void Configure(EntityTypeBuilder<Renter> entity)
        {
            entity.HasKey(e => e.IdR).HasName("renter_pkey");

            entity.ToTable("renter");

            entity.HasIndex(e => e.License, "renter_license_key").IsUnique();

            entity.Property(e => e.IdR).HasColumnName("id_r");
            entity.Property(e => e.IdU).HasColumnName("id_u");
            entity.Property(e => e.License).HasColumnName("license");

            entity.HasOne(d => d.IdUNavigation).WithMany(p => p.Renters)
                .HasForeignKey(d => d.IdU)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("renter_id_u_fkey");
        }
    }
}

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
    public class PlacementConfiguration : IEntityTypeConfiguration<Entity.Placement>
    {
        public PlacementConfiguration() : base() { }
        public virtual void Configure(EntityTypeBuilder<Placement> entity)
        {
            entity.HasKey(e => e.IdP).HasName("placement_pkey");

            entity.ToTable("placement");

            entity.Property(e => e.IdP).HasColumnName("id_p");
            entity.Property(e => e.Area)
                .HasMaxLength(20)
                .HasColumnName("area");
            entity.Property(e => e.Floor).HasColumnName("floor");
            entity.Property(e => e.IdR).HasColumnName("id_r");
            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Room).HasColumnName("room");
            entity.Property(e => e.Square)
                .HasMaxLength(15)
                .HasColumnName("square");
            entity.Property(e => e.Street)
                .HasMaxLength(20)
                .HasColumnName("street");

            entity.HasOne(d => d.IdRNavigation).WithMany(p => p.Placements)
                .HasForeignKey(d => d.IdR)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("placement_id_r_fkey");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Placements)
                .HasForeignKey(d => d.IdType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("placement_id_type_fkey");
        }
    }
}

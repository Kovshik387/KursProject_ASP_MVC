using DataBaseModel.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModel.Configuration
{
    public class AuthorizationConfiguration : IEntityTypeConfiguration<Entity.Authorization>
    {
        public AuthorizationConfiguration() :base() { }

        public virtual void Configure(EntityTypeBuilder<Authorization> entity)
        {
            entity.HasKey(e => e.IdA).HasName("authorizations_pkey");

            entity.ToTable("authorizations");

            entity.Property(e => e.IdA).HasColumnName("id_a");
            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.IdU).HasColumnName("id_u");
            entity.Property(e => e.Loginuser)
                .HasMaxLength(20)
                .HasColumnName("loginuser");
            entity.Property(e => e.Passworduser)
                .HasMaxLength(20)
                .HasColumnName("passworduser");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Authorizations)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("authorizations_id_type_fkey");

            entity.HasOne(d => d.IdUNavigation).WithMany(p => p.Authorizations)
                .HasForeignKey(d => d.IdU)
                .HasConstraintName("authorizations_id_u_fkey");
        }
    }
}

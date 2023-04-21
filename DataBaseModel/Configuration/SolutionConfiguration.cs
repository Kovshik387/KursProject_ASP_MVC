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
    public class SolutionConfiguration : IEntityTypeConfiguration<Entity.Solution>
    {
        public SolutionConfiguration() : base() { }

        public virtual void Configure(EntityTypeBuilder<Solution> entity)
        {
            entity.HasKey(e => e.IdS).HasName("solution_pkey");

            entity.ToTable("solution");

            entity.Property(e => e.IdS).HasColumnName("id_s");
            entity.Property(e => e.Datesolution).HasColumnName("datesolution");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdR).HasColumnName("id_r");
            entity.Property(e => e.IdT).HasColumnName("id_t");

            entity.HasOne(d => d.IdRNavigation).WithMany(p => p.Solutions)
                .HasForeignKey(d => d.IdR)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solution_id_r_fkey");

            entity.HasOne(d => d.IdTNavigation).WithMany(p => p.Solutions)
                .HasForeignKey(d => d.IdT)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solution_id_t_fkey");
        }
    }
}

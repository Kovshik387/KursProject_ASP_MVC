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
    public class ContractConfiguration : IEntityTypeConfiguration<Entity.Contract>
    { 
        public ContractConfiguration() : base() { }
        public virtual void Configure(EntityTypeBuilder<Entity.Contract> entity)
        {

            entity.HasKey(e => e.IdC).HasName("contract_pkey");

            entity.ToTable("contract");

            entity.Property(e => e.IdC).HasColumnName("id_c");
            entity.Property(e => e.IdP).HasColumnName("id_p");
            entity.Property(e => e.IdPay).HasColumnName("id_pay");
            entity.Property(e => e.IdS).HasColumnName("id_s");
            entity.Property(e => e.Paymentsize).HasColumnName("paymentsize");

            entity.HasOne(d => d.IdPNavigation).WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.IdP)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contract_id_p_fkey");

            entity.HasOne(d => d.IdPayNavigation).WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.IdPay)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contract_id_pay_fkey");

            entity.HasOne(d => d.IdSNavigation).WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.IdS)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contract_id_s_fkey");
        }
    }
}
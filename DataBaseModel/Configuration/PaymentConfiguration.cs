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
    public class PaymentConfiguration : IEntityTypeConfiguration<Entity.Payment>
    {
        public PaymentConfiguration() : base() { }
        public virtual void Configure(EntityTypeBuilder<Payment> entity)
        {
            entity.HasKey(e => e.IdPay).HasName("payment_pkey");

            entity.ToTable("payment");

            entity.Property(e => e.IdPay).HasColumnName("id_pay");
            entity.Property(e => e.Paymentmethod)
                    .HasMaxLength(15)
                    .HasColumnName("paymentmethod");
        }
    }
}

using DataBaseModel.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBaseModel.Configuration
{
    public class AuthorizationTypeConfiguration : IEntityTypeConfiguration<Entity.Authorizationtype>
    {
        public AuthorizationTypeConfiguration() : base() { }

        public virtual void Configure(EntityTypeBuilder<Authorizationtype> entity)
        {
            entity.HasKey(e => e.IdType).HasName("authorizationtype_pkey");

            entity.ToTable("authorizationtype");

            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .HasColumnName("type");
        }
    }
}

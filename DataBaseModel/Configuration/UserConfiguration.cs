using DataBaseModel.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBaseModel.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<Entity.User>
    {
        public  UserConfiguration() : base() { }

        public virtual void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.IdU).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.IdU).HasColumnName("id_u");
            entity.Property(e => e.Contact)
                .HasMaxLength(12)
                .HasColumnName("contact");
            entity.Property(e => e.Name)
                .HasMaxLength(15)
                .HasColumnName("name");
            entity.Property(e => e.Sex)
                .HasMaxLength(1)
                .HasColumnName("sex");
            entity.Property(e => e.Surname)
                .HasMaxLength(15)
                .HasColumnName("surname");
        }
    }
}

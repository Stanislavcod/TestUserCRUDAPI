using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.DAL.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles")
                .HasKey(x => x.Id);

            builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(50);
        }
    }
}

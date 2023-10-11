using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.DAL.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users")
                .HasKey(x => x.Id);

            builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(20);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.Age)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}

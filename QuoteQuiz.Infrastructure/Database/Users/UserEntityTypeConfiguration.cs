using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Infrastructure.Database.Users
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn().IsRequired();
            builder.HasKey(x => x.Id).HasName("Pk_User_Id");

            builder.Property(x => x.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Salt).HasColumnName("Salt").IsRequired(true).HasMaxLength(40).IsRequired(true);
            builder.HasIndex(x => x.Login).IsUnique();
            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(100).IsRequired(true);
            builder.HasMany(x => x.RefreshTokens).WithOne().HasForeignKey("Fk_User_Id");
            builder.HasMany(x => x.Answers).WithOne().HasForeignKey(x => x.UserId);
        }
    }
}

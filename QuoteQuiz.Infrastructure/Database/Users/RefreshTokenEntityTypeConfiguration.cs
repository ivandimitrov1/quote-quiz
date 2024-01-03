using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Infrastructure.Database.Users
{
    public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken");

            builder.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn().IsRequired();
            builder.HasKey(x => x.Id).HasName("Pk_RefreshToken_Id");

            builder.Property(x => x.TokenHash).HasColumnName("TokenHash").IsRequired();
            builder.Property(x => x.Expires).HasColumnName("Expires").IsRequired();
            builder.Property(x => x.Created).HasColumnName("Created").IsRequired();
        }
    }
}

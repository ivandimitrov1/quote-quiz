using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Infrastructure.Database.Quizes
{
    public class QuizEntityTypeConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.ToTable("Quiz");

            builder.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn().IsRequired();
            builder.HasKey(x => x.Id).HasName("Pk_Quiz_Id");

            builder.Property(x => x.Title).HasColumnName("Title").HasMaxLength(200).IsRequired(true);
            builder.Property(x => x.Published).HasColumnName("Published").IsRequired(true);
            builder.HasMany(x => x.Quotes).WithOne().HasForeignKey(x => x.QuizId).IsRequired(true);
        }
    }
}

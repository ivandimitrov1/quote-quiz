using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Infrastructure.Database.UserAchievements
{
    public class UserAnswerEntityTypeConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.ToTable("UserAnswer");

            builder.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn().IsRequired();
            builder.HasKey(x => x.Id).HasName("Pk_UserAnswer_Id");

            builder.Property(x => x.Answer).HasColumnName("UserAnswer");
            builder.Property(x => x.UserId).HasColumnName("Fk_User_Id").IsRequired(true);
            builder.Property(x => x.QuizId).HasColumnName("Fk_Quiz_Id");
            builder.Property(x => x.QuoteId).HasColumnName("Fk_Quote_Id");
            builder.Property(x => x.QuizFinished).HasColumnName("QuizFinished");

            builder.Property(x => x.OnDate)
                .HasColumnName("OnDate")
                .IsRequired(true);

            builder.HasOne(x => x.Quiz)
                .WithMany()
                .HasForeignKey(x => x.QuizId);
                
            builder.HasOne(x => x.Quote)
                .WithMany()
                .HasForeignKey(x => x.QuoteId)
                .IsRequired(false);
        }
    }
}

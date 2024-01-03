using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using QuoteQuiz.Application.Domain;
using Newtonsoft.Json;

namespace QuoteQuiz.Infrastructure.Database.Quizes
{
    public class QuoteEntityTypeConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {
            builder.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn().IsRequired();
            builder.HasKey(x => x.Id).HasName("Pk_Quote_Id");

            builder.Property(x => x.Text).HasColumnName("Text").IsRequired(true);
            builder.Property(x => x.Answers)
                .HasConversion(
                    data => JsonConvert.SerializeObject(data),
                    dataAsJson => JsonConvert.DeserializeObject<List<string>>(dataAsJson))
                .HasColumnName("Answers")
                .IsRequired(true);
        }
    }
}

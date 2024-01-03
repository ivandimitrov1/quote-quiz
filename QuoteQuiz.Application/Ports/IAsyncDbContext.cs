namespace QuoteQuiz.Application.Ports
{
    public interface IAsyncDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

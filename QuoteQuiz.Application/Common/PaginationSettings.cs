namespace QuoteQuiz.Api.Core
{
    public class PaginationSettings
    {
        public PaginationSettings(int? pageId, int? pageSize)
        {
            TakeCount = pageSize;
            SkipCount = pageId.HasValue && pageSize.HasValue ? pageId.Value * pageSize.Value : (int?)null;
        }

        public int? SkipCount { get; }

        public int? TakeCount { get; }
    }
}

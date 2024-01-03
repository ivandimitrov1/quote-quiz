using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Api.UserManagement.ViewModels
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IdValuePair Role { get; set; }
    }
}

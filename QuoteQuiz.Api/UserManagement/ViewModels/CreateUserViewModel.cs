using QuoteQuiz.Application.Domain;
using System.ComponentModel.DataAnnotations;

namespace QuoteQuiz.Api.UserManagement.ViewModels
{
    public class CreateUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        public string? Name { get; set; }

        [Required(ErrorMessage = "Key is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The key and confirmation key do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

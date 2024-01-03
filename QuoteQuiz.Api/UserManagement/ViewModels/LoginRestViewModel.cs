using System.ComponentModel.DataAnnotations;

namespace QuoteQuiz.Api.UserManagement.ViewModels
{
    public class LoginRestViewModel
    {
        [Required(ErrorMessage = "Login is required.")]
        public string Login { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}

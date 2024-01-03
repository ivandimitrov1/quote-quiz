using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Api.UserManagement.ViewModels
{
    public class UserDataViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public IdValuePair Role { get; set; }

        public static UserDataViewModel ToViewModel(User user)
        {
            return new UserDataViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Login = user.Login,
                Role = new IdValuePair
                {
                    Id = (int)user.Role,
                    Value = user.Role.ToString()
                }
            };
        }
    }
}

using UserAuthApp.Data.Dao;

namespace UserAuthApp.Api.Models.UserModels
{
    public class UserParamApiModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public User ToDao(int id=0)
            => new User
            {
                UserId = id,
                Email = Email,  
                UserName = UserName,    
                PasswordHash = Password
            };
    }
}

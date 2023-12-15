using NotificationApp.Data.Api.UserApiRepo.Models;
using NotificationApp.Data.Dao;

namespace NotificationApp.Data.Api.UserApiRepo
{
    public interface IUserApiRepo
    {
        public Task<User> Read(int id, CancellationToken c, bool isIncludeRole = false);
        public Task<List<User>> List(UserListParamApiDataModel p, CancellationToken c);
    }
}

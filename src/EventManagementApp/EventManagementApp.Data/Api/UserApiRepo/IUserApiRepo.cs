using EventManagementApp.Data.Api.UserApiRepo.Models;
using EventManagementApp.Data.Dao;

namespace EventManagementApp.Data.Api.UserApiRepo
{
    public interface IUserApiRepo
    {
        public Task<User> Read(int id, CancellationToken c, bool isIncludeRole = false);
        public Task<List<User>> List(UserListParamApiDataModel p, CancellationToken c);
    }
}

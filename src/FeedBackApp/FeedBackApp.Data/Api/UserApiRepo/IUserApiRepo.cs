using FeedBackApp.Data.Api.UserApiRepo.Models;
using FeedBackApp.Data.Dao;

namespace FeedBackApp.Data.Api.UserApiRepo
{
    public interface IUserApiRepo
    {
        public Task<User> Read(int id, CancellationToken c, bool isIncludeRole = false);
        public Task<List<User>> List(UserListParamApiDataModel p, CancellationToken c);
    }
}

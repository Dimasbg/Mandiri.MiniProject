using FeedBackApp.Data.Api.UserApiRepo.Models;
using FeedBackApp.Data.Dao;
using Mandiri.MiniProject.Utilities.Base;

namespace FeedBackApp.Data.Api.UserApiRepo.Impl
{
    public class UserApiRepoImpl : BaseApiCall, IUserApiRepo
    {
        private readonly IHttpClientFactory _hc;

        public UserApiRepoImpl(IHttpClientFactory hc)
        {
            _hc = hc;
        }

        private static string GenerateEndPoint(string route = "")
            => $"/V1/User{route}";

        public async Task<List<User>> List(UserListParamApiDataModel p, CancellationToken c)
        {
            var cl = _hc.CreateClient("UserAuth.App");

            return await CallEndpoint<List<User>>(cl.GetAsync(GenerateEndPoint($"{p.ToQueryString()}"), c), c);
        }

        public async Task<User> Read(int id, CancellationToken c, bool isIncludeRole = false)
        {
            var cl = _hc.CreateClient("UserAuth.App");

            return await CallEndpoint<User>(cl.GetAsync(GenerateEndPoint($"/{id}"), c), c);
        }
    }
}

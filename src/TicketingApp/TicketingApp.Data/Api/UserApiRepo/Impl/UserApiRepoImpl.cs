using Mandiri.MiniProject.Utilities.Base;
using TicketingApp.Data.Api.UserApiRepo.Models;
using TicketingApp.Data.Dao;

namespace TicketingApp.Data.Api.UserApiRepo.Impl
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

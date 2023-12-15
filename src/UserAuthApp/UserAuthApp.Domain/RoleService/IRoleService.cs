using UserAuthApp.Data.Dao;
using UserAuthApp.Domain.RoleService.Models;

namespace UserAuthApp.Domain.RoleService
{
    public interface IRoleService
    {
        public Task<Role> Create(string name, CancellationToken c);
        public Task<Role> Read(int id, CancellationToken c, bool isIncludeUser = false);
        public Task<Role> Update(Role d, CancellationToken c);
        public Task Delete(int id, CancellationToken c);
        public Task<List<Role>> List(RoleListParamDomainModel p, CancellationToken c);
    }
}

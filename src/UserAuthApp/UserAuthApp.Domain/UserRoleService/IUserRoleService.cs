using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthApp.Domain.UserRoleService
{
    public interface IUserRoleService
    {
        public Task AssignUserToRole(int userId, int roleId, CancellationToken c);
        public Task RevokeUserFromRole(int userId, int roleId, CancellationToken c);
    }
}

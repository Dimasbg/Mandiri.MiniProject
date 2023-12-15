using Mandiri.MiniProject.Utilities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthApp.Data.Dao;
using UserAuthApp.Data.Db;

namespace UserAuthApp.Domain.UserRoleService.Impl
{
    public class UserRoleServiceImpl : IUserRoleService
    {
        private readonly MiniProjectDbContext _db;

        public UserRoleServiceImpl(MiniProjectDbContext db)
        {
            _db = db;
        }

        public async Task AssignUserToRole(int userId, int roleId, CancellationToken c)
        {
            var user = await _db.Users.FirstOrDefaultAsync(b => b.UserId == userId, c)
                ?? throw new DataNotFoundException($"User with id '{userId}' not found!");
            var role = await _db.Roles.FirstOrDefaultAsync(b => b.RoleId == roleId, c)
                ?? throw new DataNotFoundException($"Role with id '{roleId}' not found!");

            var exist = await _db.UserRoles.FirstOrDefaultAsync(b => b.User.UserId == userId && b.Role.RoleId == roleId, c);

            if (exist != null)
                return;

            await _db.UserRoles.AddAsync(new UserRole { Role = role, User = user });
            await _db.SaveChangesAsync(c);
        }

        public async Task RevokeUserFromRole(int userId, int roleId, CancellationToken c)
        {
            var exist = await _db.UserRoles.FirstOrDefaultAsync(b => b.User.UserId == userId && b.Role.RoleId == roleId, c);

            if (exist == null)
                return;

            _db.UserRoles.Remove(exist);
            await _db.SaveChangesAsync(c);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthApp.Data.Dao;
using UserAuthApp.Domain.UserService.Models;

namespace UserAuthApp.Domain.UserService
{
    public interface IUserService
    {
        public Task<User> Login(string email, string password, CancellationToken c);
        public Task<User> ChangePasswordByEmail(string email, string oldPassword, string newPassword, CancellationToken c);
        public Task<User> ChangePasswordById(int id, string oldPassword, string newPassword, CancellationToken c);
        public Task<User> Create(User d, CancellationToken c);
        public Task<User> Read(int id, CancellationToken c, bool isIncludeRole = false);
        public Task<User> Update(User d, CancellationToken c);
        public Task Delete(int id, CancellationToken c);
        public Task<List<User>> List(UserListParamDomainModel p, CancellationToken c);
    }
}

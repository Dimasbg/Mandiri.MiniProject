using BCrypt.Net;
using Mandiri.MiniProject.Utilities.Base;
using Microsoft.EntityFrameworkCore;
using UserAuthApp.Data.Dao;
using UserAuthApp.Data.Db;
using UserAuthApp.Domain.UserService.Models;

namespace UserAuthApp.Domain.UserService.Impl
{
    public class UserServiceImpl : IUserService
    {
        private readonly MiniProjectDbContext _db;

        public UserServiceImpl(MiniProjectDbContext db)
        {
            _db = db;
        }

        public async Task<User> ChangePasswordByEmail(string email, string oldPassword, string newPassword, CancellationToken c)
        {
            var result = await _db.Users.FirstOrDefaultAsync(b => b.Email.ToUpper() == email.ToUpper(), c)
                ?? throw new DataNotFoundException($"User with email '{email}' not found!");

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, result.PasswordHash))
                throw new DomainLayerException($"Password mismatch!");

            result.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _db.Users.Update(result);
            await _db.SaveChangesAsync(c);
            return result;
        }

        public async Task<User> ChangePasswordById(int id, string oldPassword, string newPassword, CancellationToken c)
        {
            var result = await Read(id, c)
                ?? throw new DataNotFoundException($"User with id '{id}' not found!");

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, result.PasswordHash))
                throw new DomainLayerException($"Password mismatch!");

            result.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _db.Users.Update(result);
            await _db.SaveChangesAsync(c);
            return result;
        }

        public async Task<User> Create(User d, CancellationToken c)
        {
            var data = await _db.Users.FirstOrDefaultAsync(b => b.UserName.ToUpper() == d.UserName.ToUpper() || b.Email.ToUpper() == d.Email.ToUpper());

            if (data != null)
            {
                if (data.UserName.ToUpper() == d.UserName.ToUpper() && data.Email.ToUpper() == d.Email.ToUpper())
                    throw new DomainLayerException($"Username & Email already exist!");


                if (data.UserName.ToUpper() == d.UserName.ToUpper())
                    throw new DomainLayerException($"Username already exist!");

                if (data.Email.ToUpper() == d.Email.ToUpper())
                    throw new DomainLayerException($"Email already exist!");
            }
            d.PasswordHash = BCrypt.Net.BCrypt.HashPassword(d.PasswordHash);
            await _db.Users.AddAsync(d, c);
            await _db.SaveChangesAsync(c);
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            var data = await Read(id, c);
            _db.Users.Remove(data);
            await _db.SaveChangesAsync(c);
        }

        public async Task<List<User>> List(UserListParamDomainModel p, CancellationToken c)
        {
            IQueryable<User> rawQuery = _db.Users;
            if (p.IsIncludeRole)
                rawQuery = rawQuery.Include(b => b.UserRoles).ThenInclude(b => b.Role);

            if (!string.IsNullOrWhiteSpace(p.UserName))
                rawQuery = rawQuery.Where(b => b.UserName!.ToUpper().Contains(p.UserName.ToUpper()));


            if (!string.IsNullOrWhiteSpace(p.Email))
                rawQuery = rawQuery.Where(b => b.Email!.ToUpper().Contains(p.Email.ToUpper()));


            return await rawQuery.ToListAsync(c);
        }

        public async Task<User> Login(string email, string password, CancellationToken c)
        {
            var result = await _db.Users.FirstOrDefaultAsync(b => b.Email.ToUpper() == email.ToUpper(), c)
                ?? throw new DataNotFoundException($"User with email '{email}' not found!");

            if (!BCrypt.Net.BCrypt.Verify(password, result.PasswordHash))
                throw new DomainLayerException($"Password mismatch!");

            return result;
        }

        public async Task<User> Read(int id, CancellationToken c, bool isIncludeRole = false)
        {
            IQueryable<User> rawQuery = _db.Users;
            if (isIncludeRole)
                rawQuery = rawQuery.Include(b => b.UserRoles).ThenInclude(b => b.Role);
            return await rawQuery.FirstOrDefaultAsync(b => b.UserId == id, c);
        }

        public async Task<User> Update(User d, CancellationToken c)
        {
            var result = await Read(d.UserId, c)
                ?? throw new DataNotFoundException($"User with id '{d.UserId}' not found!");


            var data = await _db.Users.FirstOrDefaultAsync(b => b.UserId != d.UserId && (b.UserName.ToUpper() == d.UserName.ToUpper() || b.Email.ToUpper() == d.Email.ToUpper()));

            if (data != null)
            {
                if (data.UserName.ToUpper() == d.UserName.ToUpper() && data.Email.ToUpper() == d.Email.ToUpper())
                    throw new DomainLayerException($"Username & Email already exist!");

                if (data.UserName.ToUpper() == d.UserName.ToUpper())
                    throw new DomainLayerException($"Username already exist!");

                if (data.Email.ToUpper() == d.Email.ToUpper())
                    throw new DomainLayerException($"Email already exist!");
            }

            result.Update(d);
            _db.Users.Update(result);
            await _db.SaveChangesAsync(c);
            return result;
        }
    }
}

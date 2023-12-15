using Dapper;
using Mandiri.MiniProject.Utilities.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthApp.Data.Dao;
using UserAuthApp.Data.Db;
using UserAuthApp.Domain.UserService.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserAuthApp.Domain.UserService.Impl
{
    public class UserSeviceQueryImpl : IUserService
    {
        private readonly IConfiguration _config;
        private static string ConnectionString;
        private readonly MiniProjectDbContext _db;
        public UserSeviceQueryImpl(IConfiguration config, MiniProjectDbContext db)
        {
            _config = config;
            ConnectionString = _config.GetValue<string>("ConnectionStrings:Dev");
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
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var result = await Read(id, c)
                ?? throw new DataNotFoundException($"User with id '{id}' not found!");

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, result.PasswordHash))
                throw new DomainLayerException($"Password mismatch!");

            result.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            var updateQuery = "UPDATE Users SET UserName = @UserName,PasswordHash=@PasswordHash,Email=@Email; SELECT * FROM Users WHERE UserId= CAST(SCOPE_IDENTITY() AS INT)";
            return await connection.QueryFirstOrDefaultAsync<User>(updateQuery, result);
        }

        public async Task<User> Create(User d, CancellationToken c)
        {

            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var data = await connection.QueryFirstOrDefaultAsync<User>($"SELECT * FROM Users WHERE UserName = '{d.UserName}' OR Email = '{d.Email}'");

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

            var insertQuery = "INSERT INTO Users (UserName, PasswordHash,Email) VALUES (@UserName, @PasswordHash,@Email); SELECT * FROM Users WHERE UserId= CAST(SCOPE_IDENTITY() AS INT)";
            d = await connection.QueryFirstOrDefaultAsync<User>(insertQuery, d);
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<User>($"SELECT u.* FROM Users u WHERE UserId = {id}");
            if (result == null)
                return;
            await connection.ExecuteAsync($"DELETE FROM Users WHERE UserId = {id}");
            //var data = await Read(id, c);
            //_db.Users.Remove(data);
            //await _db.SaveChangesAsync(c);
        }

        public async Task<List<User>> List(UserListParamDomainModel p, CancellationToken c)
        {
            IEnumerable<User> rawData = new List<User>();
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string query = "SELECT u.*";

            IQueryable<User> rawQuery = _db.Users;

            if (p.IsIncludeRole)
                query += @" ,r.* FROM Users u  
                            LEFT JOIN UserRoles ur ON u.UserId = ur.UserId
                            LEFT JOIN Roles r ON ur.RoleId = r.RoleId ";
            else
                query += " FROM Users u ";


            if (!string.IsNullOrWhiteSpace(p.UserName) || !string.IsNullOrWhiteSpace(p.Email))
                query += " WHERE ";

            if (!string.IsNullOrWhiteSpace(p.UserName))
                query += $" UserName LIKE '%{p.UserName}%' ";

            if (!string.IsNullOrWhiteSpace(p.Email))
                query += $" Email LIKE '%{p.Email}%' ";

            if (p.IsIncludeRole)
                rawData = await connection.QueryAsync<User, Role, User>(query, (user, role) =>
                {
                    user.Roles.Add(role);
                    return user;
                }, splitOn: "RoleId");
            else
                rawData = await connection.QueryAsync<User>(query);


            return rawData.ToList();
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
            IEnumerable<User> rawData;
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string query = "SELECT u.* FROM Users u";

            if (isIncludeRole)
            {
                query = @"SELECT u.*,r.* FROM Users u  
                            LEFT JOIN UserRoles ur ON u.UserId = ur.UserId
                            LEFT JOIN Roles r ON ur.RoleId = r.RoleId";
                rawData = await connection.QueryAsync<User, Role, User>($"{query} WHERE u.UserId = '{id}'", (user, role) =>
                {
                    user.Roles.Add(role);
                    return user;
                }, splitOn: "RoleId");
            }
            else
                rawData = await connection.QueryAsync<User>($"{query} WHERE u.UserId = '{id}'");


            if (!rawData.Any())
                return null;

            return rawData.FirstOrDefault();
        }

        public async Task<User> Update(User d, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var result = await connection.QueryFirstOrDefaultAsync<User>($"SELECT u.* FROM Users u WHERE UserId = {d.UserId}")
                ?? throw new DataNotFoundException($"User with id '{d.UserId}' not found!");

            var data = await connection.QueryFirstOrDefaultAsync<User>($"SELECT u.* FROM Users u WHERE u.UserId != {d.UserId} AND (u.UserName LIKE '%{d.UserName}%' OR u.Email LIKE '%{d.Email}%') ");

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
            var updateQuery = $"UPDATE Users SET UserName = @UserName,PasswordHash=@PasswordHash,Email=@Email WHERE UserId= {d.UserId}; SELECT * FROM Users WHERE UserId= {d.UserId}";
            return await connection.QueryFirstOrDefaultAsync<User>(updateQuery, result);
        }
    }
}

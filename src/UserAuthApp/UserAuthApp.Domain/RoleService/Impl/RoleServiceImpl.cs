using Mandiri.MiniProject.Utilities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthApp.Data.Dao;
using UserAuthApp.Data.Db;
using UserAuthApp.Domain.RoleService.Models;
using UserAuthApp.Domain.UserService.Models;

namespace UserAuthApp.Domain.RoleService.Impl
{
    public class RoleServiceImpl : IRoleService
    {
        private readonly MiniProjectDbContext _db;

        public RoleServiceImpl(MiniProjectDbContext db)
        {
            _db = db;
        }
        public async Task<Role> Create(string name, CancellationToken c)
        {
            var data = await _db.Roles.FirstOrDefaultAsync(b => b.Name.ToUpper() == name.ToUpper());
            if (data != null)
                throw new DomainLayerException($"There's already role with name '{name}'");
            Role result = new Role { Name = name };
            await _db.Roles.AddAsync(result, c);
            await _db.SaveChangesAsync(c);
            return result;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            var data = await Read(id, c);
            _db.Roles.Remove(data);
            await _db.SaveChangesAsync(c);
        }

        public async Task<List<Role>> List(RoleListParamDomainModel p, CancellationToken c)
        {
            IQueryable<Role> rawQuery = _db.Roles;

            if (p.IsIncludeUser)
                rawQuery = rawQuery.Include(b => b.UserRoles).ThenInclude(b => b.User);

            if (!string.IsNullOrWhiteSpace(p.Name))
                rawQuery = rawQuery.Where(b => b.Name!.ToUpper().Contains(p.Name.ToUpper()));


            return await rawQuery.ToListAsync(c);
        }

        public async Task<Role> Read(int id, CancellationToken c, bool isIncludeUser = false)
        {
            IQueryable<Role> rawQuery = _db.Roles;

            //if (isIncludeUser)
            //    rawQuery = rawQuery.Include(b => b.UserRoles).ThenInclude(b => b.Role);

            return await rawQuery.FirstOrDefaultAsync(b => b.RoleId == id, c);
        }

        public async Task<Role> Update(Role d, CancellationToken c)
        {
            var result = await Read(d.RoleId, c)
                ?? throw new DataNotFoundException($"Role with id '{d.RoleId}' not found!");

            var data = await _db.Roles.FirstOrDefaultAsync(b => b.RoleId != result.RoleId && b.Name.ToUpper() == d.Name.ToUpper());

            if (data != null)
                throw new DomainLayerException($"There's already role with name '{d.Name}'");

            result.Update(d);
            _db.Roles.Update(result);
            await _db.SaveChangesAsync(c);
            return result;
        }
    }
}

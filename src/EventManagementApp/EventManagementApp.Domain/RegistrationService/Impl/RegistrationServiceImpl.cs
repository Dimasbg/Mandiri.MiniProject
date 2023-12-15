using EventManagementApp.Data.Api.UserApiRepo;
using EventManagementApp.Data.Dao;
using EventManagementApp.Data.Db;
using EventManagementApp.Domain.EventService;
using EventManagementApp.Domain.RegistrationService.Models;
using Mandiri.MiniProject.Utilities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.Domain.RegistrationService.Impl
{
    public class RegistrationServiceImpl : IRegistrationService
    {
        private readonly MiniProjectDbContext _db;
        private readonly IUserApiRepo _userApiRepo;

        public RegistrationServiceImpl(MiniProjectDbContext db, IUserApiRepo userApiRepo)
        {
            _db = db;
            _userApiRepo = userApiRepo;
        }

        public async Task<Registration> Create(Registration d, CancellationToken c)
        {
            d.Event = await _db.Events.FirstOrDefaultAsync(b => b.EventId == d.Event.EventId, c)
                ?? throw new DomainLayerException($"Event with id '{d.Event.EventId}' not found!");

            User dataUser = await _userApiRepo.Read(d.UserId, c)
                ?? throw new DomainLayerException($"User with id '{d.UserId}' not found!");

            await _db.Registrations.AddAsync(d, c);
            await _db.SaveChangesAsync(c);
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            var result = await _db.Registrations.FirstOrDefaultAsync(b => b.UserId == id, c);
            if (result == null)
                return;

            _db.Registrations.Remove(result);
            await _db.SaveChangesAsync(c);
        }

        public async Task<List<Registration>> List(RegistrationListParamDomainModel p, CancellationToken c)
        {
            IQueryable<Registration> rawQuery = _db.Registrations;

            if (p.IsIncludeEvent)
                rawQuery = rawQuery.Include(b => b.Event);

            return await rawQuery.ToListAsync(c);
        }

        public async Task<Registration> Read(int id, CancellationToken c, bool isIncludeEvent = false)
        {
            IQueryable<Registration> rawQuery = _db.Registrations;

            if (isIncludeEvent)
                rawQuery = rawQuery.Include(b => b.Event);

            return await rawQuery.FirstOrDefaultAsync(b => b.UserId == id, c);
        }

        public async Task<Registration> Update(Registration d, CancellationToken c)
        {
            var result = await _db.Registrations
                .Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.RegistrationId == d.RegistrationId)
                ?? throw new DomainLayerException($"Registration with id '{d.RegistrationId}' not found!");

            d.Event = await _db.Events.FirstOrDefaultAsync(b => b.EventId == d.Event.EventId, c)
                ?? throw new DomainLayerException($"Event with id '{d.Event.EventId}' not found!");

            User dataUser = await _userApiRepo.Read(d.UserId, c)
                ?? throw new DomainLayerException($"User with id '{d.UserId}' not found!");

            result.Update(d);

            _db.Registrations.Update(result);
            await _db.SaveChangesAsync(c);
            return result;

        }
    }
}

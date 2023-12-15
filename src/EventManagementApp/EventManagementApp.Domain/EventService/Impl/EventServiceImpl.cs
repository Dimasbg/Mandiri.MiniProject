using EventManagementApp.Data.Dao;
using EventManagementApp.Data.Db;
using EventManagementApp.Domain.EventService.Models;
using Mandiri.MiniProject.Utilities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.Domain.EventService.Impl
{
    public class EventServiceImpl : IEventService
    {
        private readonly MiniProjectDbContext _db;

        public EventServiceImpl(MiniProjectDbContext db)
        {
            _db = db;
        }

        public async Task<Event> Create(Event d, CancellationToken c)
        {
            var exist = await _db.Events.FirstOrDefaultAsync(b => b.Name.ToUpper() == d.Name.ToUpper(), c);
            if (exist != null)
                throw new DomainLayerException($"'{d.Name}' already used!");

            await _db.Events.AddAsync(d, c);
            await _db.SaveChangesAsync(c);
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            var result = await Read(id, c);
            if (result == null)
                return;

            _db.Events.Remove(result);
            await _db.SaveChangesAsync(c);
        }

        public async Task<List<Event>> List(EventListParamDomainModel p, CancellationToken c)
        {
            IQueryable<Event> rawQuery = _db.Events;

            if (!string.IsNullOrWhiteSpace(p.Name))
                rawQuery = rawQuery.Where(b => b.Name.ToUpper().Contains(p.Name.ToUpper()));

            if (!string.IsNullOrWhiteSpace(p.Description))
                rawQuery = rawQuery.Where(b => b.Description.ToUpper().Contains(p.Description.ToUpper()));

            if (!string.IsNullOrWhiteSpace(p.Location))
                rawQuery = rawQuery.Where(b => b.Location.ToUpper().Contains(p.Location.ToUpper()));

            return await rawQuery.ToListAsync(c);
        }

        public async Task<Event> Read(int id, CancellationToken c)
        {
            var result = await _db.Events.FirstOrDefaultAsync(b => b.EventId == id, c);
            return result;
        }

        public async Task<Event> Update(Event d, CancellationToken c)
        {
            var result = await Read(d.EventId, c)
                ?? throw new DomainLayerException($"Event with id '{d.EventId}' not found!");

            var exist = await _db.Events.FirstOrDefaultAsync(b => b.EventId != d.EventId && b.Name.ToUpper() == d.Name.ToUpper(), c);
            if (exist != null)
                throw new DomainLayerException($"'{d.Name}' already used!");

            result.Update(d);
            _db.Events.Update(result);
            await _db.SaveChangesAsync(c);
            return result;
        }
    }
}

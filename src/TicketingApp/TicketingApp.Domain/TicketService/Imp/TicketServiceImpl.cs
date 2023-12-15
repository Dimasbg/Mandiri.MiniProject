using Mandiri.MiniProject.Utilities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingApp.Data.Api.EventApiRepo;
using TicketingApp.Data.Dao;
using TicketingApp.Data.Db;
using TicketingApp.Domain.TicketService.Models;

namespace TicketingApp.Domain.TicketService.Imp
{
    public class TicketServiceImpl : ITicketService
    {
        private readonly MiniProjectDbContext _db;
        private readonly IEventApiRepo _eventApiRepo;
        public TicketServiceImpl(MiniProjectDbContext db, IEventApiRepo eventApiRepo)
        {
            _db = db;
            _eventApiRepo = eventApiRepo;
        }

        public async Task<Ticket> Create(Ticket d, CancellationToken c)
        {
            Event dataEvent = await _eventApiRepo.Read(d.EventId, c)
                ?? throw new DomainLayerException($"Event with id '{d.EventId}' not found!");

            Ticket exist = await _db.Tickets.FirstOrDefaultAsync(b => b.EventId == d.EventId && b.TicketType == d.TicketType, c);

            if (exist != null)
                throw new DomainLayerException($"Ticket with type '{d.TicketType}' for event '{dataEvent.Name}' already exist!");

            await _db.Tickets.AddAsync(d, c);
            await _db.SaveChangesAsync(c);
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            Ticket result = await _db.Tickets.FirstOrDefaultAsync(b => b.EventId == id, c);
            if (result != null)
                return;

            _db.Tickets.Remove(result);
            await _db.SaveChangesAsync(c);
        }

        public async Task<List<Ticket>> List(TicketListParamDomainModel p, CancellationToken c)
        {
            IQueryable<Ticket> rawQuery = _db.Tickets;

            if (p.Price.HasValue)
                rawQuery = rawQuery.Where(b => b.Price == p.Price);

            if (!string.IsNullOrWhiteSpace(p.TicketType))
                rawQuery = rawQuery.Where(b => b.TicketType.ToUpper().Contains(p.TicketType.ToUpper()));

            return await rawQuery.ToListAsync(c);
        }

        public async Task<Ticket> Read(int id, CancellationToken c)
        {
            IQueryable<Ticket> rawQuery = _db.Tickets;

            return await rawQuery.FirstOrDefaultAsync(b => b.TicketId == id, c);
        }

        public async Task<Ticket> Update(Ticket d, CancellationToken c)
        {
            var result = await _db.Tickets
                .FirstOrDefaultAsync(b => b.TicketId == d.TicketId)
                ?? throw new DomainLayerException($"Ticket with id '{d.TicketId}' not found!");

            Event dataEvent = await _eventApiRepo.Read(d.EventId, c)
                ?? throw new DomainLayerException($"Event with id '{d.EventId}' not found!");

            var exist = await _db.Tickets.FirstOrDefaultAsync(b => b.TicketId != d.TicketId && b.EventId == d.EventId && b.TicketType == d.TicketType, c);

            if (exist != null)
                throw new DomainLayerException($"Ticket with type '{d.TicketType}' for event '{dataEvent.Name}' already exist!");

            result.Update(d);

            _db.Tickets.Update(result);
            await _db.SaveChangesAsync(c);
            return result;
        }
    }
}

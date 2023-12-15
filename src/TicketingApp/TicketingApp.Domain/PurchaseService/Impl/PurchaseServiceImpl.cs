using Mandiri.MiniProject.Utilities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingApp.Data.Api.UserApiRepo;
using TicketingApp.Data.Dao;
using TicketingApp.Data.Db;
using TicketingApp.Domain.PurchaseService.Models;

namespace TicketingApp.Domain.PurchaseService.Impl
{
    public class PurchaseServiceImpl : IPurchaseService
    {
        private readonly MiniProjectDbContext _db;
        private readonly IUserApiRepo _userApiRepo;

        public PurchaseServiceImpl(MiniProjectDbContext db, IUserApiRepo userApiRepo)
        {
            _db = db;
            _userApiRepo = userApiRepo;
        }

        public async Task<Purchase> Create(Purchase d, CancellationToken c)
        {
            if (d.Quantity == 0)
                throw new DomainLayerException("At Least 1 ticket must be bought!");

            if (!d.PurchaseDateTime.HasValue)
                throw new DomainLayerException("Purchase date cannot be empty!");

            d.Ticket = await _db.Tickets.FirstOrDefaultAsync(b => b.TicketId == d.Ticket.TicketId, c)
                ?? throw new DomainLayerException($"Ticket with id '{d.Ticket.TicketId}' not found!");

            User dataUser = await _userApiRepo.Read(d.UserId, c)
                ?? throw new DomainLayerException($"User with id '{d.UserId}' not found!");

            await _db.Purchases.AddAsync(d, c);
            await _db.SaveChangesAsync(c);
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            Purchase result = await _db.Purchases.FirstOrDefaultAsync(b => b.PurchaseId == id, c);
            if (result == null)
                return;

            _db.Purchases.Remove(result);
            await _db.SaveChangesAsync(c);
        }

        public async Task<List<Purchase>> List(PurchaseListParamDomainModel p, CancellationToken c)
        {
            IQueryable<Purchase> rawQuery = _db.Purchases;

            if (p.PurchaseDateTime.HasValue)
                rawQuery = rawQuery.Where(b => b.PurchaseDateTime == p.PurchaseDateTime);

            if (p.Quantity.HasValue)
                rawQuery = rawQuery.Where(b => b.Quantity == p.Quantity);

            if (p.UserId.HasValue)
                rawQuery = rawQuery.Where(b => b.UserId == p.UserId);

            if (p.EventId.HasValue)
                rawQuery = rawQuery.Where(b => b.Ticket.EventId == p.EventId);

            return await rawQuery.ToListAsync(c);
        }

        public async Task<Purchase> Read(int id, CancellationToken c, bool isIncludeTicket = false)
        {
            IQueryable<Purchase> rawQuery = _db.Purchases;
            if (isIncludeTicket)
                rawQuery = rawQuery.Include(b => b.Ticket);
            return await rawQuery.FirstOrDefaultAsync(b => b.PurchaseId == id, c);
        }

        public async Task<Purchase> Update(Purchase d, CancellationToken c)
        {
            if (d.Quantity == 0)
                throw new DomainLayerException("At Least 1 ticket must be bought!");

            d.Ticket = await _db.Tickets.FirstOrDefaultAsync(b => b.TicketId == d.Ticket.TicketId, c)
                ?? throw new DomainLayerException($"Ticket with id '{d.Ticket.TicketId}' not found!");

            User dataUser = await _userApiRepo.Read(d.UserId, c)
                ?? throw new DomainLayerException($"User with id '{d.UserId}' not found!");

            Purchase result = await _db.Purchases
                .Include(b => b.Ticket)
                .FirstOrDefaultAsync(b => b.PurchaseId == d.PurchaseId, c)
                ?? throw new DomainLayerException($"Purchase with id '{d.PurchaseId}' not found!");

            result.Update(d);
            _db.Purchases.Update(result);
            await _db.SaveChangesAsync(c);
            return result;
        }
    }
}

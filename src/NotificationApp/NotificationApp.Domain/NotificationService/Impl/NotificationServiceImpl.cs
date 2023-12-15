using Mandiri.MiniProject.Utilities.Base;
using Microsoft.EntityFrameworkCore;
using NotificationApp.Data.Api.EventApiRepo;
using NotificationApp.Data.Api.UserApiRepo;
using NotificationApp.Data.Dao;
using NotificationApp.Data.Db;
using NotificationApp.Domain.NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationApp.Domain.NotificationService.Impl
{
    public class NotificationServiceImpl : INotificationService
    {
        private readonly MiniProjectDbContext _db;
        private readonly IUserApiRepo _userApiRepo;
        private readonly IEventApiRepo _eventApiRepo;

        public NotificationServiceImpl(MiniProjectDbContext db, IUserApiRepo userApiRepo, IEventApiRepo eventApiRepo)
        {
            _db = db;
            _userApiRepo = userApiRepo;
            _eventApiRepo = eventApiRepo;
        }

        public async Task<Notification> Create(Notification d, CancellationToken c)
        {
            User dataUser = await _userApiRepo.Read(d.UserId, c)
                ?? throw new DomainLayerException($"User with id '{d.UserId}' not found!");

            Event dataEvent = await _eventApiRepo.Read(d.EventId, c)
                ?? throw new DomainLayerException($"Event with id '{d.EventId}' not found!");

            await _db.Notifications.AddAsync(d, c);
            await _db.SaveChangesAsync(c);
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            var result = await _db.Notifications.FirstOrDefaultAsync(b => b.NotificationId == id, c);
            if (result == null)
                return;
            _db.Notifications.Remove(result);
            await _db.SaveChangesAsync(c);
        }

        public async Task<List<Notification>> List(NotificationListParamDomainModel p, CancellationToken c)
        {
            IQueryable<Notification> rawQuery = _db.Notifications;

            if (p.UserId.HasValue)
                rawQuery = rawQuery.Where(b => b.UserId == p.UserId);

            if (p.EventId.HasValue)
                rawQuery = rawQuery.Where(b => b.EventId == p.EventId);

            if (!string.IsNullOrWhiteSpace(p.Content))
                rawQuery = rawQuery.Where(b => b.Content.ToUpper().Contains(p.Content.ToUpper()));

            if (p.TimeStamp.HasValue)
                rawQuery = rawQuery.Where(b => b.TimeStamp == p.TimeStamp);

            return await rawQuery.ToListAsync(c);
        }

        public async Task<Notification> Read(int id, CancellationToken c)
        {
            IQueryable<Notification> rawQuery = _db.Notifications;

            return await rawQuery.FirstOrDefaultAsync(b => b.NotificationId == id, c);
        }

        public async Task<Notification> Update(Notification d, CancellationToken c)
        {
            var result = await _db.Notifications.FirstOrDefaultAsync(b => b.NotificationId == d.NotificationId, c)
                ?? throw new DomainLayerException($"Notification with id '{d.NotificationId}' not found!");

            User dataUser = await _userApiRepo.Read(d.UserId, c)
                ?? throw new DomainLayerException($"User with id '{d.UserId}' not found!");

            Event dataEvent = await _eventApiRepo.Read(d.EventId, c)
                ?? throw new DomainLayerException($"Event with id '{d.EventId}' not found!");

            result.Udate(d);
            _db.Notifications.Update(result);
            await _db.SaveChangesAsync(c);
            return result;
        }
    }
}

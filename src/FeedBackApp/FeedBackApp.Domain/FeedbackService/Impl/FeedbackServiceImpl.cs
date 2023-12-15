using FeedBackApp.Data.Api.EventApiRepo;
using FeedBackApp.Data.Api.PurchaseApiRepo;
using FeedBackApp.Data.Api.PurchaseApiRepo.Models;
using FeedBackApp.Data.Api.UserApiRepo;
using FeedBackApp.Data.Dao;
using FeedBackApp.Data.Db;
using FeedBackApp.Domain.FeedbackService.Models;
using Mandiri.MiniProject.Utilities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBackApp.Domain.FeedbackService.Impl
{
    public class FeedbackServiceImpl : IFeedbackService
    {
        private readonly MiniProjectDbContext _db;
        private readonly IUserApiRepo _userApiRepo;
        private readonly IPurchaseApiRepo _purchaseApiRepo;
        private readonly IEventApiRepo _eventApiRepo;

        public FeedbackServiceImpl(MiniProjectDbContext db, IUserApiRepo userApiRepo, IPurchaseApiRepo purchaseApiRepo, IEventApiRepo eventApiRepo)
        {
            _db = db;
            _userApiRepo = userApiRepo;
            _purchaseApiRepo = purchaseApiRepo;
            _eventApiRepo = eventApiRepo;
        }

        public async Task<Feedback> Create(Feedback feedback, CancellationToken cancellationToken)
        {
            User dataUser = await _userApiRepo.Read(feedback.UserId, cancellationToken)
                ?? throw new DomainLayerException($"User with id '{feedback.UserId}' not found!");

            Event dataEvent = await _eventApiRepo.Read(feedback.EventId, cancellationToken)
                ?? throw new DomainLayerException($"Event with id '{feedback.EventId}' not found!");

            List<Purchase> dataPurchase = await _purchaseApiRepo.List(new PurchaseListParamApiDataModel
            {
                EventId = feedback.EventId,
                UserId = feedback.UserId
            }, cancellationToken);

            if (!dataPurchase.Any())
                throw new DomainLayerException($"User with id '{feedback.UserId}' dosen't bought ticket for event with id '{feedback.EventId}'!");

            Feedback exist = await _db.Feedbacks.FirstOrDefaultAsync(b => b.UserId == feedback.UserId && b.EventId == feedback.EventId);
            if (exist != null)
                throw new DomainLayerException($"User with id '{feedback.UserId}' already give feedback for event with id '{feedback.EventId}'!");

            await _db.Feedbacks.AddAsync(feedback, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return feedback;
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var feedbackToDelete = await _db.Feedbacks.FirstOrDefaultAsync(b => b.FeedbackId == id, cancellationToken);
            if (feedbackToDelete == null)
                return;
            _db.Feedbacks.Remove(feedbackToDelete);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Feedback>> List(FeedbackListParamDomainModel parameters, CancellationToken cancellationToken)
        {
            IQueryable<Feedback> rawQuery = _db.Feedbacks;

            if (parameters.Rating.HasValue)
                rawQuery = rawQuery.Where(b => b.Rating == parameters.Rating);

            if (parameters.UserId.HasValue)
                rawQuery = rawQuery.Where(b => b.UserId == parameters.UserId);

            if (parameters.EventId.HasValue)
                rawQuery = rawQuery.Where(b => b.EventId == parameters.EventId);

            return await rawQuery.ToListAsync(cancellationToken);
        }

        public async Task<Feedback> Read(int id, CancellationToken cancellationToken)
        {
            return await _db.Feedbacks.FirstOrDefaultAsync(b => b.FeedbackId == id, cancellationToken);
        }

        public async Task<Feedback> Update(Feedback feedback, CancellationToken cancellationToken)
        {
            User dataUser = await _userApiRepo.Read(feedback.UserId, cancellationToken)
                ?? throw new DomainLayerException($"User with id '{feedback.UserId}' not found!");

            Event dataEvent = await _eventApiRepo.Read(feedback.EventId, cancellationToken)
                ?? throw new DomainLayerException($"Event with id '{feedback.EventId}' not found!");

            List<Purchase> dataPurchase = await _purchaseApiRepo.List(new PurchaseListParamApiDataModel
            {
                EventId = feedback.EventId,
                UserId = feedback.UserId
            }, cancellationToken);

            if (!dataPurchase.Any())
                throw new DomainLayerException($"User with id '{feedback.UserId}' dosen't bought ticket for event with id '{feedback.EventId}'!");

            var result = await Read(feedback.FeedbackId, cancellationToken)
                ?? throw new DataNotFoundException($"{nameof(Feedback)} not found with id '{feedback.FeedbackId}'");


            Feedback exist = await _db.Feedbacks.FirstOrDefaultAsync(b => b.FeedbackId != feedback.FeedbackId && b.UserId == feedback.UserId && b.EventId == feedback.EventId);
            if (exist != null)
                throw new DomainLayerException($"User with id '{feedback.UserId}' already give feedback for event with id '{feedback.EventId}'!");


            result.Update(feedback);
            _db.Feedbacks.Update(result);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

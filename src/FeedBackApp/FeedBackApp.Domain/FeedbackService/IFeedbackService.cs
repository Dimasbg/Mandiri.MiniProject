using FeedBackApp.Data.Dao;
using FeedBackApp.Domain.FeedbackService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBackApp.Domain.FeedbackService
{
    public interface IFeedbackService
    {
        Task<Feedback> Create(Feedback feedback, CancellationToken cancellationToken);
        Task<Feedback> Read(int id, CancellationToken cancellationToken);
        Task<Feedback> Update(Feedback feedback, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<List<Feedback>> List(FeedbackListParamDomainModel parameters, CancellationToken cancellationToken);
    }
}

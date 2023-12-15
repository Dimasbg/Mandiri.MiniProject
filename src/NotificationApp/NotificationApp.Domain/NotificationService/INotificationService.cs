using NotificationApp.Data.Dao;
using NotificationApp.Domain.NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationApp.Domain.NotificationService
{
    public interface INotificationService
    {
        Task<Notification> Create(Notification d, CancellationToken c);
        Task<Notification> Read(int id,  CancellationToken c);
        Task<Notification> Update(Notification d, CancellationToken c);
        Task Delete(int id, CancellationToken c);
        Task<List<Notification>> List(NotificationListParamDomainModel p, CancellationToken c);
    }
}

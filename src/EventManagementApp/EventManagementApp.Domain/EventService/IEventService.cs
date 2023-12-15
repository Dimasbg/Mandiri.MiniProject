using EventManagementApp.Data.Dao;
using EventManagementApp.Domain.EventService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.Domain.EventService
{
    public interface IEventService
    {
        public Task<Event> Create(Event d, CancellationToken c);
        public Task<Event> Read(int id, CancellationToken c);
        public Task<Event> Update(Event d, CancellationToken c);
        public Task Delete(int id, CancellationToken c);
        public Task<List<Event>> List(EventListParamDomainModel p, CancellationToken c);
    }
}

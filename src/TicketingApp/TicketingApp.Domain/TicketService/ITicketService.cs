using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingApp.Data.Dao;
using TicketingApp.Domain.TicketService.Models;

namespace TicketingApp.Domain.TicketService
{
    public interface ITicketService
    {
        public Task<Ticket> Create(Ticket d, CancellationToken c);
        public Task<Ticket> Read(int id, CancellationToken c);
        public Task<Ticket> Update(Ticket d, CancellationToken c);
        public Task Delete(int id, CancellationToken c);
        public Task<List<Ticket>> List(TicketListParamDomainModel p, CancellationToken c);
    }
}

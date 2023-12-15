using EventManagementApp.Data.Dao;
using EventManagementApp.Domain.EventService.Models;
using EventManagementApp.Domain.RegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.Domain.RegistrationService
{
    public interface IRegistrationService
    {
        public Task<Registration> Create(Registration d, CancellationToken c);
        public Task<Registration> Read(int id, CancellationToken c, bool isIncludeEvent = false);
        public Task<Registration> Update(Registration d, CancellationToken c);
        public Task Delete(int id, CancellationToken c);
        public Task<List<Registration>> List(RegistrationListParamDomainModel p, CancellationToken c);
    }
}

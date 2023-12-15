using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingApp.Domain.TicketService.Models
{
    public class TicketListParamDomainModel
    {
        public string? TicketType { get; set; }
        public decimal? Price { get; set; }
    }
}

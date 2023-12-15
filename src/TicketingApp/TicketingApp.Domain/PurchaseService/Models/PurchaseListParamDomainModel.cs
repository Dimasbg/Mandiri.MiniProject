using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingApp.Data.Dao;

namespace TicketingApp.Domain.PurchaseService.Models
{
    public class PurchaseListParamDomainModel
    {
        public DateTime? PurchaseDateTime { get; set; }
        public int? Quantity { get; set; }
        public int? UserId { get; set; }
        public int? EventId { get; set; }
    }
}

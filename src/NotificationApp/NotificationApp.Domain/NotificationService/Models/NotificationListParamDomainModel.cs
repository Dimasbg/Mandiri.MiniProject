using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationApp.Domain.NotificationService.Models
{
    public class NotificationListParamDomainModel
    {
        public int? UserId { get; set; }
        public int? EventId { get; set; }
        public string? Content { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}

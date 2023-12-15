using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBackApp.Domain.FeedbackService.Models
{
    public class FeedbackListParamDomainModel
    {
        public int? UserId { get; set; }
        public int? EventId { get; set; }
        public int? Rating { get; set; }
    }
}

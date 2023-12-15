using System.ComponentModel.DataAnnotations;

namespace FeedBackApp.Data.Dao
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public void Update(Feedback d)
        {
            UserId = d.UserId;
            EventId = d.EventId;
            Rating = d.Rating;
            Comment = d.Comment;
        }
    }
}

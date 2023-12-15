using FeedBackApp.Data.Dao;
using System.ComponentModel.DataAnnotations;

namespace FeedBackApp.Api.Models.FeedBackModels
{
    public class FeedBackApiModel
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public Feedback ToDao(int id = 0)
            => new Feedback
            {
                Comment = Comment,
                EventId = EventId,
                Rating = Rating,
                UserId = UserId,
                FeedbackId = id
            };
    }
}

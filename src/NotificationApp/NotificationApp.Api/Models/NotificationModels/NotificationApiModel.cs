using NotificationApp.Data.Dao;

namespace NotificationApp.Api.Models.NotificationModels
{
    public class NotificationApiModel
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string? Content { get; set; }
        public DateTime? TimeStamp { get; set; }
        public Notification ToDao(int id = 0)
            => new Notification
            {
                NotificationId = id,
                UserId = UserId,
                Content = Content,
                EventId = EventId,
                TimeStamp = TimeStamp
            };
    }
}

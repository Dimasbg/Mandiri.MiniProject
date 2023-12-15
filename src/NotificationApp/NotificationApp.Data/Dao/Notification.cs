namespace NotificationApp.Data.Dao
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string? Content { get; set; }
        public DateTime? TimeStamp { get; set; }
        public void Udate(Notification d)
        {
            UserId = d.UserId;
            EventId = d.EventId;
            Content = d.Content;
            TimeStamp = d.TimeStamp;
        }
    }
}

namespace Order.API.Message
{
    public class MessageTrackLog
    {
        public MessageTrackLog(string messageId)
        {
            MessageId = messageId;
            CreatedTime = DateTime.Now;
        }

        public string MessageId { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}

using Message.API.Utils;

namespace Message.API.Message
{
    public class MessageData<T>
    {
        public string Id { get; set; }

        public T MessageBody { get; set; }

        public DateTime CreatedTime { get; set; }

        public MessageData(T messageBody)
        {
            MessageBody = messageBody;
            CreatedTime = DateTime.Now;
            Id = SnowflakeGenerator.Instance().GetId().ToString();
        }
    }
}

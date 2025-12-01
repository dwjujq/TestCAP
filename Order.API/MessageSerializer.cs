using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Order.API
{
    public class MessageSerializer : ISerializer
    {
        public DotNetCore.CAP.Messages.Message Deserialize(string json)
        {
            return JsonExtension.Deserialize<DotNetCore.CAP.Messages.Message>(json);
        }

        public object Deserialize(object value, Type valueType)
        {
            if (value is JToken jToken)
            {
                return jToken.ToObject(valueType);
            }
            throw new NotSupportedException("Type is not of type JToken");
        }

        public ValueTask<DotNetCore.CAP.Messages.Message> DeserializeAsync(TransportMessage transportMessage, Type valueType)
        {
            if (valueType == null || transportMessage.Body.IsEmpty)
            {
                return ValueTask.FromResult(new DotNetCore.CAP.Messages.Message(transportMessage.Headers, null));
            }
            var json = Encoding.UTF8.GetString(transportMessage.Body.ToArray());
            return ValueTask.FromResult(new DotNetCore.CAP.Messages.Message(transportMessage.Headers, JsonExtension.Deserialize(json, valueType)));
        }

        public bool IsJsonType(object jsonObject)
        {
            return jsonObject is JsonToken || jsonObject is JToken;
        }

        public string Serialize(DotNetCore.CAP.Messages.Message message)
        {
            return JsonExtension.Serialize(message);
        }

        public ValueTask<TransportMessage> SerializeAsync(DotNetCore.CAP.Messages.Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (message.Value == null)
            {
                return ValueTask.FromResult(new TransportMessage(message.Headers, null));
            }
            var json = JsonExtension.Serialize(message.Value);
            return ValueTask.FromResult(new TransportMessage(message.Headers, Encoding.UTF8.GetBytes(json)));
        }
    }

}

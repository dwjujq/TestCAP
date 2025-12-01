namespace Order.API.Message
{
    public interface IMessageTracker
    {
        Task<bool> HasProcessedAsync(string msgId);

        bool HasProcessed(string msgId);

        Task MarkAsProcessedAsync(string msgId);

        void MarkAsProcessed(string msgId);
    }
}

using Microsoft.EntityFrameworkCore;

namespace Message.API.Message
{
    public class MySqlMessageTracker : IMessageTracker
    {
        private readonly AppDbContext _capContext;

        public MySqlMessageTracker(AppDbContext capContext)
        {
            _capContext = capContext;
        }

        public bool HasProcessed(string msgId)
        {
            return _capContext.MessageTrackLogs.Any(x => x.MessageId == msgId);
        }

        public Task<bool> HasProcessedAsync(string msgId)
        {
            return _capContext.MessageTrackLogs.AnyAsync(x => x.MessageId == msgId);
        }

        public void MarkAsProcessed(string msgId)
        {
            MessageTrackLog messageTrackLog = new MessageTrackLog(msgId);
            _capContext.MessageTrackLogs.Add(messageTrackLog);
            _capContext.SaveChanges();
        }

        public async Task MarkAsProcessedAsync(string msgId)
        {
            MessageTrackLog messageTrackLog = new MessageTrackLog(msgId);
            await _capContext.MessageTrackLogs.AddAsync(messageTrackLog);
            await _capContext.SaveChangesAsync();
        }
    }
}

using Message.API.Message;
using Microsoft.EntityFrameworkCore;

namespace Message.API
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Inventory> Inventory { get; set; }

        public DbSet<MessageTrackLog> MessageTrackLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 可以在这里进行模型配置
            modelBuilder.Entity<MessageTrackLog>(entity =>
            {
                entity.ToTable("message_track_log");
                entity.ToTable(t => t.HasComment("消息踪迹表"));

                entity.HasKey(e => e.MessageId);
            });
        }
    }
}

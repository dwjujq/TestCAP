using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using Message.API.Message;
using Message.API.Utils;
using Microsoft.EntityFrameworkCore;
using System;

namespace Message.API.Extensions
{
    public static class CapExtensions
    {
        public static IServiceCollection AddMCodeCap(this IServiceCollection services, Action<CapOptions> configure = null)
        {
            var rabbitMQOptions = ServiceProviderServiceExtensions.GetRequiredService<IConfiguration>(services.BuildServiceProvider()).GetSection("rabbitmq").Get<RabbitMQOptions>();

            var logger = ServiceProviderServiceExtensions.GetRequiredService<ILogger<AppDbContext>>(services.BuildServiceProvider());

            if (rabbitMQOptions == null)
            {
                throw new ArgumentNullException("rabbitmq not config.");
            }

            var capConnection = ServiceProviderServiceExtensions.GetRequiredService<IConfiguration>(services.BuildServiceProvider()).GetConnectionString("InventoryContext");

            if (string.IsNullOrWhiteSpace(capConnection))
            {
                throw new ArgumentException("cap未设置");
            }

            services.AddCap(x =>
            {
                x.UseEntityFramework<AppDbContext>();

                //使用RabbitMQ传输
                x.UseRabbitMQ(opt => { opt = rabbitMQOptions; });

                ////使用MySQL持久化
                x.UseMySql(capConnection);

                //x.UseEntityFramework<CapContext>();

                x.UseDashboard();

                //成功消息的过期时间（秒）
                x.SucceedMessageExpiredAfter = 10 * 24 * 3600;

                x.FailedRetryCount = 3;

                x.FailedRetryInterval = 10;

                //失败回调，通过企业微信，短信通知人工干预
                x.FailedThresholdCallback = (e) =>
                {
                    if (e.MessageType == MessageType.Publish)
                    {
                        logger.LogError("Cap发送消息失败;" + JsonExtension.Serialize(e.Message));
                    }
                    else if (e.MessageType == MessageType.Subscribe)
                    {
                        logger.LogError("Cap接收消息失败;" + JsonExtension.Serialize(e.Message));
                    }
                };

                configure?.Invoke(x);
            });

            return services;
        }
    }
}

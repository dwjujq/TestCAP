using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using System;

namespace Order.API.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// 添加分布式事务服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="capSection">cap链接项</param>
        /// <param name="rabbitMQSection">rabbitmq配置项</param>
        /// <param name="expiredTime">成功消息过期时间</param>
        /// <returns></returns>
        public static IServiceCollection AddMCodeCap(this IServiceCollection services, Action<CapOptions> configure = null, string capSection = "cap", string rabbitMQSection = "rabbitmq")
        {
            var rabbitMQOptions = ServiceProviderServiceExtensions.GetRequiredService<IConfiguration>(services.BuildServiceProvider()).GetSection(rabbitMQSection).Get<RabbitMQOptions>();

            var logger = ServiceProviderServiceExtensions.GetRequiredService<ILogger<Order.API.AppContext>>(services.BuildServiceProvider());

            if (rabbitMQOptions == null)
            {
                throw new ArgumentNullException("rabbitmq not config.");
            }

            var capJson = ServiceProviderServiceExtensions.GetRequiredService<IConfiguration>(services.BuildServiceProvider()).GetValue<string>(capSection);


            if (string.IsNullOrEmpty(capJson))
            {
                throw new ArgumentException("cap未设置");
            }

            //services.AddDbContext<CapContext>(options => options.UseMySql(capJson, ServerVersion.AutoDetect(capJson)));

            services.AddCap(x =>
            {
                //使用RabbitMQ传输
                x.UseRabbitMQ(opt => { opt = rabbitMQOptions; });

                ////使用MySQL持久化
                x.UseMySql(capJson);

                //x.UseEntityFramework<CapContext>();

                x.UseDashboard();

                //成功消息的过期时间（秒）
                x.SucceedMessageExpiredAfter = 10 * 24 * 3600;

                x.FailedRetryCount = 5;

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

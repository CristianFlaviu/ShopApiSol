using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopApi.Core.Email;
using ShopApi.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopApi.Notification
{
    public class NotificationService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<NotificationService> _logger;
        private readonly IServiceProvider _provider;
        private Timer _timer;

        public NotificationService(ILogger<NotificationService> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(30));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            var scope = _provider.CreateScope();
            var orderRepo = scope.ServiceProvider.GetRequiredService<OrderRepo>();

            var emailService = scope.ServiceProvider.GetRequiredService<EmailSender>();

            var orders = await orderRepo.GetAllOrders();



            foreach (var order in orders)
            {
                if (order.Payment == null && order.DueDate < DateTime.Now)
                {
                    _logger.LogInformation($"user {order.User.FirstName} Order {order.Id} \n");

                    await emailService.SendOrderNotPaidMailAsync(order.User.FirstName, order.User.LastName, order.User.Email, order.OrderDate.ToLocalTime().ToShortDateString());
                }

            }

            _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
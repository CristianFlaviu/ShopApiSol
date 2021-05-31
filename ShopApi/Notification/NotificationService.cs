using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ShopApi.Repository;

namespace ShopApi.Core.Notification
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
            //_logger.LogInformation("Timed Hosted Service running.");

            //_timer = new Timer(DoWork, null, TimeSpan.Zero,
            //    TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            var scope = _provider.CreateScope();
            var productUserShoppingCartRepo = scope.ServiceProvider.GetRequiredService<ShoppingCartRepo>();

            var productUserShoppingCartList = await productUserShoppingCartRepo.GetProductsAboutToExpire();

            foreach (var productsUsersShoppingCart in productUserShoppingCartList)
            {
                _logger.LogInformation($"user {productsUsersShoppingCart.User.FirstName} Product {productsUsersShoppingCart.Product.Title} \n");
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
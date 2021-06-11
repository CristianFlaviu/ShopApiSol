using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using ShopApi.Core.SignalR;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShopApi.Core.RabbitMQ
{
    public class ConsumerService : BackgroundService
    {
        private readonly IServiceProvider _provider;

        private readonly ILogger<ConsumerService> _logger;
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private const string QueueName = "Barcode_Second_queue";
        private readonly IHubContext<MessageHub> _messageHub;

        public ConsumerService(IServiceProvider provider,
            ILogger<ConsumerService> logger,
            IHubContext<MessageHub> messageHub)
        {
            _provider = provider;
            _logger = logger;
            _messageHub = messageHub;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = "roedeer-01.rmq.cloudamqp.com",
                UserName = "tkfdwjro",
                Password = "calbiuNFyl_9kOwHQH6eBS5omW5Wb_zV",
                VirtualHost = "tkfdwjro",
                DispatchConsumersAsync = true
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _logger.LogInformation($"Queue [{QueueName}] is waiting for messages.");


            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _connection.Close();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (bc, ea) =>
           {

               var message = Encoding.UTF8.GetString(ea.Body.ToArray());

               var decodedMessage = JsonConvert.DeserializeObject<RaspberryInfo>(message);


               _logger.LogInformation($"{DateTime.Now}  -  socket = {decodedMessage.Socket}   message= {decodedMessage.Barcode}");

               await _messageHub.Clients.All.SendAsync("transferData/" + decodedMessage.Socket, decodedMessage.Barcode, stoppingToken);
               try
               {
                   _channel.BasicAck(ea.DeliveryTag, false);
               }
               catch (JsonException)
               {
                   _logger.LogError($"JSON Parse Error: .");
                   _channel.BasicNack(ea.DeliveryTag, false, false);
               }
               catch (AlreadyClosedException)
               {
                   _logger.LogInformation("RabbitMQ is closed!");
               }
               catch (Exception e)
               {
                   _logger.LogError(default, e, e.Message);
               }
           };

            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

            await Task.CompletedTask;
        }
    }

    public class RaspberryInfo
    {
        public string Socket { get; set; }
        public string Barcode { get; set; }
    }

}
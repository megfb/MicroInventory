using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using MicroInventory.Shared.EventBus.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MicroInventory.Shared.EventBus
{
    public class AzureServiceBusConsumer : BackgroundService
    {
        private readonly List<ServiceBusProcessor> _processors = [];
        private readonly IEventBusSubscriptionManager _subscriptionManager;
        private readonly IServiceProvider _serviceProvider;

        private readonly List<(string TopicName, string SubscriptionName)> _subscriptions;

        public AzureServiceBusConsumer(
            ServiceBusClient client,
            IEventBusSubscriptionManager subscriptionManager,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _subscriptionManager = subscriptionManager;
            _serviceProvider = serviceProvider;

            // Burada istediğin kadar sub ekleyebilirsin:
            _subscriptions = new()
            {
                ("category-events-topic", "new-category-added-sub"),
                ("category-events-topic", "category-updated-sub"),
                ("category-events-topic", "category-deleted-sub"),
            };

            foreach (var (topic, sub) in _subscriptions)
            {
                var processor = client.CreateProcessor(topic, sub, new ServiceBusProcessorOptions
                {
                    AutoCompleteMessages = false,
                    MaxConcurrentCalls = 1
                });

                processor.ProcessMessageAsync += OnMessageReceived;
                processor.ProcessErrorAsync += OnError;

                _processors.Add(processor);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var processor in _processors)
            {
                Console.WriteLine($"🟢 Başlatılıyor: {processor.EntityPath}");
                await processor.StartProcessingAsync(stoppingToken);
            }
        }

        private async Task OnMessageReceived(ProcessMessageEventArgs args)
        {
            var eventName = args.Message.Subject;

            Console.WriteLine($"📩 Mesaj alındı: {eventName}");

            if (_subscriptionManager.HasSubscriptionForEvent(eventName))
            {
                var handlers = _subscriptionManager.GetHandlersForEvent(eventName);

                foreach (var handlerInfo in handlers)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetService(handlerInfo.HandlerType);
                    if (handler == null) continue;

                    var eventType = _subscriptionManager.GetEventTypeByName(eventName);
                    var body = Encoding.UTF8.GetString(args.Message.Body);
                    var integrationEvent = JsonSerializer.Deserialize(body, eventType);

                    var concreteHandlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    var handleMethod = concreteHandlerType.GetMethod("Handle");
                    await (Task)handleMethod.Invoke(handler, new[] { integrationEvent });
                }
            }
            else
            {
                Console.WriteLine($"⚠️ Handler bulunamadı: {eventName}");
            }

            await args.CompleteMessageAsync(args.Message);
        }

        private Task OnError(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"❌ Hata: {args.Exception.Message}");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var processor in _processors)
            {
                await processor.StopProcessingAsync(cancellationToken);
                await processor.DisposeAsync();
            }

            await base.StopAsync(cancellationToken);
        }
    }
}

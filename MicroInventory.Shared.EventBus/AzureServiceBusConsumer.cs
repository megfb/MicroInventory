using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MicroInventory.Shared.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MicroInventory.Shared.EventBus
{
    public class AzureServiceBusConsumer : BackgroundService
    {
        private readonly ServiceBusProcessor _processor;
        private readonly IEventBusSubscriptionManager _subscriptionManager;
        private readonly IServiceProvider _serviceProvider;

        public AzureServiceBusConsumer(
            ServiceBusClient client,
            string topicName,
            string subscriptionName,
            IEventBusSubscriptionManager subscriptionManager,
            IServiceProvider serviceProvider)
        {
            _subscriptionManager = subscriptionManager;
            _serviceProvider = serviceProvider;

            _processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor.ProcessMessageAsync += OnMessageReceived;
            _processor.ProcessErrorAsync += ErrorHandler;

            await _processor.StartProcessingAsync(stoppingToken);
        }

        private async Task OnMessageReceived(ProcessMessageEventArgs args)
        {
            var eventName = args.Message.Subject;

            if (_subscriptionManager.HasSubscriptionForEvent(eventName))
            {
                var handlers = _subscriptionManager.GetHandlersForEvent(eventName);

                foreach (var handlerInfo in handlers)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetService(handlerInfo.HandlerType);
                    if (handler == null) continue;

                    var eventType = _subscriptionManager.GetEventTypeByName(eventName);
                    var messageBody = Encoding.UTF8.GetString(args.Message.Body);
                    var integrationEvent = JsonSerializer.Deserialize(messageBody, eventType);

                    var concreteHandlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    var handleMethod = concreteHandlerType.GetMethod("Handle");
                    await (Task)handleMethod.Invoke(handler, new[] { integrationEvent });
                }
            }

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"❌ Hata: {args.Exception.Message}");
            return Task.CompletedTask;
        }
    }
}

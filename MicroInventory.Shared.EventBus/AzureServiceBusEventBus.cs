using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MicroInventory.Shared.EventBus.Abstractions;

namespace MicroInventory.Shared.EventBus
{
    public class AzureServiceBusEventBus : IEventBus
    {
        private readonly ServiceBusClient _client;
        private readonly IEventBusSubscriptionManager _subscriptionManager;
        private readonly IServiceProvider _serviceProvider;

        public AzureServiceBusEventBus(
            ServiceBusClient client,
            IEventBusSubscriptionManager subscriptionManager,
            IServiceProvider serviceProvider)
        {
            _client = client;
            _subscriptionManager = subscriptionManager;
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync<T>(T @event, string topicName) where T : IntegrationEvent
        {
            var sender = _client.CreateSender(topicName);

            var jsonMessage = JsonSerializer.Serialize(@event);
            var message = new ServiceBusMessage(jsonMessage)
            {
                Subject = typeof(T).Name,
                MessageId = @event.Id.ToString(),
            };
            message.ApplicationProperties["type"] = @event.GetType().Name;

            await sender.SendMessageAsync(message);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _subscriptionManager.AddSubscription<T, TH>();
        }

        public void Unsubscribe<T, TH>(string topicName, string subscriptionName)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _subscriptionManager.RemoveSubscription<T, TH>();
        }
    }
}

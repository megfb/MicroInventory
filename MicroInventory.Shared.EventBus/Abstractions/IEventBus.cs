using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroInventory.Shared.EventBus.Abstractions
{

    //servislerin eventler için kullanabileceği base class. rabbitmq veya servicebus için kullanılabilir. servislere implemente edilecek ve 
    //servis içerisinde bu üç method kullanılacak. publish: bir eventi yayınlamak için, subscribe-unsubscribe bir topic'e abone olmak veya ayrılmak için varlar.
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event, string topicName) where T : IntegrationEvent;

        void Subscribe<T,TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
        void Unsubscribe<T, TH>(string topicName, string subscriptionName)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}

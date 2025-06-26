using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroInventory.Shared.EventBus.Abstractions
{

    //bir servis içerisine gönderilen integrationevent lerle ilgili işlem yapmak için kullanacağız.
    //ilgili servis tetiklendiğinde Handle methodu devreye girer ve işlem yapar.
    public interface IIntegrationEventHandler<TIngetrationEvent> where TIngetrationEvent:IntegrationEvent
    {
        Task Handle(TIngetrationEvent @event);
    }
}

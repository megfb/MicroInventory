using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroInventory.Shared.EventBus.Models;

namespace MicroInventory.Shared.EventBus.Abstractions
{

    //subscriptionları, integrationeventleri, handler ları bu class sayesinde tutuyoruz.
    //InMemory çalışır fakat memory de çalıştırmaktansa fikrimiz değişirse diye db de veya başka bir yerde tutmak istersek diye base bir class oluşturuyoruz.
    public interface IEventBusSubscriptionManager
    {
        //herhangi bir event ismi ile event subscription ı olup olmadığını kontrol eder.
        bool IsEmpty { get; }
        //event remove edildiği zaman içeride bir event oluşturulur. unsubscribe işlemi yapıldığında bu event tetiklenir.
        event EventHandler<string> OnEventRemoved;
        //event subscription ı ekler.
        void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
        //event subscription ı siler.
        void RemoveSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
        // dışarıdan bir event geldiğinde bizim subscription ımız var mı yok mu kontrol eder. generic olmayan hali.
        bool HasSubscriptionForEvent(string eventName);
        // dışarıdan bir event geldiğinde bizim subscription ımız var mı yok mu kontrol eder. generic olan hali.
        bool HasSubscriptionForEvent<T>() where T : IntegrationEvent;
        //event ismi ile event type ını alır.
        Type GetEventTypeByName(string eventName);
        // listeyi siler (örneğin uygulama kapatıldığında subscriptionları temizlemek için kullanılır).
        void Clear();
        // eventin tüm subslarını tüm handler larını döner. generic hali.
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;
        // eventin tüm subslarını tüm handler larını döner. generic olmayan hali.
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        //anlamadım.
        string GetEventKey<T>();

    }
}

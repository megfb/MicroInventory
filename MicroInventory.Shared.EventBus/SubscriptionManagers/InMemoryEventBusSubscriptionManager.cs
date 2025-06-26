using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Models;

namespace MicroInventory.Shared.EventBus.SubscriptionManagers
{
    public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        //handlerları tutan bir dictionary yapısı.
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;
        //event ismi ile event type ını alabilmek için kullanacağımız bir delegate. kuyruk oluşturduğumuzda IntegrationEvent kısmından kurtulacağız.
        private readonly Func<string, string> eventNameGetter;
        public InMemoryEventBusSubscriptionManager(Func<string, string> eventNameGetter)
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
            this.eventNameGetter = eventNameGetter;
        }
        //key olup olmadığına bakacağız
        public bool IsEmpty => !_handlers.Keys.Any();
        //handler ı clear edeceğiz
        public void Clear() => _handlers.Clear();

        public void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            // event ismini alacağız
            var eventName = GetEventKey<T>();

            AddSubscription(typeof(TH), eventName);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }
        private void AddSubscription(Type handlerType, string eventName)
        {
            //dictionary de bu event için bir key olup olmadığı kontrol edilir.
            if (!HasSubscriptionForEvent(eventName))
            {
                //subscribe edilmediyse listeye ekleniyor
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }
            //bu tip ile takip ettiğimiz bir handler var ise bir hata fırlatıyoruz.
            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException($"handler type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }
            //2. if gerçekleşmediyse yeni bir tip yaratılıyor.
            _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));

            //burda ki işlemler aynı event in birden fazla kez dinlenmesini engelliyor.
        }
        public void RemoveSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var eventName = GetEventKey<T>();
            RemoveHandler(eventName,handlerToRemove);
        }
        private void RemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove != null)
            {
                _handlers[eventName].Remove(subsToRemove);
                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }
                    RaiseOnEventRemoved(eventName);
                }
            }
        }
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }
        private SubscriptionInfo FindSubscriptionToRemove<T,TH>() where T:IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            return FindSubscriptionToRemove(eventName, typeof(TH));
        }
        private SubscriptionInfo FindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionForEvent(eventName))
            {
                return null;
            }
            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];
        public bool HasSubscriptionForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionForEvent(key);
        }
        public bool HasSubscriptionForEvent(string eventName) => _handlers.ContainsKey(eventName);
        public string GetEventKey<T>()
        {
            string eventName = typeof(T).Name;
            return eventNameGetter(eventName);
        }

        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBus
{
    public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        private readonly Dictionary<string, List<Type>> _handlers;

        public InMemoryEventBusSubscriptionManager()
        {
            _handlers = new Dictionary<string, List<Type>>();
        }

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            if (_handlers.TryGetValue(typeof(T).Name, out var handlers))
            {
                handlers.Remove(typeof(TH));
            }
        }

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            if (_handlers.TryGetValue(typeof(T).Name, out var handlers))
            {
                handlers.Add(typeof(TH));
            }
            else
            {
                _handlers[typeof(T).Name] = new List<Type> { typeof(TH) };
            }
        }

        public IEnumerable<Type> GetHandlersOf<T>() where T : IntegrationEvent
        {
            return GetHandlersOf(typeof(T).Name);
        }

        public IEnumerable<Type> GetHandlersOf(string @event)
        {
            if (_handlers.TryGetValue(@event, out var handlers))
            {
                return handlers;
            }

            return Enumerable.Empty<Type>();
        }

        public bool HasAnyHandler(string @event)
        {
            return _handlers.ContainsKey(@event);
        }
    }
}

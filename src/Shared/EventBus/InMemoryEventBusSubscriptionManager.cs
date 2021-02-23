using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBus
{
    public class InMemoryEventBusSubscriptionManager : ISubscriptionManager
    {
        private readonly Dictionary<string, HashSet<Type>> _handlers;
        private readonly HashSet<Type> _eventTypes;

        public InMemoryEventBusSubscriptionManager()
        {
            _handlers = new Dictionary<string, HashSet<Type>>();
            _eventTypes = new HashSet<Type>();
        }

        public IEnumerable<Type> GetHandlersOf(string @event)
        {
            if (_handlers.TryGetValue(@event, out var handlers))
            {
                return handlers;
            }

            return Enumerable.Empty<Type>();
        }

        public IEnumerable<Type> GetHandlersOf<TEvent>() where TEvent : IntegrationEvent
        {
            return GetHandlersOf(GetEventName<TEvent>());
        }

        public bool HasAnyHandler<T>()
        {
            return HasAnyHandler(GetEventName<T>());
        }

        public bool HasAnyHandler(string @event)
        {
            return GetHandlersOf(@event).Any();
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            if (!_handlers.TryGetValue(GetEventName<T>(), out var handlers))
            {
                handlers = new HashSet<Type>();

                _handlers[GetEventName<T>()] = handlers;
            }

            handlers.Add(typeof(TH));

            _eventTypes.Add(typeof(T));
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            if (_handlers.TryGetValue(GetEventName<T>(), out var handlers))
            {
                handlers.Remove(typeof(TH));

                if (!handlers.Any())
                {
                    _eventTypes.Remove(typeof(T));
                }
            }
        }

        public string GetEventName<TEvent>()
        {
            return typeof(TEvent).Name;
        }

        public Type GetEventType(string eventName)
        {
            return _eventTypes.FirstOrDefault(e => e.Name == eventName);
        }

      
    }
}

using System;
using System.Collections.Generic;

namespace EventBus
{
    public interface IEventBusSubscriptionManager
    {
        void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        bool HasAnyHandler(string @event);
        IEnumerable<Type> GetHandlersOf(string @event);
        IEnumerable<Type> GetHandlersOf<T>() where T : IntegrationEvent;
    }
}

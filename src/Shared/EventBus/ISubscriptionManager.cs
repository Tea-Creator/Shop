using System;
using System.Collections.Generic;

namespace EventBus
{
    public interface ISubscriptionManager
    {
        string GetEventName<T>();
        Type GetEventType(string eventName);

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        bool HasAnyHandler<T>();
        bool HasAnyHandler(string @event);

        IEnumerable<Type> GetHandlersOf(string @event);

        IEnumerable<Type> GetHandlersOf<T>() where T : IntegrationEvent;
    }
}

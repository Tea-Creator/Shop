using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus
{
    public class NullEventBus : IEventBus
    {
        public void Publish<TEvent>(TEvent integrationEvent) where TEvent : IntegrationEvent
        {
        }

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
        }

        public void Unsubscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
        }
    }
}

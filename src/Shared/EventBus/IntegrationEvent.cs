using System;
using System.Text.Json.Serialization;

namespace EventBus
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createdDate)
        {
            Id = id;
            CreatedDate = createdDate;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; }

        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ProcessedOn { get; set; }
        public string? Error { get; set; }
    }
}

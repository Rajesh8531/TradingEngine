using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interface
{
    public interface IEventPublisher
    {
        Task PublishAsync(ReadOnlyMemory<byte> body, string ExchangeName, string routingKey, CancellationToken ct = default);
    }
}

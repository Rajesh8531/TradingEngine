using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common
{
    public record OrderPayloadForQueue(
        Guid UserId, 
        Guid OrderId, 
        string Symbol, 
        OrderType Side,
        decimal Price, 
        decimal Quantity, 
        OrderStatus Status, 
        DateTimeOffset CreatedAt
        );
}

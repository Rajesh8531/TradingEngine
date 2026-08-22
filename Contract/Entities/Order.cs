using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Symbol Symbol { get; set; }
        public OrderType Side { get; set; }
        public decimal Price { get; set; }
        public int OriginalQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public OrderStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}

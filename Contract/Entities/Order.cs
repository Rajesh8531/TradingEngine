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
        public User User { get; set; } = default!;
        public string Symbol { get; set; } = string.Empty;
        public Stock Stock { get; set; } = default!; 
        public OrderType Side { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }
        public OrderStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}

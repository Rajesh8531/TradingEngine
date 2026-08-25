using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Trade
    {
        public Guid Id { get; set; }
        public Guid MakerOrderId { get; set; }
        public Order MakerOrder { get; set; } = default!;
        public Guid TakerOrderId { get; set; }
        public Order TakerOrder { get; set; } = default!;
        public string Symbol { get; set; } = string.Empty;
        public decimal ExecutionPrice { get; set; }
        public DateTimeOffset ExecutedAt { get; set; }
        public decimal Quantity { get; set; }

    }
}

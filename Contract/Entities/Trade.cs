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
        public Guid TakerOrderId { get; set; }
        public Symbol Symbol { get; set; }
        public decimal ExecutedPrice { get; set; }
        public DateTimeOffset ExecutedAt { get; set; }
        public int Quantity { get; set; }

    }
}

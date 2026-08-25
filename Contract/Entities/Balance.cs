using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Balance
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public string AssetSymbol { get; set; } = string.Empty;
        public decimal AvailableBalance { get; set; }
        public decimal LockedBalance { get; set; }
    }
}

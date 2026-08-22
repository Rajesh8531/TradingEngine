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
        public Symbol AssetSymbol { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal LockedBalance { get; set; }
    }
}

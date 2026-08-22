using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Stock
    {
        public Symbol Symbol { get; set; }
        public string CompanyName { get; set; } = string.Empty;
    }
}

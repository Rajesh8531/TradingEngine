using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<Balance> Balances { get; set; } = new List<Balance>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

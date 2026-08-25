using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs
{
    public record CreateOrderResponseDTO(Guid Id, decimal Quantity, OrderType Side, string Symbol, Guid UserId);
}

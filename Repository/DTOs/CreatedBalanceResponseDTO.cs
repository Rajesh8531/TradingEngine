using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs
{
    public record CreatedBalanceResponseDTO(Guid UserId, string AssetSymbol, decimal AvailableBalance, decimal LockedBalance);
}

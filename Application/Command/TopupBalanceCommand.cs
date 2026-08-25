using Application.Common;
using Infrastructure.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Command
{
    public record TopupBalanceCommand(Guid UserId, string AssetSymbol, decimal QuantityToBeAdded) : IRequest<Result<TopupBalanceResponseDTO>>;
}

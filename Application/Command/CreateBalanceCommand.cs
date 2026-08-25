using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.DTOs;
using Application.Common;

namespace Application.Command
{
    public record CreateBalanceCommand(Guid UserId, string AssetSymbol, decimal Quantity) : IRequest<Result<CreatedBalanceResponseDTO>>;
}

using Application.Common;
using Infrastructure.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Command
{
    public record MakeTradeCommand(Guid MakerOrderId, Guid TakerOrderId, string Symbol, decimal Quantity) : IRequest<Result<MakeTradeResponseDTO>>;
}

using Application.Common;
using Domain.Enums;
using Infrastructure.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Command
{
    public record CreateOrderCommand(Guid UserId, string Symbol,OrderType Side, decimal Price, decimal Quantity) : IRequest<Result<CreateOrderResponseDTO>>;
}

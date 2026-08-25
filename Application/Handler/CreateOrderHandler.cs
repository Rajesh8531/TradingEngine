using Application.Command;
using Application.Common;
using Application.Common.Enums;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTOs;
using Infrastructure.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Application.Handler
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<CreateOrderResponseDTO>>
    {
        private readonly IUserRepository _users;
        private readonly IBalanceRepository _balances;
        private readonly IOutboxMessageRepository _outboxMessages;
        private readonly IOrderRepository _orders;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderHandler(IUserRepository users, IBalanceRepository balances, IOutboxMessageRepository outboxMessages, IOrderRepository orders,IUnitOfWork unitOfWork)
        {
            _users = users;
            _balances = balances;
            _outboxMessages = outboxMessages;
            _orders = orders;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateOrderResponseDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var isUserExist = await _users.UserExistsAsync(request.UserId, cancellationToken);
            if (!isUserExist)
            {
                return Result<CreateOrderResponseDTO>.Failure([new Error() { ErrorCode = ErrorType.Authentication }], ErrorType.Authentication);
            }

            Balance? existingBalance;

            if(request.Side == OrderType.BUY)
            {
                existingBalance = await _balances.GetBalanceAsync(request.UserId, "USD", cancellationToken);
            } else
            {
                existingBalance = await _balances.GetBalanceAsync(request.UserId, request.Symbol, cancellationToken);
            }
            
            if(existingBalance == null)
            {
                return Result<CreateOrderResponseDTO>.Failure([new Error() { ErrorCode = ErrorType.NotFound }], ErrorType.NotFound);
            }

            bool haveEnoughBalance;

            if(request.Side == OrderType.BUY)
            {
                haveEnoughBalance = existingBalance.AvailableBalance >= request.Price * request.Quantity;
            } else
            {
                haveEnoughBalance = existingBalance.AvailableBalance >= request.Quantity;
            }

            if (!haveEnoughBalance)
            {
                return Result<CreateOrderResponseDTO>.Failure([new Error() { ErrorCode = ErrorType.Conflict }], ErrorType.Conflict);
            }

            var order = new Order()
            {
                Id = Guid.NewGuid(),
                Price = request.Price,
                OriginalQuantity = request.Quantity,
                RemainingQuantity = request.Quantity,
                Side = request.Side,
                Status = OrderStatus.PENDING,
                UserId = request.UserId,
                Symbol = request.Symbol,
                CreatedAt = DateTime.UtcNow,
            };

            var queuePayload = new OrderPayloadForQueue(
                UserId: order.UserId, 
                OrderId: order.Id, 
                Symbol: request.Symbol, 
                Side: request.Side, 
                Price: request.Price, 
                Quantity: request.Quantity, 
                Status: order.Status, 
                CreatedAt: order.CreatedAt
            );

            var payload = JsonSerializer.Serialize(queuePayload);

            var outboxMessage = new OutboxMessage()
            {
                CreatedAt = order.CreatedAt,
                Error = null,
                EventType = "order.created",
                Payload = payload,
                ReferenceId = order.Id
            };

            if (request.Side == OrderType.BUY)
            {
                existingBalance.AvailableBalance -= request.Price * request.Quantity;
                existingBalance.LockedBalance += request.Price * request.Quantity;
            }
            else
            {
                existingBalance.AvailableBalance -= request.Quantity;
                existingBalance.LockedBalance += request.Quantity;
            }

            _outboxMessages.Add(outboxMessage);
            _orders.Add(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = new CreateOrderResponseDTO(Id: order.Id, UserId: order.UserId, Side: order.Side, Quantity: request.Quantity, Symbol: order.Symbol);
            return Result<CreateOrderResponseDTO>.Success(dto);
        }
    }
}

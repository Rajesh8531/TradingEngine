using Application.Command;
using Application.Common;
using Application.Common.Enums;
using Domain.Entities;
using Infrastructure.DTOs;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handler
{
    internal class CreateBalanceHandler : IRequestHandler<CreateBalanceCommand, Result<CreatedBalanceResponseDTO>>
    {
        private readonly IUserRepository _users;
        private readonly IBalanceRepository _balances;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBalanceHandler( IUserRepository users, IBalanceRepository balances, IUnitOfWork unitOfWork)
        {
            _users = users;
            _balances = balances;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreatedBalanceResponseDTO>> Handle(CreateBalanceCommand request, CancellationToken cancellationToken)
        {
            var isBalanceExists = await _balances.BalanceExistsAsync(request.UserId, request.AssetSymbol, cancellationToken);
            if (isBalanceExists)
            {
                return Result<CreatedBalanceResponseDTO>.Failure([new Error { ErrorCode = ErrorType.NotFound, Message = "Balance already exists for this user and asset" }], ErrorType.NotFound);
            }

            var isUserExists = await _users.UserExistsAsync(request.UserId, cancellationToken);
            if(!isUserExists)
            {
                return Result<CreatedBalanceResponseDTO>.Failure([new Error { ErrorCode = ErrorType.NotFound, Message = "User not found" }], ErrorType.NotFound);
            }

            var balance = new Balance
            {
                UserId = request.UserId,
                AssetSymbol = request.AssetSymbol,
                AvailableBalance = request.Quantity,
                LockedBalance = 0
            };

            _balances.AddBalance(balance);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<CreatedBalanceResponseDTO>.Success(new CreatedBalanceResponseDTO(request.UserId, request.AssetSymbol, request.Quantity, 0));
        }
    }
}

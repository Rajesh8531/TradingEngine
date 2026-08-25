using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Contracts
{
    public interface IBalanceRepository
    {
        public Task<Balance> GetBalanceAsync(Guid userId, string assetSymbol, CancellationToken cancellationToken);
        public Task<bool> BalanceExistsAsync(Guid userId, string assetSymbol, CancellationToken cancellationToken);
        public void AddBalance(Balance balance, CancellationToken cancellationToken);
    }
}

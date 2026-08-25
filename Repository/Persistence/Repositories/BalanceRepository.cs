using Domain.Entities;
using Infrastructure.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {

        public Task<bool> BalanceExistsAsync(Guid userId, string assetSymbol, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Balance> GetBalanceAsync(Guid userId, string assetSymbol, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IBalanceRepository.AddBalance(Balance balance, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

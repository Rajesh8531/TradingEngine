using Domain.Entities;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly ApplicationDbContext _context;
        public BalanceRepository(ApplicationDbContext context) {
            _context = context;
        }
        public void AddBalance(Balance balance)
        {
            _context.Balances.Add(balance);
        }

        public Task<bool> BalanceExistsAsync(Guid userId, string assetSymbol, CancellationToken cancellationToken)
        {
            return _context.Balances.AnyAsync(b => b.UserId == userId && b.AssetSymbol == assetSymbol, cancellationToken);
        }

        public Task<Balance?> GetBalanceAsync(Guid userId, string assetSymbol, CancellationToken cancellationToken)
        {
            return _context.Balances.Where(b => b.UserId == userId && b.AssetSymbol == assetSymbol).FirstOrDefaultAsync(cancellationToken);
        }

    }
}

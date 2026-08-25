using Domain.Entities;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken)
        {
            return await _context.Users.Where(u => u.Id == Id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> UserExistsAsync(Guid Id, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(u => u.Id == Id, cancellationToken);
        }
    }
}

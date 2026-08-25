using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Contracts
{
    public interface IUserRepository
    {
        public Task<User> GetUserAsync(Guid Id, CancellationToken cancellationToken);
        public Task<bool> UserExistsAsync(Guid Id, CancellationToken cancellationToken);
    }
}

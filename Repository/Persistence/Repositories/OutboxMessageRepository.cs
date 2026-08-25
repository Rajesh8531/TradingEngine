using Domain.Entities;
using Infrastructure.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class OutboxMessageRepository : IOutboxMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public OutboxMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(OutboxMessage message)
        {
            _context.OutboxMessages.Add(message);
        }
    }
}

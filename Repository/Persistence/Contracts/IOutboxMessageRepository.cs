using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Contracts
{
    public interface IOutboxMessageRepository
    {
        void Add(OutboxMessage message);
    }
}

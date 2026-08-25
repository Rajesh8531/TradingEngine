using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Payload)
                .IsRequired()
                .HasColumnType("jsonb");

            builder.Property(x => x.EventType)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.CreatedAt)
                   .HasFilter("\"ProcessedOn\" IS NULL");

            builder.HasIndex(x => x.ReferenceId);
        }
    }
}

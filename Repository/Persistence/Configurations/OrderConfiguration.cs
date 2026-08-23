using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price).HasPrecision(18, 8);

            // Save Enums as strings for direct DB readability
            builder.Property(x => x.Side)
                .HasConversion<string>()
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Foreign Keys - NEVER cascade delete orders if a user is deleted
            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Stock)
                .WithMany()
                .HasForeignKey(x => x.Symbol)
                .OnDelete(DeleteBehavior.Restrict);

            // Queue Index: The matching engine needs to find Pending orders for a specific symbol, sorted by time
            builder.HasIndex(x => new { x.Symbol, x.Status, x.CreatedAt });
        }
    }
}

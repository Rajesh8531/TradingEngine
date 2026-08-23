using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    internal class TradeConfiguration : IEntityTypeConfiguration<Trade>
    {
        public void Configure(EntityTypeBuilder<Trade> builder)
        {
            builder.ToTable("Trades");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ExecutionPrice).HasPrecision(18, 8);

            // Prevent Cascade Deletes
            builder.HasOne(x => x.MakerOrder)
                .WithMany() // Assuming no navigation collection on Order for Trades
                .HasForeignKey(x => x.MakerOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TakerOrder)
                .WithMany()
                .HasForeignKey(x => x.TakerOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for generating price charts (e.g., TradingView candlestick charts)
            builder.HasIndex(x => new { x.Symbol, x.ExecutedAt });
        }
    }
}

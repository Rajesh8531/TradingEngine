using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    internal class BalanceConfiguration : IEntityTypeConfiguration<Balance>
    {
        public void Configure(EntityTypeBuilder<Balance> builder)
        {
            builder.ToTable("Balances");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AssetSymbol)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(10);

            // Precision 18, Scale 8 is standard for crypto/fractional stocks
            builder.Property(x => x.AvailableBalance).HasPrecision(18, 8);
            builder.Property(x => x.LockedBalance).HasPrecision(18, 8);

            // A user can only have one balance record per asset (e.g., one AAPL wallet)
            builder.HasIndex(x => new { x.UserId, x.AssetSymbol })
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Balances)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

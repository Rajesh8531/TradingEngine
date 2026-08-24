using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    internal class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stocks");

            // Using Symbol as PK instead of UUID
            builder.HasKey(x => x.Symbol);

            builder.Property(x => x.Symbol)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.CompanyName)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}

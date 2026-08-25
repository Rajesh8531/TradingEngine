using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Infrastructure.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDataBase(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            if(! await context.Stocks.AnyAsync())
            {
                var stocks = new List<Stock> {

                    new Stock { Symbol = "AAPL", CompanyName = "Apple Inc." },
                    new Stock { Symbol = "MSFT", CompanyName = "Microsoft Corporation" },
                    new Stock { Symbol = "GOOGL", CompanyName = "Alphabet Inc." },
                    new Stock { Symbol = "AMZN", CompanyName = "Amazon.com Inc." },
                    new Stock { Symbol = "NVDA", CompanyName = "NVIDIA Corporation" },
                    new Stock { Symbol = "TSLA", CompanyName = "Tesla Inc." },
                    new Stock { Symbol = "META", CompanyName = "Meta Platforms Inc." },
                };

                await context.Stocks.AddRangeAsync(stocks,cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        public static string GetDescription(this Enum enumValue)
        {
            // Get the field information for the specific enum value
            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());

            if (field == null)
                return enumValue.ToString();

            // Check if the Description attribute is present
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            // Return description text if found; otherwise, fall back to string representation
            return attribute != null ? attribute.Description : enumValue.ToString();
        }
    }
}

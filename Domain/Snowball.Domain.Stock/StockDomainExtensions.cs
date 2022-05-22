using Microsoft.Extensions.DependencyInjection;
using Snowball.Domain.Stock.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Domain.Stock
{
    public static class StockDomainExtensions
    {
        public static IServiceCollection AddStockDomain(this IServiceCollection services)
        {
            return services.AddSingleton<IIndexValuationService, IndexValuationService>();
        }
    }
}

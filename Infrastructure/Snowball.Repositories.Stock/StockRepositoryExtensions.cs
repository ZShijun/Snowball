using Microsoft.Extensions.DependencyInjection;
using Snowball.Domain.Stock.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Repositories.Stock
{
    public static class StockRepositoryExtensions
    {
        public static IServiceCollection AddStockRepository(this IServiceCollection services)
        {
            return services.AddSingleton<IIndexValuationRepository, IndexValuationRepository>();
        }
    }
}

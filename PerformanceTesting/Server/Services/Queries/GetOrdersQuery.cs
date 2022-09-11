using PerformanceTesting.Server.Services.Data;
using SnowStorm.QueryExecutors;

namespace PerformanceTesting.Server.Services.Queries
{
    public class GetOrdersQuery : IQueryResultList<Order>
    {

        public IQueryable<Order> Get(IQueryableProvider queryableProvider)
        {
            var query = queryableProvider.Query<Order>()
               .OrderBy(o => o.OrderDate)
               .AsQueryable();

            return query;
        }
    }
}

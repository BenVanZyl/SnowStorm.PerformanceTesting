using PerformanceTesting.Server.Services.Data;
using SnowStorm.QueryExecutors;

namespace PerformanceTesting.Server.Services.Queries
{
    public class GetOrderQuery : IQueryResultSingle<Order>
    {
        private readonly int _id;

        public GetOrderQuery(int id)
        {
            _id = id;
        }

        public IQueryable<Order> Get(IQueryableProvider queryableProvider)
        {
            var query = queryableProvider.Query<Order>()
                .Where(w => w.Id == _id)
                .OrderBy(o => o.OrderDate)
               .AsQueryable();

            return query;
        }
    }
}

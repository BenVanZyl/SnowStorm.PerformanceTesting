using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SnowStorm.QueryExecutors;
using PerformanceTesting.Server.Services.Queries;
using Serilog;
using Microsoft.EntityFrameworkCore;
using SnowStorm.Domain;
using PerformanceTesting.Server.Services.Data;

namespace PerformanceTesting.Server.Services.Api
{
    public class SampleController : Controller
    {
        public SampleController(IQueryExecutor executor, AppDbContext dbContext)
        {
            _executor = executor;
            _dbContext = dbContext;
        }

        private readonly AppDbContext _dbContext;
        private readonly IQueryExecutor _executor;

        [HttpGet]
        [Route("api/samples")]
        [Route("api/samples/{count}")]
        //[ValidateAntiForgeryToken()]
        public async Task<IActionResult> GetSamples(int count = 128)
        {
            try
            {
                var results = new List<Tuple<int, string>>();
                for (int i = 0; i < count; i++)
                {
                    results.Add(new Tuple<int, string>(i, Guid.NewGuid().ToString()));
                }
                return Ok(results);
            }
            catch (System.Exception ex)
            {

                Log.Error(ex, "GetSamples ERROR");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("api/orders")]
        //[ValidateAntiForgeryToken()]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var results = await _executor.Get(new GetOrdersQuery());
                return Ok(results);
            }
            catch (System.Exception ex)
            {

                Log.Error(ex, "GetOrders ERROR");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("api/orders/direct")]
        //[ValidateAntiForgeryToken()]
        public async Task<IActionResult> GetOrdersDirect()
        {
            try
            {
                var results = new QueryableProvider(_dbContext).Query<Order>()
                    .OrderBy(o => o.OrderDate)
                    .AsQueryable();
                return Ok(results);
            }
            catch (System.Exception ex)
            {

                Log.Error(ex, "GetOrders ERROR");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        [Route("api/orders")]
        //[ValidateAntiForgeryToken()]
        public async Task<IActionResult> PostOrders()
        {
            try
            {
                var results = await _executor.Get(new GetOrdersQuery());
                return Ok(results);
            }
            catch (System.Exception ex)
            {

                Log.Error(ex, "PostOrders ERROR");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}

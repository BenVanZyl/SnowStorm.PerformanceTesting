using AutoMapper.Execution;
using MediatR;
using SnowStorm.QueryExecutors;

namespace PerformanceTesting.Server.Services.Commands
{
    public class SaveSampleCommand : IRequest<bool>
    {
    }

    public class SaveSampleCommandHandler : IRequestHandler<SaveSampleCommand, bool>
    {
        private readonly IQueryExecutor _executor;

        public SaveSampleCommandHandler(IQueryExecutor executor)
        {
            _executor = executor;
        }

        public async Task<bool> Handle(SaveSampleCommand request, CancellationToken cancellationToken)
        {
            
            await _executor.Save();

            return true;

            //throw new ThisAppExecption(StatusCodes.Status400BadRequest, "Record not found."



        }
    }
}

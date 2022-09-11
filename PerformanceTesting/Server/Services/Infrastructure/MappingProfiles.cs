using AutoMapper;
using PerformanceTesting.Server.Services.Data;
using PerformanceTesting.Shared;

namespace JkaPerth.Web.Services.Infrastructure
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Order, OrderDto>();


        }
    }
}

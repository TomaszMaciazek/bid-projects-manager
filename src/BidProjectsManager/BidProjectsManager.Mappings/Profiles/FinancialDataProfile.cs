using AutoMapper;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.Mappings.Profiles
{
    public class FinancialDataProfile : Profile
    {
        public FinancialDataProfile()
        {
            CreateMap<CreateEbitCommand, BidEbit>();

            CreateMap<CreateCapexCommand, BidCapex>();

            CreateMap<CreateOpexCommand, BidOpex>();

            CreateMap<BidCapex, CapexDto>();

            CreateMap<BidOpex, OpexDto>();

            CreateMap<BidEbit, EbitDto>();
        }
    }
}

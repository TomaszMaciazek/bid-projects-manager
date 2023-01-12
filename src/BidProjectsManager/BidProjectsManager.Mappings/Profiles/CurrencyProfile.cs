using AutoMapper;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.Mappings.Profiles
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile() {
            CreateMap<Currency, CurrencyDto>();
            CreateMap<Currency, CurrencyListItemDto>()
                .ForMember(dest => dest.IsDeletable, opt => opt.MapFrom(src => !src.Countries.Any()));
        }
    }
}

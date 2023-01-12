using AutoMapper;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.Mappings.Profiles
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryDto>();
            CreateMap<Country, CountryListItemDto>()
                .ForMember(dest => dest.IsDeletable, opt => opt.MapFrom(src => !src.Projects.Any()));
        }
    }
}

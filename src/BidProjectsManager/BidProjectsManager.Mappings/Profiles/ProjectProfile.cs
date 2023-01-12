using AutoMapper;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;
using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Mappings.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<CreateDraftProjectCommand, Project>()
                .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => ProjectStage.Draft))
                .ForMember(dest => dest.ApprovalDate, opt => opt.Ignore());

            CreateMap<CreateSubmittedProjectCommand, Project>()
                .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => ProjectStage.Submited))
                .ForMember(dest => dest.ApprovalDate, opt => opt.Ignore());

            CreateMap<Project, ProjectDto>();

            CreateMap<Project, ProjectListItemDto>();

            CreateMap<UpdateDraftProjectCommand, Project>()
                .ForMember(dest => dest.Stage, opt => opt.MapFrom(x => ProjectStage.Draft))
                .ForMember(dest => dest.ApprovalDate, opt => opt.Ignore())
                .ForMember(dest => dest.Capexes, opt => opt.Ignore())
                .ForMember(dest => dest.Ebits, opt => opt.Ignore())
                .ForMember(dest => dest.Opexes, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectCurrency, opt => opt.Ignore());

            CreateMap<SubmitProjectCommand, Project>()
                .ForMember(dest => dest.ApprovalDate, opt => opt.Ignore())
                .ForMember(dest => dest.Capexes, opt => opt.Ignore())
                .ForMember(dest => dest.Ebits, opt => opt.Ignore())
                .ForMember(dest => dest.Opexes, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectCurrency, opt => opt.Ignore());
        }
    }
}

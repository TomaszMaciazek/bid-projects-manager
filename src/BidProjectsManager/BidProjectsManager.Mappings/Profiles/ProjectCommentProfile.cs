using AutoMapper;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.Mappings.Profiles
{
    public class ProjectCommentProfile : Profile
    {
        public ProjectCommentProfile()
        {
            CreateMap<ProjectComment, CommentDto>();

            CreateMap<CreateCommentCommand, ProjectComment>();
        }
    }
}

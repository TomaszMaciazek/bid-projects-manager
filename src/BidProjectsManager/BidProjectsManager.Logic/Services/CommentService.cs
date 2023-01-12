using AutoMapper;
using AutoMapper.QueryableExtensions;
using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Logic.Services
{
    public interface ICommentService
    {
        Task<bool> CreateCommentAsync(CreateCommentCommand command);
        Task<IList<CommentDto>> GetCommentsFromProjectAsync(int projectId);
    }

    public class CommentService : ICommentService
    {
        private readonly IProjectCommentRepository _projectCommentRepository;
        private IValidator<CreateCommentCommand> _createCommentCommandValidator;
        private IMapper _mapper;

        public CommentService(IProjectCommentRepository projectCommentRepository, IValidator<CreateCommentCommand> createCommentCommandValidator, IMapper mapper)
        {
            _projectCommentRepository = projectCommentRepository;
            _createCommentCommandValidator = createCommentCommandValidator;
            _mapper = mapper;
        }

        public async Task<IList<CommentDto>> GetCommentsFromProjectAsync(int projectId)
            => await _projectCommentRepository.GetAll().AsNoTracking()
            .Where(x => x.ProjectId == projectId)
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task<bool> CreateCommentAsync(CreateCommentCommand command)
        {
            var validationResult = await _createCommentCommandValidator.ValidateAsync(command);
            if (validationResult.IsValid)
            {
                var comment = new ProjectComment
                {
                    Created = DateTime.Now,
                    Content = command.Content,
                    ProjectId = command.ProjectId
                };

                _projectCommentRepository.Add(comment);
                await _projectCommentRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

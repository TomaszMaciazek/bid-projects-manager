using AutoMapper;
using AutoMapper.QueryableExtensions;
using BidProjectsManager.DataLayer.Common;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCommentCommand> _createCommentCommandValidator;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IValidator<CreateCommentCommand> createCommentCommandValidator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _createCommentCommandValidator = createCommentCommandValidator;
            _mapper = mapper;
        }

        public async Task<IList<CommentDto>> GetCommentsFromProjectAsync(int projectId)
            => await _unitOfWork.ProjectCommentRepository.GetAll().AsNoTracking()
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

                _unitOfWork.ProjectCommentRepository.Add(comment);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

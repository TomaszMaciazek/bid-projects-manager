using AutoMapper;
using AutoMapper.QueryableExtensions;
using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.Logic.Extensions;
using BidProjectsManager.Logic.Result;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;
using BidProjectsManager.Model.Enums;
using BidProjectsManager.Model.Queries;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Logic.Services
{
    public interface IProjectService
    {
        Task<bool> ApproveProjectAsync(int id);
        Task<bool> CreateDraftProjectAsync(CreateDraftProjectCommand command);
        Task<bool> CreateSubmittedProjectAsync(CreateSubmittedProjectCommand command);
        Task<bool> DeleteProjectAsync(int id);
        Task<PaginatedList<ProjectListItemDto>> GetProjectsAsync(ProjectQuery query);
        Task<ProjectDto> GetProjectByIdAsync(int id);
        Task<bool> RejectProjectAsync(int id);
        Task<bool> UpdateProjectDraftAsync(UpdateDraftProjectCommand command);
        Task<bool> SubmitProject(SubmitProjectCommand command);
    }

    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateDraftProjectCommand> _createDraftProjectValidator;
        private readonly IValidator<CreateSubmittedProjectCommand> _createSubmittedProjectValidator;
        private readonly IValidator<UpdateDraftProjectCommand> _updateDraftProjectValidator;
        private readonly IValidator<SubmitProjectCommand> _submitProjectValidator;
        private readonly IMapper _mapper;

        public ProjectService(
            IUnitOfWork unitOfWork,
            IValidator<CreateDraftProjectCommand> createDraftProjectValidator,
            IValidator<CreateSubmittedProjectCommand> createSubmittedProjectValidator,
            IValidator<UpdateDraftProjectCommand> updateDraftProjectValidator,
            IValidator<SubmitProjectCommand> submitProjectValidator,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _createDraftProjectValidator = createDraftProjectValidator;
            _createSubmittedProjectValidator = createSubmittedProjectValidator;
            _updateDraftProjectValidator = updateDraftProjectValidator;
            _submitProjectValidator = submitProjectValidator;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProjectListItemDto>> GetProjectsAsync(ProjectQuery query)
        {
            var projects = _unitOfWork.ProjectRepository.GetAll().AsNoTracking();

            projects = !string.IsNullOrEmpty(query.Name)
                ? projects.Where(x => x.Name.ToLower().Contains(query.Name.ToLower()))
                : projects;

            projects = query.Statuses != null && query.Statuses.Any()
                ? projects.Where(x => x.Status.HasValue && query.Statuses.Contains(x.Status.Value))
                : projects;

            projects = query.Stages != null && query.Stages.Any()
                ? projects.Where(x => query.Stages.Contains(x.Stage))
                : projects;

            projects = query.CountriesIds != null && query.CountriesIds.Any()
                ? projects.Where(x => query.CountriesIds.Contains(x.Id))
                : projects;

            if (query.SortOption.HasValue)
            {
                if(query.SortOption.Value == ProjectSortOption.IdDescending) {
                    projects = projects.OrderByDescending(x => x.Id);
                }
                else if (query.SortOption.Value == ProjectSortOption.NameAscending)
                {
                    projects = projects.OrderBy(x => x.Name);
                }
                else if (query.SortOption.Value == ProjectSortOption.NameDescending)
                {
                    projects = projects.OrderByDescending(x => x.Name);
                }
            }

            return await projects.ProjectTo<ProjectListItemDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(query.PageNumber, query.PageSize);
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int id) 
            => await _unitOfWork.ProjectRepository.GetByIdEager(id)
            .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        public async Task<bool> CreateDraftProjectAsync(CreateDraftProjectCommand command)
        {
            try
            {
                var validationResult = await _createDraftProjectValidator.ValidateAsync(command);
                if (validationResult.IsValid)
                {
                    var project = _mapper.Map<Project>(command);
                    project.CreatedBy = "admin";
                    project.Created = DateTime.Now;
                    _unitOfWork.ProjectRepository.Add(project);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateSubmittedProjectAsync(CreateSubmittedProjectCommand command)
        {
            try
            {
                var validationResult = _createSubmittedProjectValidator.Validate(command);
                if (validationResult.IsValid)
                {
                    var project = _mapper.Map<Project>(command);
                    _unitOfWork.ProjectRepository.Add(project);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateProjectDraftAsync(UpdateDraftProjectCommand command)
        {
            try
            {
                var validationResult = await _updateDraftProjectValidator.ValidateAsync(command);
                if (validationResult.IsValid)
                {
                    var project = await _unitOfWork.ProjectRepository.GetById(command.Id)
                    .FirstOrDefaultAsync();
                    if (project != null)
                    {
                        project.Name= command.Name;
                        project.Description= command.Description;
                        project.ApprovalDate = null;
                        project.BidEstimatedOperationEnd = command.BidEstimatedOperationEnd;
                        project.BidOperationStart = command.BidOperationStart;
                        project.Status = command.Status;
                        project.Stage = ProjectStage.Draft;
                        project.CountryId= command.CountryId;
                        project.CurrencyId= command.CurrencyId;
                        project.Modified = DateTime.Now;
                        project.ModifiedBy = "admin";
                        project.LifetimeInThousandsKilometers = command.LifetimeInThousandsKilometers;
                        project.NoBidReason = command.NoBidReason;
                        project.NumberOfVechicles = command.NumberOfVechicles;
                        project.OptionalExtensionYears = command.OptionalExtensionYears;
                        project.Priority = command.Priority;
                        project.Probability= command.Probability;
                        project.TotalCapex = command.TotalCapex;
                        project.TotalEbit = command.TotalEbit;
                        project.TotalOpex = command.TotalOpex;
                        project.Type = command.Type;
                        _unitOfWork.ProjectRepository.Update(project);
                        await _unitOfWork.SaveChangesAsync();
                        await UpdateProjectFinancialDataFromCommand(command);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SubmitProject(SubmitProjectCommand command)
        {
            try
            {
                var validationResult = _submitProjectValidator.Validate(command);
                var project = await _unitOfWork.ProjectRepository.GetById(command.Id)
                    .Include(x => x.Ebits)
                    .Include(x => x.Capexes)
                    .Include(x => x.Opexes)
                    .FirstOrDefaultAsync();
                if (project != null)
                {
                    _mapper.Map(command, project);
                    await _unitOfWork.SaveChangesAsync();
                    await UpdateProjectFinancialDataFromCommand(command);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ApproveProjectAsync(int id)
        {
            var project = await _unitOfWork.ProjectRepository.GetById(id).FirstOrDefaultAsync();
            if (project == null || project.Stage != ProjectStage.Submited)
            {
                return false;
            }
            project.Stage = ProjectStage.Approved;
            project.ApprovalDate = DateTime.Now;
            _unitOfWork.ProjectRepository.Update(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectProjectAsync(int id)
        {
            var project = await _unitOfWork.ProjectRepository.GetById(id).FirstOrDefaultAsync();
            if (project == null || project.Stage != ProjectStage.Submited)
            {
                return false;
            }
            project.Stage = ProjectStage.Rejected;
            _unitOfWork.ProjectRepository.Update(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _unitOfWork.ProjectRepository.GetById(id).AsNoTracking().FirstOrDefaultAsync();
            if (project == null || project.Stage == ProjectStage.Approved)
            {
                return false;
            }
            _unitOfWork.ProjectRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private async Task UpdateProjectFinancialDataFromCommand(BaseUpdateProjectCommand command)
        {
            var projectEbits = await _unitOfWork.EbitRepository.GetAll().Where(x => x.ProjectId == command.Id).ToListAsync();
            var projectCapexes = await _unitOfWork.CapexRepository.GetAll().Where(x => x.ProjectId == command.Id).ToListAsync();
            var projectOpexes = await _unitOfWork.OpexRepository.GetAll().Where(x => x.ProjectId == command.Id).ToListAsync();

            var capexesToRemove = projectCapexes.Where(x => command.YearsToRemove.Contains(x.Year));
            var capexesToEdit = projectCapexes.Where(x => command.Capexes.Any(y => y.Id == x.Id));
            var ebitsToRemove = projectEbits.Where(x => command.YearsToRemove.Contains(x.Year));
            var ebitsToEdit = projectEbits.Where(x => command.Ebits.Any(y => y.Id == x.Id));
            var opexesToRemove = projectOpexes.Where(x => command.YearsToRemove.Contains(x.Year));
            var opexesToEdit = projectOpexes.Where(x => command.Opexes.Any(y => y.Id == x.Id));

            _unitOfWork.OpexRepository.RemoveRange(opexesToRemove);
            _unitOfWork.EbitRepository.RemoveRange(ebitsToRemove);
            _unitOfWork.CapexRepository.RemoveRange(capexesToRemove);
            await _unitOfWork.SaveChangesAsync();

            foreach (var ebit in ebitsToEdit)
            {
                var value = command.Ebits.FirstOrDefault(x => x.Id == ebit.Id).Value;
                ebit.Value = value; ;
                _unitOfWork.EbitRepository.Update(ebit);
            }

            foreach (var opex in opexesToEdit)
            {
                var value = command.Opexes.FirstOrDefault(x => x.Id == opex.Id).Value;
                opex.Value = value; ;
                _unitOfWork.OpexRepository.Update(opex);
            }

            foreach (var capex in capexesToEdit)
            {
                var value = command.Capexes.FirstOrDefault(x => x.Id == capex.Id).Value;
                capex.Value = value; ;
                _unitOfWork.CapexRepository.Update(capex);
            }

            await _unitOfWork.SaveChangesAsync();

            _unitOfWork.EbitRepository.AddRange(command.NewEbits.Select(ebit => new BidEbit
            {
                Id = 0,
                ProjectId = command.Id,
                Value = ebit.Value,
                Year = ebit.Year
            }).ToList());

            _unitOfWork.CapexRepository.AddRange(command.NewCapexes.Select(capex => new BidCapex
            {
                Id = 0,
                ProjectId = command.Id,
                Value = capex.Value,
                Year = capex.Year
            }).ToList());

            _unitOfWork.OpexRepository.AddRange(command.NewOpexes.Select(opex => new BidOpex
            {
                Id = 0,
                ProjectId = command.Id,
                Value = opex.Value,
                Year = opex.Year
            }).ToList());

            await _unitOfWork.SaveChangesAsync();

        }
    }
}

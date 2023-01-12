using AutoMapper;
using AutoMapper.QueryableExtensions;
using BidProjectsManager.DataLayer.Repositories;
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
        private readonly IProjectRepository _projectRepository;
        private readonly ICapexRepository _capexRepository;
        private readonly IEbitRepository _ebitRepository;
        private readonly IOpexRepository _opexRepository;
        private readonly IValidator<CreateDraftProjectCommand> _createDraftProjectValidator;
        private readonly IValidator<CreateSubmittedProjectCommand> _createSubmittedProjectValidator;
        private readonly IValidator<UpdateDraftProjectCommand> _updateDraftProjectValidator;
        private readonly IValidator<SubmitProjectCommand> _submitProjectValidator;
        private readonly IMapper _mapper;

        public ProjectService(
            IProjectRepository projectRepository,
            ICapexRepository capexRepository,
            IEbitRepository ebitRepository,
            IOpexRepository opexRepository,
            IValidator<CreateDraftProjectCommand> createDraftProjectValidator,
            IValidator<CreateSubmittedProjectCommand> createSubmittedProjectValidator,
            IValidator<UpdateDraftProjectCommand> updateDraftProjectValidator,
            IValidator<SubmitProjectCommand> submitProjectValidator,
            IMapper mapper
            )
        {
            _projectRepository = projectRepository;
            _capexRepository = capexRepository;
            _ebitRepository = ebitRepository;
            _opexRepository = opexRepository;
            _createDraftProjectValidator = createDraftProjectValidator;
            _createSubmittedProjectValidator = createSubmittedProjectValidator;
            _updateDraftProjectValidator = updateDraftProjectValidator;
            _submitProjectValidator = submitProjectValidator;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProjectListItemDto>> GetProjectsAsync(ProjectQuery query)
        {
            var projects = _projectRepository.GetAll().AsNoTracking();

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
                if(query.SortOption.Value == Model.Enums.ProjectSortOption.IdDescending) {
                    projects = projects.OrderByDescending(x => x.Id);
                }
                else if (query.SortOption.Value == Model.Enums.ProjectSortOption.NameAscending)
                {
                    projects = projects.OrderBy(x => x.Name);
                }
                else if (query.SortOption.Value == Model.Enums.ProjectSortOption.NameDescending)
                {
                    projects = projects.OrderByDescending(x => x.Name);
                }
            }

            return await projects.ProjectTo<ProjectListItemDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(query.PageNumber, query.PageSize);
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int id) 
            => await _projectRepository.GetByIdEager(id)
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
                    _projectRepository.Add(project);
                    await _projectRepository.SaveChangesAsync();
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
                    _projectRepository.Add(project);
                    await _projectRepository.SaveChangesAsync();
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
                    var project = await _projectRepository.GetById(command.Id)
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
                        _projectRepository.Update(project);
                        await _projectRepository.SaveChangesAsync();
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
                var project = await _projectRepository.GetById(command.Id)
                    .Include(x => x.Ebits)
                    .Include(x => x.Capexes)
                    .Include(x => x.Opexes)
                    .FirstOrDefaultAsync();
                if (project != null)
                {
                    _mapper.Map(command, project);
                    await _projectRepository.SaveChangesAsync();
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
            var project = await _projectRepository.GetById(id).FirstOrDefaultAsync();
            if (project == null || project.Stage != ProjectStage.Submited)
            {
                return false;
            }
            project.Stage = ProjectStage.Approved;
            project.ApprovalDate = DateTime.Now;
            _projectRepository.Update(project);
            await _projectRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectProjectAsync(int id)
        {
            var project = await _projectRepository.GetById(id).FirstOrDefaultAsync();
            if (project == null || project.Stage != ProjectStage.Submited)
            {
                return false;
            }
            project.Stage = ProjectStage.Rejected;
            _projectRepository.Update(project);
            await _projectRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _projectRepository.GetById(id).AsNoTracking().FirstOrDefaultAsync();
            if (project == null || project.Stage == ProjectStage.Approved)
            {
                return false;
            }
            _projectRepository.Delete(id);
            await _projectRepository.SaveChangesAsync();
            return true;
        }

        private async Task UpdateProjectFinancialDataFromCommand(BaseUpdateProjectCommand command)
        {
            var projectEbits = await _ebitRepository.GetAll().Where(x => x.ProjectId == command.Id).ToListAsync();
            var projectCapexes = await _capexRepository.GetAll().Where(x => x.ProjectId == command.Id).ToListAsync();
            var projectOpexes = await _opexRepository.GetAll().Where(x => x.ProjectId == command.Id).ToListAsync();

            var capexesToRemove = projectCapexes.Where(x => command.YearsToRemove.Contains(x.Year));
            var capexesToEdit = projectCapexes.Where(x => command.Capexes.Any(y => y.Id == x.Id));
            _capexRepository.RemoveRange(capexesToRemove);
            await _capexRepository.SaveChangesAsync();
            var ebitsToRemove = projectEbits.Where(x => command.YearsToRemove.Contains(x.Year));
            var ebitsToEdit = projectEbits.Where(x => command.Ebits.Any(y => y.Id == x.Id));
            _ebitRepository.RemoveRange(ebitsToRemove);
            await _ebitRepository.SaveChangesAsync();
            var opexesToRemove = projectOpexes.Where(x => command.YearsToRemove.Contains(x.Year));
            var opexesToEdit = projectOpexes.Where(x => command.Opexes.Any(y => y.Id == x.Id));
            _opexRepository.RemoveRange(opexesToRemove);
            await _opexRepository.SaveChangesAsync();

            _ebitRepository.AddRange(command.NewEbits.Select(ebit => new BidEbit
            {
                Id = 0,
                ProjectId = command.Id,
                Value = ebit.Value,
                Year = ebit.Year
            }).ToList());

            await _ebitRepository.SaveChangesAsync();

            _capexRepository.AddRange(command.NewCapexes.Select(capex => new BidCapex
            {
                Id = 0,
                ProjectId = command.Id,
                Value = capex.Value,
                Year = capex.Year
            }).ToList());

            await _capexRepository.SaveChangesAsync();

            _opexRepository.AddRange(command.NewOpexes.Select(opex => new BidOpex
            {
                Id = 0,
                ProjectId = command.Id,
                Value = opex.Value,
                Year = opex.Year
            }).ToList());

            await _opexRepository.SaveChangesAsync();

            foreach (var ebit in ebitsToEdit)
            {
                var value = command.Ebits.FirstOrDefault(x => x.Id == ebit.Id).Value;
                ebit.Value = value; ;
                _ebitRepository.Update(ebit);
            }

            foreach (var opex in opexesToEdit)
            {
                var value = command.Opexes.FirstOrDefault(x => x.Id == opex.Id).Value;
                opex.Value = value; ;
                _opexRepository.Update(opex);
            }

            foreach (var capex in capexesToEdit)
            {
                var value = command.Capexes.FirstOrDefault(x => x.Id == capex.Id).Value;
                capex.Value = value; ;
                _capexRepository.Update(capex);
            }

            await _opexRepository.SaveChangesAsync();

        }
    }
}

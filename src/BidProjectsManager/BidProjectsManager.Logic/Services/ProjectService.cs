using AutoMapper;
using AutoMapper.QueryableExtensions;
using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.Logic.Extensions;
using BidProjectsManager.Logic.Helpers;
using BidProjectsManager.Logic.Result;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;
using BidProjectsManager.Model.Enums;
using BidProjectsManager.Model.Queries;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace BidProjectsManager.Logic.Services
{
    public interface IProjectService
    {
        Task<bool> ApproveProjectAsync(int id, string userEmail);
        Task<bool> CreateDraftProjectAsync(CreateDraftProjectCommand command, string userEmail);
        Task<bool> CreateSubmittedProjectAsync(CreateSubmittedProjectCommand command, string userEmail);
        Task<bool> DeleteProjectAsync(int id);
        Task<PaginatedList<ProjectListItemDto>> GetProjectsAsync(ProjectQuery query);
        Task<ProjectDto> GetProjectByIdAsync(int id);
        Task<bool> RejectProjectAsync(int id, string userEmail);
        Task<bool> UpdateProjectDraftAsync(UpdateDraftProjectCommand command, string userEmail);
        Task<bool> SubmitProjectAsync(SubmitProjectCommand command, string userEmail);
        Task<bool> RollbackProjectToDraftAsync(int id, string userEmail);
        Task<byte[]> ExportProjectsDataAsync(ProjectQuery query);
        Task<byte[]> ExportProjectAsync(int id);
        Task<bool> SaveProjectAsync(SaveProjectCommand command, string userEmail);
    }

    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateDraftProjectCommand> _createDraftProjectValidator;
        private readonly IValidator<CreateSubmittedProjectCommand> _createSubmittedProjectValidator;
        private readonly IValidator<UpdateDraftProjectCommand> _updateDraftProjectValidator;
        private readonly IValidator<SubmitProjectCommand> _submitProjectValidator;
        private readonly IValidator<SaveProjectCommand> _saveProjectValidator;
        private readonly IMapper _mapper;

        public ProjectService(
            IUnitOfWork unitOfWork,
            IValidator<CreateDraftProjectCommand> createDraftProjectValidator,
            IValidator<CreateSubmittedProjectCommand> createSubmittedProjectValidator,
            IValidator<UpdateDraftProjectCommand> updateDraftProjectValidator,
            IValidator<SubmitProjectCommand> submitProjectValidator,
            IValidator<SaveProjectCommand> saveProjectValidator,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _createDraftProjectValidator = createDraftProjectValidator;
            _createSubmittedProjectValidator = createSubmittedProjectValidator;
            _updateDraftProjectValidator = updateDraftProjectValidator;
            _submitProjectValidator = submitProjectValidator;
            _saveProjectValidator = saveProjectValidator;
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

        public async Task<bool> CreateDraftProjectAsync(CreateDraftProjectCommand command, string userEmail)
        {
            try
            {
                var validationResult = await _createDraftProjectValidator.ValidateAsync(command);
                if (validationResult.IsValid)
                {
                    var project = _mapper.Map<Project>(command);
                    project.CreatedBy = userEmail;
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

        public async Task<bool> CreateSubmittedProjectAsync(CreateSubmittedProjectCommand command, string userEmail)
        {
            try
            {
                var validationResult = await _createSubmittedProjectValidator.ValidateAsync(command);
                if (validationResult.IsValid)
                {
                    var project = _mapper.Map<Project>(command);
                    project.Created = DateTime.Now;
                    project.CreatedBy = userEmail;
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

        public async Task<bool> UpdateProjectDraftAsync(UpdateDraftProjectCommand command, string userEmail)
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
                        _mapper.Map(command, project);
                        project.Modified= DateTime.Now;
                        project.ModifiedBy = userEmail;
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

        public async Task<bool> SubmitProjectAsync(SubmitProjectCommand command, string userEmail)
        {
            try
            {
                var validationResult = await _submitProjectValidator.ValidateAsync(command);
                if (validationResult.IsValid)
                {
                    var project = await _unitOfWork.ProjectRepository.GetById(command.Id)
                    .Include(x => x.Ebits)
                    .Include(x => x.Capexes)
                    .Include(x => x.Opexes)
                    .FirstOrDefaultAsync();
                    if (project != null)
                    {
                        _mapper.Map(command, project);
                        project.ModifiedBy = userEmail;
                        project.Modified = DateTime.Now;
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

        //
        public async Task<bool> SaveProjectAsync(SaveProjectCommand command, string userEmail)
        {
            try
            {
                var validationResult = await _saveProjectValidator.ValidateAsync(command);
                if (validationResult.IsValid)
                {
                    var project = await _unitOfWork.ProjectRepository.GetById(command.Id)
                    .Include(x => x.Ebits)
                    .Include(x => x.Capexes)
                    .Include(x => x.Opexes)
                    .FirstOrDefaultAsync();
                    if (project != null)
                    {
                        _mapper.Map(command, project);
                        project.ModifiedBy = userEmail;
                        project.Modified = DateTime.Now;
                        await _unitOfWork.SaveChangesAsync();
                        var projectEbits = await _unitOfWork.EbitRepository.GetAll().Where(x => x.ProjectId == command.Id).ToListAsync();
                        var projectCapexes = await _unitOfWork.CapexRepository.GetAll().Where(x => x.ProjectId == command.Id).ToListAsync();
                        var projectOpexes = await _unitOfWork.OpexRepository.GetAll().Where(x => x.ProjectId == command.Id).ToListAsync();

                        var capexesToEdit = projectCapexes.Where(x => command.Capexes.Any(y => y.Id == x.Id));
                        var ebitsToEdit = projectEbits.Where(x => command.Ebits.Any(y => y.Id == x.Id));
                        var opexesToEdit = projectOpexes.Where(x => command.Opexes.Any(y => y.Id == x.Id));
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


        public async Task<bool> ApproveProjectAsync(int id, string userEmail)
        {
            var project = await _unitOfWork.ProjectRepository.GetById(id).FirstOrDefaultAsync();
            if (project == null || project.Stage != ProjectStage.Submited)
            {
                return false;
            }
            project.Stage = ProjectStage.Approved;
            project.ApprovalDate = DateTime.Now;
            project.Modified = DateTime.Now;
            project.ModifiedBy = userEmail;
            _unitOfWork.ProjectRepository.Update(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectProjectAsync(int id, string userEmail)
        {
            var project = await _unitOfWork.ProjectRepository.GetById(id).FirstOrDefaultAsync();
            if (project == null || project.Stage != ProjectStage.Submited)
            {
                return false;
            }
            project.Stage = ProjectStage.Rejected;
            project.Modified = DateTime.Now;
            project.ModifiedBy = userEmail;
            _unitOfWork.ProjectRepository.Update(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RollbackProjectToDraftAsync(int id, string userEmail)
        {
            var project = await _unitOfWork.ProjectRepository.GetById(id).FirstOrDefaultAsync();
            if (project == null || project.Stage != ProjectStage.Rejected)
            {
                return false;
            }
            project.Stage = ProjectStage.Draft;
            project.Modified = DateTime.Now;
            project.ModifiedBy = userEmail;
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

        public async Task<byte[]> ExportProjectAsync(int id)
        {
            var project = await _unitOfWork.ProjectRepository.GetById(id)
                .Include(x => x.Capexes)
                .Include(x => x.Ebits)
                .Include(x => x.Opexes)
                .Include(x => x.Country)
                .Include(x => x.ProjectCurrency)
                .AsNoTracking().FirstOrDefaultAsync();

            return project.GenerateProjectExportData();
        }

        public async Task<byte[]> ExportProjectsDataAsync(ProjectQuery query)
        {
            var projects = _unitOfWork.ProjectRepository.GetAll()
                .Include(x => x.ProjectCurrency)
                .Include(x => x.Country)
                .AsNoTracking();

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
                if (query.SortOption.Value == ProjectSortOption.IdDescending)
                {
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

            var projectsToExport = await projects.ProjectTo<ProjectExportDto>(_mapper.ConfigurationProvider).ToListAsync();
            
            return projectsToExport.GetProjectExportData();
        }
    }
}

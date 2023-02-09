using BidProjectsManager.Logic.Result;
using BidProjectsManager.Logic.Services;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;
using BidProjectsManager.Model.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BidProjectsManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ProjectListItemDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedList<ProjectListItemDto>>> Get([FromQuery] ProjectQuery query)
        {
            try
            {
                var projects = await _projectService.GetProjectsAsync(query);
                return Ok(projects);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectDto>> GetById([FromRoute] int id)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(id);
                if (project == null)
                {
                    return NotFound();
                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("draft")]
        [Authorize(Policy = "Editor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDraftProject([FromBody] CreateDraftProjectCommand command)
        {
            try
            {
                var result = await _projectService.CreateDraftProjectAsync(command, GetCurrentUserEmail());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("submit")]
        [Authorize(Policy = "Editor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSubmittedProject([FromBody] CreateSubmittedProjectCommand command)
        {
            try
            {
                var result = await _projectService.CreateSubmittedProjectAsync(command, GetCurrentUserEmail());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("draft")]
        [Authorize(Policy = "Editor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDraftProject([FromBody] UpdateDraftProjectCommand command)
        {
            try
            {
                var result = await _projectService.UpdateProjectDraftAsync(command, GetCurrentUserEmail());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("submit")]
        [Authorize(Policy = "Editor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitProject([FromBody] SubmitProjectCommand command)
        {
            try
            {
                var result = await _projectService.SubmitProjectAsync(command, GetCurrentUserEmail());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("save")]
        [Authorize(Policy = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveProject([FromBody] SaveProjectCommand command)
        {
            try
            {
                var result = await _projectService.SaveProjectAsync(command, GetCurrentUserEmail());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPatch("approve/{id}")]
        [Authorize(Policy = "Reviewer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveProject([FromRoute] int id)
        {
            try
            {
                var result = await _projectService.ApproveProjectAsync(id, GetCurrentUserEmail());
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch("reject/{id}")]
        [Authorize(Policy = "Reviewer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectProject([FromRoute] int id)
        {
            try
            {
                var result = await _projectService.RejectProjectAsync(id, GetCurrentUserEmail());
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch("rollback/{id}")]
        [Authorize(Policy = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RollbackProjectToDraft([FromRoute] int id)
        {
            try
            {
                var result = await _projectService.RollbackProjectToDraftAsync(id, GetCurrentUserEmail());
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            try
            {
                var result = await _projectService.DeleteProjectAsync(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("export")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/octet-stream", Type = typeof(FileContentResult))]
        public async Task<IActionResult> Export([FromQuery] ProjectQuery query)
        {
            try
            {
                var data = await _projectService.ExportProjectsDataAsync(query);
                return File(
                    fileContents: data,
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: $"projects_{DateTime.Now.ToString("G")}.xlsx"
                );
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("export/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/octet-stream", Type = typeof(FileContentResult))]
        public async Task<IActionResult> Export([FromRoute] int id)
        {
            try
            {
                var data = await _projectService.ExportProjectAsync(id);
                return File(
                    fileContents: data,
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: $"project.xlsx"
                );
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private string GetCurrentUserEmail() => User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
    }
}

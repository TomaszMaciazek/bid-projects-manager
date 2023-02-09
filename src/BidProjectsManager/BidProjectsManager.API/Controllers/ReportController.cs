using BidProjectsManager.API.Helpers;
using BidProjectsManager.Logic.Result;
using BidProjectsManager.Logic.Services;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BidProjectsManager.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ReportListItemDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedList<ReportListItemDto>>> GetReports([FromQuery] ReportQuery query)
        {
            try
            {
                var isAdmin = User.HasRole("Administrator");

                return Ok(await _reportService.GetReportsAsync(query, isAdmin));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReportDefinitionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReportDefinitionDto>> GetReportDefinition([FromRoute] int id)
        {
            try
            {
                return Ok(await _reportService.GetReportById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("page/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReportGenerationPageDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReportGenerationPageDto>> GetReportGenerationPageData([FromRoute] int id)
        {
            try
            {
                return Ok(await _reportService.GetReportPageAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[Authorize(Policy = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<bool>> AddReport([FromBody] CreateReportDefinitionCommand command)
        {
            try
            {
                var result = await _reportService.CreateReportDefinitionAsync(command);
                if (!result)
                {
                    return Conflict();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("generate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> GenerateReport([FromBody] GeneratorQuery query)
        {
            try
            {
                var result = await _reportService.GenerateReportAsync(query);
                return File(
                    fileContents: result,
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: $"report_{DateTime.Now.ToString("G")}.xlsx"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Authorize(Policy = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<bool>> UpdateReport([FromBody] UpdateReportDefinitionCommand command)
        {
            try
            {
                var result = await _reportService.UpdateReportDefinitionAsync(command);
                if (!result)
                {
                    return Conflict();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

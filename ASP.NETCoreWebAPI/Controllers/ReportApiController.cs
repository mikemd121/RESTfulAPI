
using Application;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Description;

namespace ASP.NETCoreWebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportApiController : ControllerBase
    {
        IReportService reportService;

        public ReportApiController(IReportService ReportService)
        {
            this.reportService = ReportService;

        }

        /// <summary>
        /// Gets the customer ids.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("reportdata")]
        [ResponseType(typeof(List<ReportModel>))]
        public async Task<IActionResult> GetReportData()
        {
            try
            {
                var model = await reportService.GenerateReport();
                if (model == null)
                    return BadRequest();
                return Ok(model);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }


        }

    }
}

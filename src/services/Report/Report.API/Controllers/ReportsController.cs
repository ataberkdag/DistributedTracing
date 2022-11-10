using Microsoft.AspNetCore.Mvc;

namespace Report.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(ILogger<ReportsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddReport(AddReportRequest request)
        {
            _logger.LogInformation($"{DateTime.Now} | {request.MethodName}");

            return Ok();
        }
    }

    public class AddReportRequest
    {
        public string MethodName { get; set; }
    }
}

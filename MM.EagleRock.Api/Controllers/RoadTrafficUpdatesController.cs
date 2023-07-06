using Microsoft.AspNetCore.Mvc;
using MM.EagleRock.Contract.Models;
using MM.EagleRock.Contract.RoadTraffic;
using System.ComponentModel.DataAnnotations;

namespace MM.EagleRock.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadTrafficUpdatesController : ControllerBase
    {
        private readonly IRoadTrafficOfficer _roadTrafficOfficer;

        private readonly ILogger<RoadTrafficUpdatesController> _logger;

        public RoadTrafficUpdatesController(IRoadTrafficOfficer roadTrafficOfficer, ILoggerFactory loggerFactory)
        {
            _roadTrafficOfficer = roadTrafficOfficer;
            _logger = loggerFactory.CreateLogger<RoadTrafficUpdatesController>();
        }

        [HttpPost]
        public ActionResult ReportRoadTrafficUpdate([FromBody] RoadTrafficUpdatePayload roadTrafficUpdatePayload)
        {
            try
            {
                _roadTrafficOfficer.ProcessTrafficUpdate(roadTrafficUpdatePayload);

                return CreatedAtAction("ReportRoadTrafficUpdate", roadTrafficUpdatePayload.PayloadId);
            }
            catch (ValidationException e) 
            {
                _logger.LogError(e, e.Message);

                return BadRequest(e.Message);
            }
        }

        [Route("deviceStatuses")]
        [HttpGet]
        public ActionResult<IEnumerable<DeviceStatus>> GetDeviceStatuses() 
        {
            var deviceStatuses = _roadTrafficOfficer.GetDevicesSummary();

            return Ok(deviceStatuses);
        }
    }
}

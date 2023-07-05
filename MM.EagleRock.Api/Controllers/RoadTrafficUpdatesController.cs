using Microsoft.AspNetCore.Mvc;
using MM.EagleRock.Contract;
using MM.EagleRock.Contract.Models;
using System.ComponentModel.DataAnnotations;

namespace MM.EagleRock.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadTrafficUpdatesController : ControllerBase
    {
        private readonly IRoadTrafficOfficer _roadTrafficOfficer;

        private readonly ILogger<RoadTrafficUpdatesController> _logger;

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
                return BadRequest(e.Message);
            }
        }

        [HttpGet(Name = "deviceStatuses")]
        public ActionResult<IEnumerable<DeviceStatus>> GetDeviceStatuses() 
        {
            var deviceStatuses = _roadTrafficOfficer.GetDeviceStatuses();

            return Ok(deviceStatuses);
        }
    }
}

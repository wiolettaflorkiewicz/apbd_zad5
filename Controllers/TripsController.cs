using Microsoft.AspNetCore.Mvc;
using Zadanie7.Interfaces;
using Zadanie7.DTO;

namespace Zadanie7.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;

        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            try
            {
                return Ok(await _tripsService.GetTripsAsync());
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [HttpPost("{idTrip:int}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, ClientAssignmentDTO clientAssignmentDTO)
        {
            if (idTrip != clientAssignmentDTO.IdTrip) return BadRequest("IdTrip in URL and body must be the same.");
            try
            {
                return Ok(await _tripsService.AssignClientToTripAsync(clientAssignmentDTO));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Application.DTOs.Common;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly IHallRepository _hallRepository;
        private readonly IScreeningRepository _screeningRepository;

        public HallsController(IHallRepository hallRepository, IScreeningRepository screeningRepository)
        {
            _hallRepository = hallRepository;
            _screeningRepository = screeningRepository;
        }

        // GET: api/halls
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        public async Task<IActionResult> GetAllHalls()
        {
            var halls = await _hallRepository.GetAllHallsAsync();
            return Ok(BaseResponse<object>.Success(halls));
        }

        // GET api/halls/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> GetHallById(int id)
        {
            var hall = await _hallRepository.GetHallByIdAsync(id);
            if (hall == null)
                return NotFound(BaseResponse<object>.Failure("Hall not found"));
            
            return Ok(BaseResponse<object>.Success(hall));
        }

        // GET: api/halls/1/seats
        [HttpGet("{id}/seats")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        public async Task<IActionResult> GetSeatsByHallId(int id)
        {
            var seats = await _hallRepository.GetSeatsByHallIdAsync(id);
            return Ok(BaseResponse<object>.Success(seats));
        }

        // GET: api/halls/1/freeslots/01-01-2001
        [HttpGet("{hallId}/freeslots/{date}")]
        [ProducesResponseType(typeof(BaseResponse<List<string>>), 200)]
        public async Task<IActionResult> GetAvailableTimeSlots(int hallId, DateOnly date)
        {
            var allSlots = new List<TimeOnly> {
                new TimeOnly(11,0,0),
                new TimeOnly(14,0,0),
                new TimeOnly(17,0,0),
                new TimeOnly(20,0,0),
                new TimeOnly(23,0,0)
            };

            var bookedScreenings = await _screeningRepository.GetScreeningsByHallAndDateAsync(hallId, date);

            var bookedTimes = bookedScreenings
                  .Select(s => new TimeOnly(s.Screening_Date_Time.Hour, s.Screening_Date_Time.Minute, 0))
                  .ToList();

            var allowedSlots = allSlots
                    .Where(t => !bookedTimes.Contains(t))
                    .Select(t => t.ToString("HH:mm"))
                    .ToList();

            return Ok(BaseResponse<List<string>>.Success(allowedSlots));
        }
    }
}

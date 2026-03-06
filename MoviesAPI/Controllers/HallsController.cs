using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Responses.Halls;
using AutoMapper;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly IHallRepository _hallRepository;
        private readonly IScreeningRepository _screeningRepository;
        private readonly IMapper _mapper;

        public HallsController(IHallRepository hallRepository, IScreeningRepository screeningRepository, IMapper mapper)
        {
            _hallRepository = hallRepository;
            _screeningRepository = screeningRepository;
            _mapper = mapper;
        }

        // GET: api/halls
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<HallResponse>>), 200)]
        public async Task<IActionResult> GetAllHalls()
        {
            var halls = await _hallRepository.GetAllHallsAsync();
            var response = _mapper.Map<List<HallResponse>>(halls);
            return Ok(BaseResponse<List<HallResponse>>.Success(response));
        }

        // GET api/halls/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<HallResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> GetHallById(Guid id)
        {
            var hall = await _hallRepository.GetHallByIdAsync(id);
            if (hall == null)
                return NotFound(BaseResponse<object>.Failure("Hall not found"));
            
            var response = _mapper.Map<HallResponse>(hall);
            return Ok(BaseResponse<HallResponse>.Success(response));
        }

        // GET: api/halls/1/seats
        [HttpGet("{id}/seats")]
        [ProducesResponseType(typeof(BaseResponse<List<HallSeatResponse>>), 200)]
        public async Task<IActionResult> GetSeatsByHallId(Guid id)
        {
            var seats = await _hallRepository.GetSeatsByHallIdAsync(id);
            var response = _mapper.Map<List<HallSeatResponse>>(seats);
            return Ok(BaseResponse<List<HallSeatResponse>>.Success(response));
        }

        // GET: api/halls/1/freeslots/01-01-2001
        [HttpGet("{hallId}/freeslots/{date}")]
        [ProducesResponseType(typeof(BaseResponse<List<string>>), 200)]
        public async Task<IActionResult> GetAvailableTimeSlots(Guid hallId, DateOnly date)
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
                  .Select(s => new TimeOnly(s.ScreeningDateTime.Hour, s.ScreeningDateTime.Minute, 0))
                  .ToList();

            var allowedSlots = allSlots
                    .Where(t => !bookedTimes.Contains(t))
                    .Select(t => t.ToString("HH:mm"))
                    .ToList();

            return Ok(BaseResponse<List<string>>.Success(allowedSlots));
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Models.System;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Screenings;
using AutoMapper;
using FluentValidation;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningsController : ControllerBase
    {
        private readonly IScreeningRepository _screeningRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateScreeningRequest> _createValidator;
        private readonly IValidator<UpdateScreeningRequest> _updateValidator;

        public ScreeningsController(
            IScreeningRepository screeningRepository,
            IMapper mapper,
            IValidator<CreateScreeningRequest> createValidator,
            IValidator<UpdateScreeningRequest> updateValidator)
        {
            _screeningRepository = screeningRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }


        // GET: api/screenings
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ScreeningResponse>>), 200)]
        public async Task<ActionResult<BaseResponse<List<ScreeningResponse>>>> Get()
        {
            var screenings = await _screeningRepository.GetScreeningsAsync();
            return Ok(BaseResponse<List<ScreeningResponse>>.Success(screenings));
        }

        // GET api/screenings/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ScreeningResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<ScreeningResponse>>> Get(long id)
        {
            var screening = await _screeningRepository.GetScreeningAsync(id);

            if (screening == null)
                return NotFound(BaseResponse<object>.Failure("Screening not found"));

            return Ok(BaseResponse<ScreeningResponse>.Success(screening));
        }

        // POST api/screenings
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<long>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<long>>> Post([FromBody] CreateScreeningRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var screening = new CreateScreening
            {
                Movie_Id = request.MovieId,
                Hall_Id = request.HallId,
                Screening_Date_Time = request.ScreeningDateTime
            };

            var id = await _screeningRepository.CreateScreeningAsync(screening);
            return CreatedAtAction(nameof(Get), new { id }, BaseResponse<long>.Success(id, "Screening created successfully"));
        }

        // PUT api/screenings/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<bool>>> Put(int id, [FromBody] UpdateScreeningRequest request)
        {
            var existing = await _screeningRepository.GetScreeningForUpdateAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Screening not found"));

            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            if (request.AvailableTickets > request.TotalTickets)
                return BadRequest(BaseResponse<object>.Failure("Available tickets cannot be greater than total tickets"));

            var screening = new UpdateScreening
            {
                Movie_Id = request.MovieId,
                Screening_Date_Time = request.ScreeningDateTime,
                Total_Tickets = request.TotalTickets,
                Available_Tickets = request.AvailableTickets
            };

            var result = await _screeningRepository.UpdateScreeningAsync(id, screening);
            return Ok(BaseResponse<int>.Success(result, "Screening updated successfully"));
        }

        // DELETE api/screenings/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _screeningRepository.GetScreeningAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Screening not found"));

            var result = await _screeningRepository.DeleteScreeningAsync(id);
            return Ok(BaseResponse<int>.Success(result, "Screening deleted successfully"));
        }

        // GET: api/screenings/1/reservedseats
        [HttpGet("{id}/reservedseats")]
        [ProducesResponseType(typeof(BaseResponse<List<SeatForScreeningDto>>), 200)]
        public async Task<IActionResult> GetReservedSeatsForScreening(int id)
        {
            var reservedSeats = await _screeningRepository.GetReservedSeatsAsync(id);

            if (reservedSeats == null || !reservedSeats.Any())
                reservedSeats = new List<SeatForScreeningDto>();

            return Ok(BaseResponse<List<SeatForScreeningDto>>.Success(reservedSeats));
        }

        // POST: api/screenings/5/book
        [HttpPost("{id}/book")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> BookSeats(int id, [FromBody] BookSeatsRequest request)
        {
            if (request.SelectedSeatsId == null || !request.SelectedSeatsId.Any())
                return BadRequest(BaseResponse<object>.Failure("No seats selected"));

            var screening = await _screeningRepository.GetScreeningAsync(id);

            if (screening == null)
                return NotFound(BaseResponse<object>.Failure("Screening not found"));

            try
            {
                await _screeningRepository.BookSeatsAsync(id, request.username, request.SelectedSeatsId);
                return Ok(BaseResponse<object>.Success(new { Message = "Seats successfully booked!" }, "Seats successfully booked!"));
            }
            catch (Exception ex)
            {
                return BadRequest(BaseResponse<object>.Failure(ex.Message));
            }
        }
    }
}

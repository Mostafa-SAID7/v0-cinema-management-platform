using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Domain.Entities.Screenings;
using MoviesAPI.Domain.Entities.Halls;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Screenings;
using MoviesAPI.Application.DTOs.Responses.Screenings;
using MoviesAPI.Data;
using AutoMapper;
using FluentValidation;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateScreeningRequest> _createValidator;
        private readonly IValidator<UpdateScreeningRequest> _updateValidator;

        public ScreeningsController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateScreeningRequest> createValidator,
            IValidator<UpdateScreeningRequest> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }


        // GET: api/screenings
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ScreeningResponse>>), 200)]
        public async Task<ActionResult<BaseResponse<List<ScreeningResponse>>>> Get()
        {
            var screenings = await _unitOfWork.Screenings.GetScreeningsAsync();
            var response = _mapper.Map<List<ScreeningResponse>>(screenings);
            return Ok(BaseResponse<List<ScreeningResponse>>.Success(response));
        }

        // GET api/screenings/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ScreeningResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<ScreeningResponse>>> Get(Guid id)
        {
            var screening = await _unitOfWork.Screenings.GetScreeningAsync(id);

            if (screening == null)
                return NotFound(BaseResponse<object>.Failure("Screening not found"));

            var response = _mapper.Map<ScreeningResponse>(screening);
            return Ok(BaseResponse<ScreeningResponse>.Success(response));
        }

        // POST api/screenings
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<Guid>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<Guid>>> Post([FromBody] CreateScreeningRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var screening = _mapper.Map<Screening>(request);
            var created = await _unitOfWork.Screenings.CreateScreeningAsync(screening);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = created.Id }, BaseResponse<Guid>.Success(created.Id, "Screening created successfully"));
        }

        // PUT api/screenings/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<object>>> Put(Guid id, [FromBody] UpdateScreeningRequest request)
        {
            var existing = await _unitOfWork.Screenings.GetScreeningAsync(id);
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

            _mapper.Map(request, existing);
            await _unitOfWork.Screenings.UpdateScreeningAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<object>.Success(null, "Screening updated successfully"));
        }

        // DELETE api/screenings/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _unitOfWork.Screenings.GetScreeningAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Screening not found"));

            await _unitOfWork.Screenings.DeleteScreeningAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<object>.Success(null, "Screening deleted successfully"));
        }

        // GET: api/screenings/1/reservedseats
        [HttpGet("{id}/reservedseats")]
        [ProducesResponseType(typeof(BaseResponse<List<SeatInfo>>), 200)]
        public async Task<IActionResult> GetReservedSeatsForScreening(Guid id)
        {
            var reservedSeats = await _unitOfWork.Screenings.GetReservedSeatsAsync(id);
            var response = _mapper.Map<List<SeatInfo>>(reservedSeats);

            return Ok(BaseResponse<List<SeatInfo>>.Success(response));
        }

        // POST: api/screenings/5/book
        [HttpPost("{id}/book")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> BookSeats(Guid id, [FromBody] BookSeatsRequest request)
        {
            if (request.SelectedSeatsId == null || !request.SelectedSeatsId.Any())
                return BadRequest(BaseResponse<object>.Failure("No seats selected"));

            var screening = await _unitOfWork.Screenings.GetScreeningAsync(id);

            if (screening == null)
                return NotFound(BaseResponse<object>.Failure("Screening not found"));

            try
            {
                // Parse username as Guid (assuming it's a user ID)
                if (!Guid.TryParse(request.username, out Guid userId))
                    return BadRequest(BaseResponse<object>.Failure("Invalid user ID"));

                var seatIds = request.SelectedSeatsId.Select(s => Guid.Parse(s.ToString())).ToList();
                await _unitOfWork.Screenings.BookSeatsAsync(id, userId, seatIds);
                await _unitOfWork.SaveChangesAsync();

                return Ok(BaseResponse<object>.Success(new { Message = "Seats successfully booked!" }, "Seats successfully booked!"));
            }
            catch (Exception ex)
            {
                return BadRequest(BaseResponse<object>.Failure(ex.Message));
            }
        }
    }
}


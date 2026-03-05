using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Tickets;
using AutoMapper;
using FluentValidation;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTicketRequest> _createValidator;

        public TicketsController(
            ITicketRepository ticketRepository,
            IMapper mapper,
            IValidator<CreateTicketRequest> createValidator)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _createValidator = createValidator;
        }


        // GET: api/tickets
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<TicketResponse>>), 200)]
        public async Task<ActionResult<BaseResponse<List<TicketResponse>>>> Get()
        {
            var tickets = await _ticketRepository.GetTicketsAsync();
            return Ok(BaseResponse<List<TicketResponse>>.Success(tickets));
        }

        // GET api/tickets/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<Ticket>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<Ticket>>> Get(long id)
        {
            var ticket = await _ticketRepository.GetTicketAsync(id);

            if (ticket == null)
                return NotFound(BaseResponse<object>.Failure("Ticket not found"));

            return Ok(BaseResponse<Ticket>.Success(ticket));
        }

        // POST api/tickets
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<long>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<long>>> Post([FromBody] CreateTicketRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var ticket = new CreateTicket
            {
                Movie_Id = request.MovieId,
                User_Id = request.UserId,
                Watch_Movie = request.WatchDateTime,
                Price = request.Price,
                hall_seat_id = request.HallSeatId
            };

            var id = await _ticketRepository.CreateTicketAsync(ticket);
            return CreatedAtAction(nameof(Get), new { id }, BaseResponse<long>.Success(id, "Ticket created successfully"));
        }

        // DELETE api/tickets/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _ticketRepository.GetTicketAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Ticket not found"));

            var result = await _ticketRepository.DeleteTicketAsync(id);
            return Ok(BaseResponse<bool>.Success(result, "Ticket deleted successfully"));
        }
    }
}

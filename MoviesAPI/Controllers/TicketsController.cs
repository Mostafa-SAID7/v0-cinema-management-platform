using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Domain.Entities.Tickets;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Tickets;
using MoviesAPI.Application.DTOs.Responses.Tickets;
using MoviesAPI.Data;
using AutoMapper;
using FluentValidation;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTicketRequest> _createValidator;

        public TicketsController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateTicketRequest> createValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
        }


        // GET: api/tickets
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<TicketResponse>>), 200)]
        public async Task<ActionResult<BaseResponse<List<TicketResponse>>>> Get()
        {
            var tickets = await _unitOfWork.Tickets.GetTicketsAsync();
            var response = _mapper.Map<List<TicketResponse>>(tickets);
            return Ok(BaseResponse<List<TicketResponse>>.Success(response));
        }

        // GET api/tickets/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<TicketDetailsResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<TicketDetailsResponse>>> Get(Guid id)
        {
            var ticket = await _unitOfWork.Tickets.GetTicketAsync(id);

            if (ticket == null)
                return NotFound(BaseResponse<object>.Failure("Ticket not found"));

            var response = _mapper.Map<TicketDetailsResponse>(ticket);
            return Ok(BaseResponse<TicketDetailsResponse>.Success(response));
        }

        // POST api/tickets
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<Guid>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<Guid>>> Post([FromBody] CreateTicketRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var ticket = _mapper.Map<Ticket>(request);
            var created = await _unitOfWork.Tickets.CreateTicketAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = created.Id }, BaseResponse<Guid>.Success(created.Id, "Ticket created successfully"));
        }

        // DELETE api/tickets/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _unitOfWork.Tickets.GetTicketAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Ticket not found"));

            await _unitOfWork.Tickets.DeleteTicketAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<object>.Success(null, "Ticket deleted successfully"));
        }
    }
}


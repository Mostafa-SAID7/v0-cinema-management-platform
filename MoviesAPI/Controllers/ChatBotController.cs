using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Domain.Entities.Faqs;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Service.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.ChatBot;
using MoviesAPI.Application.DTOs.Responses.Faqs;
using MoviesAPI.Data;
using AutoMapper;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotRepository _chatbotRepository;
        private readonly IOpenAIService _openAIService;
        private readonly IChatBotRagService _chatBotRagService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChatBotController(
            IChatBotRepository chatbotRepository,
            IOpenAIService openAIService,
            IChatBotRagService chatBotRagService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _chatbotRepository = chatbotRepository;
            _openAIService = openAIService;
            _chatBotRagService = chatBotRagService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/chatbot/faqs
        [HttpGet("faqs")]
        [ProducesResponseType(typeof(BaseResponse<List<FaqResponse>>), 200)]
        public async Task<ActionResult<BaseResponse<List<FaqResponse>>>> GetAllFaq()
        {
            var faqs = await _chatbotRepository.GetAllFaqAsync();
            var response = _mapper.Map<List<FaqResponse>>(faqs);
            return Ok(BaseResponse<List<FaqResponse>>.Success(response));
        }

        // GET: api/chatbot/faqs/{id}
        [HttpGet("faqs/{id}")]
        [ProducesResponseType(typeof(BaseResponse<FaqResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<FaqResponse>>> GetFaqById(Guid id)
        {
            var faq = await _chatbotRepository.GetFaqByIdAsync(id);
            if (faq == null)
                return NotFound(BaseResponse<object>.Failure("FAQ not found"));
            
            var response = _mapper.Map<FaqResponse>(faq);
            return Ok(BaseResponse<FaqResponse>.Success(response));
        }

        // POST: api/chatbot/faq
        [HttpPost("faq")]
        [ProducesResponseType(typeof(BaseResponse<FaqResponse>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<FaqResponse>>> CreateFaq(Faq faq)
        {
            if (!ModelState.IsValid)
                return BadRequest(BaseResponse<object>.Failure(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            await _chatbotRepository.AddFaqAsync(faq);
            await _unitOfWork.SaveChangesAsync();
            
            var response = _mapper.Map<FaqResponse>(faq);
            return CreatedAtAction(nameof(GetFaqById), new { id = faq.Id }, BaseResponse<FaqResponse>.Success(response, "FAQ created successfully"));
        }

        // PUT: api/chatbot/faq/{id}
        [HttpPut("faq/{id}")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<object>>> Update(Guid id, Faq faq)
        {
            if (id != faq.Id)
                return BadRequest(BaseResponse<object>.Failure("ID mismatch"));
            
            var existing = await _chatbotRepository.GetFaqByIdAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("FAQ not found"));

            await _chatbotRepository.UpdateFaqAsync(faq);
            await _unitOfWork.SaveChangesAsync();
            
            return Ok(BaseResponse<object>.Success(null, "FAQ updated successfully"));
        }

        // DELETE: api/chatbot/faq/{id}
        [HttpDelete("faq/{id}")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<object>>> Delete(Guid id)
        {
            var faq = await _chatbotRepository.GetFaqByIdAsync(id);
            if (faq == null)
                return NotFound(BaseResponse<object>.Failure("FAQ not found"));

            await _chatbotRepository.DeleteFaqAsync(faq);
            await _unitOfWork.SaveChangesAsync();
            
            return Ok(BaseResponse<object>.Success(null, "FAQ deleted successfully"));
        }

        // POST: api/chatbot/ask
        [HttpPost("ask")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<object>>> Ask([FromBody] UserQuestionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest(BaseResponse<object>.Failure("Please ask a question"));

            var answer = await _chatBotRagService.AskQuestionAsync(request.Question);
            return Ok(BaseResponse<object>.Success(new { answer }, "Question answered successfully"));
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Service.Interface;
using MoviesAPI.Application.DTOs.Common;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotRepository _chatbotRepository;
        private readonly IOpenAIService _openAIService;
        private readonly IChatBotRagService _chatBotRagService;


        public ChatBotController(IChatBotRepository chatbotRepository,IOpenAIService openAIService,IChatBotRagService chatBotRagService)
        {
            _chatbotRepository = chatbotRepository;
            _openAIService = openAIService;
            _chatBotRagService = chatBotRagService;
        }

        // GET: api/chatbot/faqs
        [HttpGet("faqs")]
        [ProducesResponseType(typeof(BaseResponse<List<Faq>>), 200)]
        public async Task<ActionResult<BaseResponse<List<Faq>>>> GetAllFaq()
        {
            var faqs = await _chatbotRepository.GetAllFaqAsync();
            return Ok(BaseResponse<List<Faq>>.Success(faqs));
        }

        // GET: api/chatbot/faqs/{id}
        [HttpGet("faqs/{id}")]
        [ProducesResponseType(typeof(BaseResponse<Faq>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<Faq>>> GetFaqById(int id)
        {
            var faq = await _chatbotRepository.GetFaqByIdAsync(id);
            if (faq == null)
                return NotFound(BaseResponse<object>.Failure("FAQ not found"));
            
            return Ok(BaseResponse<Faq>.Success(faq));
        }

        // POST: api/chatbot/faq
        [HttpPost("faq")]
        [ProducesResponseType(typeof(BaseResponse<Faq>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<Faq>>> CreateFaq(Faq faq)
        {
            if (!ModelState.IsValid)
                return BadRequest(BaseResponse<object>.Failure(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            await _chatbotRepository.AddFaqAsync(faq);
            return CreatedAtAction(nameof(GetFaqById), new { id = faq.Id }, BaseResponse<Faq>.Success(faq, "FAQ created successfully"));
        }

        // PUT: api/chatbot/faq/{id}
        [HttpPut("faq/{id}")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<object>>> Update(int id, Faq faq)
        {
            if (id != faq.Id)
                return BadRequest(BaseResponse<object>.Failure("ID mismatch"));
            
            var existing = await _chatbotRepository.GetFaqByIdAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("FAQ not found"));

            await _chatbotRepository.UpdateFaqAsync(faq);
            return Ok(BaseResponse<object>.Success(null, "FAQ updated successfully"));
        }

        // DELETE: api/chatbot/faq/{id}
        [HttpDelete("faq/{id}")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<object>>> Delete(int id)
        {
            var existing = await _chatbotRepository.GetFaqByIdAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("FAQ not found"));

            await _chatbotRepository.DeleteFaqAsync(id);
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

        public class UserQuestionRequest
        {
            public string Question { get; set; }
        }
    }
}
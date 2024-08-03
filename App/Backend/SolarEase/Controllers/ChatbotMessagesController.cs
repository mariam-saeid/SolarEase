using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Models.DTO.ChatbotMessageDto;
using SolarEase.Repositories;
using SolarEase.Services;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotMessagesController : ControllerBase
    {
        private readonly IChatbotMessageRepository chatbotMessageRepository;
        private readonly IPersonRepository personRepository;
        private readonly IFAQRepository fAQRepository;
        private readonly HttpClient _httpClient;

        public ChatbotMessagesController(IChatbotMessageRepository chatbotMessageRepository,IPersonRepository personRepository, IFAQRepository fAQRepository) {
            this.chatbotMessageRepository = chatbotMessageRepository;
            this.personRepository = personRepository;
            this.fAQRepository = fAQRepository;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://solareasegp-chatbot.hf.space"); // Update the URL accordingly
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chatbotMessages = await chatbotMessageRepository.GetAllAsync();
            var chatbotMessagesDto = chatbotMessages.ChatbotMessagesInfo();
            return Ok(chatbotMessagesDto);
        }

        [HttpGet]
        [Authorize]
        [Route("ByPerson/{personId:int}")]
        public async Task<IActionResult> GetAllByPerson([FromRoute] int personId)
        {
            var chatbotMessages = await chatbotMessageRepository.GetAllByPersonAsync(personId);
            if (chatbotMessages == null)
                return NotFound("person not found");

            var chatbotMessagesDto = chatbotMessages.ChatbotMessagesInfo();
            return Ok(chatbotMessagesDto);
        }

        [HttpPost]
        [Authorize]
        [Route("Answer/{personId:int}")]
        public async Task<IActionResult> GetAnswer([FromRoute] int personId, [FromBody] CreateChatbotMessageDto createChatbotMessageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }

            try
            {
                // Make GET request to FastAPI endpoint
                var response = await _httpClient.GetAsync($"/query/?query={createChatbotMessageDto.Question}");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize JSON response
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var responseContent = await JsonSerializer.DeserializeAsync<JsonElement>(responseStream);

                    // Access best match information
                    var bestMatch = responseContent.GetProperty("best_match").GetString();
                    var bestMatchText = responseContent.GetProperty("best_match_text").GetString();

                    var chatbotMessage = new ChatbotMessage
                    {
                        PersonId = personId,
                        Question = createChatbotMessageDto.Question,
                        Date = DateTime.Now
                    };

                    if (bestMatch == "Not found")
                    {
                        chatbotMessage.Answer = "I'm sorry, I don't know the answer to that question.";
                    }
                    else
                    {
                        chatbotMessage.Answer = bestMatchText;
                    }

                    chatbotMessage = await chatbotMessageRepository.CreateAsync(chatbotMessage);

                    var chatbotMessageDto = chatbotMessage.ChatbotMessageInfo();

                    return Ok(chatbotMessageDto);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        
        [HttpPost]
        [Authorize]
        [Route("Create/{personId:int}/{fAQId:int}")]
        public async Task<IActionResult> Create([FromRoute] int personId, [FromRoute] int fAQId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var fAQ = await fAQRepository.GetByIdAsync(fAQId);
            if (fAQ == null)
            {
                return NotFound("fAQ not found");
            }

            var chatbotMessage = new ChatbotMessage
            {
                PersonId = personId,
                Answer = fAQ.Answer,
                Question = fAQ.Question
            };
            chatbotMessage.Date = DateTime.Now;

            chatbotMessage = await chatbotMessageRepository.CreateAsync(chatbotMessage);

            var chatbotMessageDto = chatbotMessage.ChatbotMessageInfo();

            return Ok(chatbotMessageDto);
        }

        [HttpDelete]
        [Route("{personId:int}")]
        public async Task<IActionResult> DeleteAll([FromRoute] int personId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var chatbotMessages = await chatbotMessageRepository.DeleteAllByPersonAsync(personId);

            return Ok();
        }

    }
}

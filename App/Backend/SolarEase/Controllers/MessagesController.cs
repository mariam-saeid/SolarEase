using AutoMapper;
using SolarEase.Services;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using SolarEase.Models.DTO.PeakHourDto;
using SolarEase.Models.DTO.MessageDto;
using SolarEase.Models.DTO.FavoritePostDto;

namespace SolarEase.wwwroot.SolarProductImages
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly FileService fileService;
        private readonly IMessageRepository messageRepository;
        private readonly IPersonRepository personRepository;

        public MessagesController(IMapper mapper, FileService fileService, IMessageRepository messageRepository,
            IPersonRepository personRepository)
        {
            this.mapper = mapper;
            this.fileService = fileService;
            this.messageRepository = messageRepository;
            this.personRepository = personRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await messageRepository.GetAllAsync();
            var messagesDto = messages.MessagesInfo();
            return Ok(messagesDto);
        }

        [HttpGet]
        [Authorize]
        [Route("ByPerson/{personId:int}")]
        public async Task<IActionResult> GetAllByPerson([FromRoute] int personId)
        {
            var messages = await messageRepository.GetAllByPersonAsync(personId);
            if (messages == null)
                return Ok("person not found");
            var messagesDto = messages.MessagesInfo();
            return Ok(messagesDto);
        }

        //**************************************************************************************************************
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var message = await messageRepository.GetByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            var messageDto = message.MessageInfo();
            return Ok(messageDto);
        }

        //**************************************************************************************************************
        [HttpPost]
        [Route("{personId:int}")]
        public async Task<IActionResult> Create([FromRoute] int personId, [FromForm] CreateMessageDto createMessageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }

            var message = mapper.Map<Message>(createMessageDto);

            message.SentDate = DateTime.Now;
            message.PersonId = personId;

            message = await messageRepository.CreateAsync(message);

            var messageDto = message.MessageInfo();
            return Ok(messageDto);
        }

        //**************************************************************************************************************
        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CreateMessageDto updateMessageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = mapper.Map<Message>(updateMessageDto);
            message = await messageRepository.UpdateAsync(id, message);
            if (message == null)
            {
                return NotFound();
            }
            var messageDto = message.MessageInfo();
            return Ok(messageDto);
        }

        //**************************************************************************************************************
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var message = await messageRepository.DeleteAsync(id);

            if (message == null) { return NotFound(); }

            return Ok("message Deleted Successfully");
        }
    }
}


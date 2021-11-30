using AutoMapper;
using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Helpers;
using DatingApp.API.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;

        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.messageRepository = messageRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var userName = User.GetUserName();
            if (userName == createMessageDto.RecipientUserName.ToLower())
            {
                return BadRequest("You cannot send message to your self");
            }
            var sender = await userRepository.GetUserByUserNameAsync(userName);
            var recipient = await userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);
            if (recipient == null) return NotFound();
            var message = new Message()
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content,
            };
            messageRepository.AddMessage(message);

            if (await messageRepository.SaveAllAsync())
                return Ok(this.mapper.Map<MessageDto>(message));
            return BadRequest("Failed to send message");
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUserName();
            var messages = await this.messageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize,
                messages.TotalCount, messages.TotalPages);
            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string userName)
        {
            var currentUserName = User.GetUserName();
            var retVal = await this.messageRepository.GetMessageThread(currentUserName, userName);
            return Ok(retVal);
        }


    }
}

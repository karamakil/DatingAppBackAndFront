using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Helpers;
using DatingApp.API.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public MessageRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            this.dataContext.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            this.dataContext.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int Id)
        {
            return await this.dataContext.Messages.FindAsync(Id);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = this.dataContext.Messages
                .OrderByDescending(m => m.MessageSent).AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.UserName),
                "OutBox" => query.Where(u => u.Sender.UserName == messageParams.UserName),
                _ => query.Where(u => u.Recipient.UserName == messageParams.UserName && u.DateRead == null),
            };

            var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize); ;
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var messages = this.dataContext.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => m.Recipient.UserName == currentUserName &&
                m.Sender.UserName == recipientUserName
                || m.Recipient.UserName == recipientUserName
                && m.Sender.UserName == currentUserName)
                .OrderBy(z => z.MessageSent).ToList();
            var unreadMessages = messages.Where(m => m.DateRead == null && m.Recipient.UserName == currentUserName).ToList();

            if (unreadMessages.Any())
            {
                foreach (var msg in unreadMessages)
                {
                    msg.DateRead = DateTime.Now;
                }
                await this.dataContext.SaveChangesAsync();
            }
            var retVal = this.mapper.Map<IEnumerable<MessageDto>>(messages);
            return retVal;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await this.dataContext.SaveChangesAsync() > 0;
        }
    }
}

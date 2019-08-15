﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IMessageSqlDAL
    {
        void CreateMessage(Message message);

        Dictionary<string, Message> GetMessagesByUsername(User user);

        List<Message> GetConversation(string SenderUsername, string receiverUsername);

        void UpdateToRead(List<Message> ListOfMessages, int CurrentUserId);

        List<Message> GetCurrentUsersMessages(User user);
    }
}

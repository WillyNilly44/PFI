using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class ChatRepository : Repository<Chat>
    {
        public override bool Update(Chat chat)
        {
            BeginTransaction();
            var result = base.Update(chat);
            EndTransaction();
            return result;
        }
        public override bool Delete(int Id)
        {
            Chat chat = DB.Chat.Get(Id);
            if (chat != null)
            {
                BeginTransaction();
                var result = base.Delete(Id);
                EndTransaction();
                return result;
            }
            return false;
        }

        public Chat GetChat(int message_id)
        {
            Chat chat = ToList().Where(u => (u.Id == message_id))
                                .FirstOrDefault();
            return chat;
        }
    }
}
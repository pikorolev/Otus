using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkFor5Lesson
{
    internal class ToDoUser
    {
        public Guid UserId { get; }
        public long TelegramUserId { get; }
        public string TelegramUserName { get; }
        public DateTime RegisteredAtGuid { get; }

        public ToDoUser(long telegramUserId, string telegramUserName)
        {
            UserId = Guid.NewGuid();
            TelegramUserId = telegramUserId;
            TelegramUserName = telegramUserName;
            RegisteredAtGuid = DateTime.Now;
        }
    }
}

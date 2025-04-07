using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkFor5Lesson
{
    internal class UserService : IUserService
    {
        // Список зарегестрированных пользователлей
        List<ToDoUser> users = new List<ToDoUser>();
        // Регистирует нового пользователя
        public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
        {
            var user = new ToDoUser(telegramUserId, telegramUserName);
            users.Add(user);
            return user;
        }
        // Возвращает пользователя по telegramUserId
        public ToDoUser? GetUser(long telegramUserId)
        {
            foreach (var user in users)
            {
                if (user.TelegramUserId == telegramUserId)
                    return user;
            }
            return null;
        }
    }
}

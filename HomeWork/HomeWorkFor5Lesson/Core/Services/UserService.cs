using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeWorkFor5Lesson.Core.DataAccess;
using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Core.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        // Регистирует нового пользователя
        public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
        {
            var user = new ToDoUser(telegramUserId, telegramUserName);
            userRepository.Add(user);
            return user;
        }
        // Возвращает пользователя по telegramUserId
        public ToDoUser? GetUser(long telegramUserId)
        {
            return userRepository.GetUserByTelegramUserId(telegramUserId);
        }
    }
}

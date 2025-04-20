using HomeWorkFor5Lesson.Core.DataAccess;
using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Infrastructure.DataAccess
{
    internal class InMemoryUserRepository : IUserRepository
    {
        private readonly List<ToDoUser> users = new List<ToDoUser>();
        public ToDoUser? GetUser(Guid userId)
        {
            foreach (var user in users)
            {
                if (user.UserId == userId)
                    return user;
            }
            return null;
        }
        public ToDoUser? GetUserByTelegramUserId(long telegramUserId)
        {
            foreach (var user in users)
            {
                if (user.TelegramUserId == telegramUserId)
                    return user;
            }
            return null;
        }
        public void Add(ToDoUser user)
        {
            users.Add(user);
        }
    }
}

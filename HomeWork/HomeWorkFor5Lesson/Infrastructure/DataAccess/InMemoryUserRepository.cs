using HomeWorkFor5Lesson.Core.DataAccess;
using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Infrastructure.DataAccess
{
    internal class InMemoryUserRepository : IUserRepository
    {
        private readonly List<ToDoUser> users = new List<ToDoUser>();
        public Task<ToDoUser?> GetUser(Guid userId, CancellationToken ct)
        {
            foreach (var user in users)
            {
                if (user.UserId == userId)
                    return Task.FromResult(user);
            }
            return null;
        }
        public Task<ToDoUser?> GetUserByTelegramUserId(long telegramUserId, CancellationToken ct)
        {
            foreach (var user in users)
            {
                if (user.TelegramUserId == telegramUserId)
                    return Task.FromResult(user);
            }
            return null;
        }
        public Task Add(ToDoUser user, CancellationToken ct)
        {
            users.Add(user);
            return Task.CompletedTask;
        }
    }
}

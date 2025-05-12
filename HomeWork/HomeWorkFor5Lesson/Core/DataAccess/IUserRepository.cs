using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Core.DataAccess
{
    interface IUserRepository
    {
        Task<ToDoUser?> GetUser(Guid userId, CancellationToken ct);
        Task<ToDoUser?> GetUserByTelegramUserId(long telegramUserId, CancellationToken ct);
        Task Add(ToDoUser user, CancellationToken ct);
    }
}

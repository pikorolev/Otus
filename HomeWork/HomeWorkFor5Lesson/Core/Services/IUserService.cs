using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Core.Services
{
    internal interface IUserService
    {
        Task<ToDoUser> RegisterUser(long telegramUserId, string telegramUserName, CancellationToken ct);
        Task<ToDoUser?> GetUser(long telegramUserId, CancellationToken ct);
    }
}

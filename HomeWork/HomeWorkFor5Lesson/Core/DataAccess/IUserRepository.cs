using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Core.DataAccess
{
    interface IUserRepository
    {
        ToDoUser? GetUser(Guid userId);
        ToDoUser? GetUserByTelegramUserId(long telegramUserId);
        void Add(ToDoUser user);
    }
}

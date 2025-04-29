using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Core.Services
{
    internal interface IUserService
    {
        ToDoUser RegisterUser(long telegramUserId, string telegramUserName);
        ToDoUser? GetUser(long telegramUserId);
    }
}

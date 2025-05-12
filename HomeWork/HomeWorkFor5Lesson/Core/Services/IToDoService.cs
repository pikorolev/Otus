using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Core.Services
{
    internal interface IToDoService
    {
        //Возвращает ToDoItem для UserId со статусом Active
        Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId);
        Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId);
        Task<ToDoItem> Add(ToDoUser user, string name);
        Task MarkCompleted(Guid id);
        Task Delete(Guid id);
        Task<IReadOnlyList<ToDoItem>> Find(ToDoUser user, string namePrefix);
    }
}

using HomeWorkFor5Lesson.Core.Entities;
using System.Collections.Generic;

namespace HomeWorkFor5Lesson.Core.DataAccess
{
    interface IToDoRepository
    {
        Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId, CancellationToken ct);
        //Возвращает ToDoItem для UserId со статусом Active
        Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId, CancellationToken ct);
        Task Add(ToDoItem item, CancellationToken ct);
        Task Update(ToDoItem item, CancellationToken ct);
        Task Delete(Guid id, CancellationToken ct);
        //Проверяет есть ли задача с таким именем у пользователя
        bool ExistsByName(Guid userId, string name);
        //Возвращает количество активных задач у пользователя
        int CountActive(Guid userId);
        Task<ToDoItem> GetToDoItemById(Guid id, CancellationToken ct);
        Task<IReadOnlyList<ToDoItem>> Find(Guid userId, Func<ToDoItem, bool> predicate, CancellationToken ct);
    }
}

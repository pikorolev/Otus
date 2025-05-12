using HomeWorkFor5Lesson.Core.Entities;
using System.Collections.Generic;

namespace HomeWorkFor5Lesson.Core.DataAccess
{
    interface IToDoRepository
    {
        Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId);
        //Возвращает ToDoItem для UserId со статусом Active
        Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId);
        Task Add(ToDoItem item);
        Task Update(ToDoItem item);
        Task Delete(Guid id);
        //Проверяет есть ли задача с таким именем у пользователя
        bool ExistsByName(Guid userId, string name);
        //Возвращает количество активных задач у пользователя
        int CountActive(Guid userId);
        Task<ToDoItem> GetToDoItemById(Guid id);
        Task<IReadOnlyList<ToDoItem>> Find(Guid userId, Func<ToDoItem, bool> predicate);
    }
}

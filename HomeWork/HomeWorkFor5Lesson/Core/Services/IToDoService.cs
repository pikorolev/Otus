using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Core.Services
{
    internal interface IToDoService
    {
        //Возвращает ToDoItem для UserId со статусом Active
        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);
        ToDoItem Add(ToDoUser user, string name);
        void MarkCompleted(Guid id);
        void Delete(Guid id);
        IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix);
    }
}

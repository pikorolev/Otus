using System.Diagnostics;
using System.Threading.Tasks;
using HomeWorkFor5Lesson.Core.DataAccess;
using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Infrastructure.DataAccess
{
    internal class InMemoryToDoRepository : IToDoRepository
    {
        private readonly List<ToDoItem> toDoItems = new List<ToDoItem>();

        public Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId, CancellationToken ct)
        {
            List<ToDoItem> tempList = new List<ToDoItem>();
            foreach (var item in toDoItems)
            {
                if (item.User.UserId == userId)
                    tempList.Add(item);
            }
            return Task.FromResult((IReadOnlyList<ToDoItem>)tempList);
        }
        //Возвращает ToDoItem для UserId со статусом Active
        public Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId, CancellationToken ct)
        {
            List<ToDoItem> tempList = new List<ToDoItem>();
            foreach (var item in toDoItems)
            {
                if (item.User.UserId == userId && item.State == ToDoItemState.Active)
                    tempList.Add(item);
            }
            return Task.FromResult((IReadOnlyList<ToDoItem>)tempList);
        }
        public Task Add(ToDoItem item, CancellationToken ct)
        {
            toDoItems.Add(item);
            return Task.CompletedTask;
        }
        public Task Update(ToDoItem item, CancellationToken ct)
        {
            for (int i = 0; i < toDoItems.Count; i++)
            {
                if (toDoItems[i].Id == item.Id)
                {
                    toDoItems[i] = item;
                }
            }
            return Task.CompletedTask;
        }
        public Task Delete(Guid id, CancellationToken ct)
        {
            foreach (var item in toDoItems)
            {
                if (item.Id == id)
                    toDoItems.Remove(item);
            }
            return Task.CompletedTask;
        }
        //Проверяет есть ли задача с таким именем у пользователя
        public bool ExistsByName(Guid userId, string name)
        {
            foreach (var item in toDoItems)
            {
                if (item.User.UserId == userId && item.Name == name)
                   return true;
            }
            return false;
        }
        //Возвращает количество активных задач у пользователя
        public int CountActive(Guid userId)
        {
            int cnt = 0;
            foreach (var item in toDoItems)
            {
                if (item.User.UserId == userId && item.State == ToDoItemState.Active)
                    cnt++;
            }
            return cnt;
        }
        public Task<ToDoItem> GetToDoItemById(Guid id, CancellationToken ct)
        {
            foreach (var item in toDoItems)
            {
                if (item.Id == id)
                    return Task.FromResult(item);
            }
            return null;
        }
        public Task<IReadOnlyList<ToDoItem>> Find(Guid userId, Func<ToDoItem, bool> predicate, CancellationToken ct)
        {
            var tempList = toDoItems.Where(item => item.User.UserId == userId && predicate(item)).ToList();
            return Task.FromResult((IReadOnlyList<ToDoItem>)tempList);
        }
    }
}

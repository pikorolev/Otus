using System.Diagnostics;
using System.Threading.Tasks;
using HomeWorkFor5Lesson.Core.DataAccess;
using HomeWorkFor5Lesson.Core.Entities;

namespace HomeWorkFor5Lesson.Infrastructure.DataAccess
{
    internal class InMemoryToDoRepository : IToDoRepository
    {
        private readonly List<ToDoItem> toDoItems = new List<ToDoItem>();

        public async Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId, CancellationToken ct)
        {
            List<ToDoItem> tempList = new List<ToDoItem>();
            foreach (var item in toDoItems)
            {
                if (item.User.UserId == userId)
                    tempList.Add(item);
            }
            return tempList;
        }
        //Возвращает ToDoItem для UserId со статусом Active
        public async Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId, CancellationToken ct)
        {
            List<ToDoItem> tempList = new List<ToDoItem>();
            foreach (var item in toDoItems)
            {
                if (item.User.UserId == userId && item.State == ToDoItemState.Active)
                    tempList.Add(item);
            }
            return tempList;
        }
        public async Task Add(ToDoItem item, CancellationToken ct)
        {
            toDoItems.Add(item);
        }
        public async Task Update(ToDoItem item, CancellationToken ct)
        {
            for (int i = 0; i < toDoItems.Count; i++)
            {
                if (toDoItems[i].Id == item.Id)
                {
                    toDoItems[i] = item;
                }
            }
        }
        public async Task Delete(Guid id, CancellationToken ct)
        {
            foreach (var item in toDoItems)
            {
                if (item.Id == id)
                    toDoItems.Remove(item);
            }
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
        public async Task<ToDoItem> GetToDoItemById(Guid id, CancellationToken ct)
        {
            foreach (var item in toDoItems)
            {
                if (item.Id == id)
                    return item;
            }
            return null;
        }
        public async Task<IReadOnlyList<ToDoItem>> Find(Guid userId, Func<ToDoItem, bool> predicate, CancellationToken ct)
        {
            return toDoItems.Where(item => item.User.UserId == userId && predicate(item)).ToList();
        }
    }
}

using HomeWorkFor5Lesson.Core.DataAccess;
using HomeWorkFor5Lesson.Core.Entities;
using Otus.ToDoList.ConsoleBot.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkFor5Lesson.Core.Services
{
    internal class ToDoService : IToDoService
    {
        private readonly IToDoRepository toDoRepository;
        private int taskCountLimit;
        private int taskLengthLimit;
        //private List<ToDoItem> taskList = new List<ToDoItem>();
        // Возващаеет количество задач
        public ToDoService(int taskCountLimit, int taskLengthLimit, IToDoRepository toDoRepository)
        {
            this.taskCountLimit = taskCountLimit;
            this.taskLengthLimit = taskLengthLimit;
            this.toDoRepository = toDoRepository;
        }
        // Возвращает ToDoItem для UserId со статусом Active
        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            return toDoRepository.GetActiveByUserId(userId);
        }
        // Возвращает полный список задач
        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            return toDoRepository.GetAllByUserId(userId);
        }
        // Добавляет новую задачу
        public ToDoItem Add(ToDoUser user, string name)
        {
            if (toDoRepository.CountActive(user.UserId) >= taskCountLimit)
                throw new TaskCountLimitException(taskCountLimit);

            if (name.Length > taskLengthLimit)
                throw new TaskLengthLimitException(name.Length, taskLengthLimit);

            if (toDoRepository.ExistsByName(user.UserId, name))
                throw new DuplicateTaskException(name);

            var newItem = new ToDoItem(user, name);
            toDoRepository.Add(newItem);
            return newItem;
        }
        // Помечает задачу как выполненную
        public void MarkCompleted(Guid id)
        {
            var item = toDoRepository.GetToDoItemById(id);
            if (item != null)
            {
                item.State = ToDoItemState.Completed;
                item.StateChangedAt = DateTime.Now;
                toDoRepository.Update(item);
            }
        }
        // Удаляет задачу
        public void Delete(Guid id)
        {
            toDoRepository.Delete(id);
        }
        public IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix)
        {
            return toDoRepository.Find(user.UserId, item => item.Name.StartsWith(namePrefix, StringComparison.OrdinalIgnoreCase));
        }
    }
}

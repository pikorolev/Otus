using Otus.ToDoList.ConsoleBot.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkFor5Lesson
{
    internal class ToDoService : IToDoService
    {
        private int taskCountLimit;
        private int taskLengthLimit;
        private List<ToDoItem> taskList = new List<ToDoItem>();
        // Возващаеет количество задач
        public ToDoService(int taskCountLimit, int taskLengthLimit)
        {
            this.taskCountLimit = taskCountLimit;
            this.taskLengthLimit = taskLengthLimit;
        }
        public int GetCount()
        {
            return taskList.Count;
        }
        // Возвращает ToDoItem для UserId со статусом Active
        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            List<ToDoItem> tempList = new List<ToDoItem>();
            foreach (var item in taskList)
            {
                if (item.User.UserId == userId && item.State == ToDoItemState.Active)
                    tempList.Add(item);
            }
            return tempList;
        }
        // Возвращает полный список задач
        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            List<ToDoItem> tempList = new List<ToDoItem>();
            foreach (var item in taskList)
            {
                if (item.User.UserId == userId)
                    tempList.Add(item);
            }
            return tempList;
        }
        // Добавляет новую задачу
        public ToDoItem Add(ToDoUser user, string name)
        {
            if (GetCount() >= taskCountLimit)
            {
                throw new TaskCountLimitException(taskCountLimit);
            }
            if (name.Length > taskLengthLimit)
            {
                throw new TaskLengthLimitException(name.Length, taskLengthLimit);
            }
            var toDoList = GetAllByUserId(user.UserId);
            foreach (var toDoItem in toDoList)
            {
                if (toDoItem.Name == name)
                    throw new DuplicateTaskException(name);
            }
            var newItem = new ToDoItem(user, name);
            taskList.Add(newItem);
            return newItem;
        }
        // Помечает задачу как выполненную
        public void MarkCompleted(Guid id)
        {
            foreach (var item in taskList)
            {
                if (item.Id == id)
                {
                    item.State = ToDoItemState.Completed;
                    item.StateChangedAt = DateTime.Now;
                }   
            }
        }
        // Удаляет задачу
        public void Delete(Guid id)
        {
            foreach (var item in taskList)
            {
                if (item.Id == id)
                    taskList.Remove(item);
            }
            
        }
    }
}

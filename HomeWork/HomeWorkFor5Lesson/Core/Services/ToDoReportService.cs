using HomeWorkFor5Lesson.Core.DataAccess;
using HomeWorkFor5Lesson.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkFor5Lesson.Core.Services
{   
    internal class ToDoReportService : IToDoReportService
    {
        private readonly IToDoRepository toDoRepository;

        public ToDoReportService (IToDoRepository toDoRepository)
        {
            this.toDoRepository = toDoRepository;
        }

        public (int total, int completed, int active, DateTime generatedAt) GetUserStats(Guid userId)
        {
            int total = 0;
            int completed = 0;
            int active = 0;

            var items = toDoRepository.GetAllByUserId(userId);
            total = items.Count;
            foreach (ToDoItem item in items)
            {
                switch (item.State)
                {
                    case ToDoItemState.Completed:
                        completed++;
                        break;
                    case ToDoItemState.Active:
                        active++;
                        break;
                }
            }
            return (total, completed, active, DateTime.UtcNow);
        }
    }
}

using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HomeWorkFor5Lesson
{
    internal class UpdateHandler : IUpdateHandler
    {
        UserService userService = new UserService();
        ToDoService toDoService = new ToDoService();
        private ToDoUser currentUser;
        private static int taskCountLimit;
        private static int taskLengthLimit;
        public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
        {
            try
            {
                if (taskCountLimit == 0)
                {
                    EnterTaskCount(botClient, update);
                }

                if (taskLengthLimit == 0)
                {
                    EnterTaskLengthLimit(botClient, update);
                }
                EnableCommand(botClient, update);

                switch (update.Message.Text.Split(' ')[0])
                {
                    case @"/start":
                        StartCommand(botClient, update);
                        break;
                    case @"/help":
                        HelpCommand(botClient, update);
                        break;
                    case @"/info":
                        InfoCommand(botClient, update);
                        break;
                    case @"/addtask":
                        AddTaskCommand(botClient, update);
                        break;
                    case @"/showtasks":
                        ShowTasksCommand(botClient, update);
                        break;
                    case @"/removetask":
                        RemoveTaskCommand(botClient, update);
                        break;
                    case @"/completetask":
                        CompleteTask(botClient, update);
                        break;
                    case @"/showalltasks":
                        ShowAllTasks(botClient, update);
                        break;
                    case @"/exit":
                        return;
                    default:
                        if (currentUser != null)
                            botClient.SendMessage(update.Message.Chat, $"{currentUser.TelegramUserName}, вы ввели неизвестную команду:");
                        else
                            botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                botClient.SendMessage(update.Message.Chat,ex.Message);
            }
            catch (TaskCountLimitException ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message);
            }
            catch (TaskLengthLimitException ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message);
            }
            catch (DuplicateTaskException ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message);
            }
        }

        private void ShowAllTasks(ITelegramBotClient botClient, Update update)
        {
            if (currentUser == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
            }

            if (toDoService.GetCount() == 0)
                botClient.SendMessage(update.Message.Chat, "Список задач пуст");
            else
                botClient.SendMessage(update.Message.Chat, "Вот ваш список задач:");
            foreach (var task in toDoService.GetAllByUserId(currentUser.UserId))
            {
                botClient.SendMessage(update.Message.Chat, $"({task.State}) {task.Name} - {task.CreatedAt} - {task.Id}");
            }
        }

        private void CompleteTask(ITelegramBotClient botClient, Update update)
        {
            if (currentUser == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
            }
            string str = update.Message.Text.Substring(@"/completetask ".Length);
            Guid delGuig = new Guid(str);
            toDoService.MarkCompleted(delGuig);
        }

        // Ввод максимального кол-ва задач
        private static void EnterTaskCount(ITelegramBotClient botClient, Update update)
        {
            botClient.SendMessage(update.Message.Chat,"Введите максимально допустимое количество задач");
            string? taskCountInput = Console.ReadLine();
            taskCountLimit = ParseAndValidateInt(taskCountInput, 1, 100);

        }
        // Ввод максимальной длины задачи
        private static void EnterTaskLengthLimit(ITelegramBotClient botClient, Update update)
        {
            botClient.SendMessage(update.Message.Chat, "Введите максимально допустимую длину задачи");
            string? taskLengthInput = Console.ReadLine();
            taskLengthLimit = ParseAndValidateInt(taskLengthInput, 1, 100);

        }
        // Приводит полученную строку к int и проверяет, что оно находится в диапазоне min и max
        private static int ParseAndValidateInt(string? str, int min, int max)
        {
            ValidateString(str);

            if (!int.TryParse(str, out int result))
            {
                throw new ArgumentException("Вводимое значение должно быть целым числом.");
            }

            if (result < min || result > max)
            {
                throw new ArgumentException($"Число должно быть от {min} до {max}.");
            }

            return result;
        }
        // Проверяем, является ли строка null или пустой
        private static void ValidateString(string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Вводимое значение не должно быть пустым.");
            }
        }
        // Вывоодим список доступных комманд
        private void EnableCommand(ITelegramBotClient botClient, Update update)
        {
            if (currentUser != null)
                botClient.SendMessage(update.Message.Chat, $"{currentUser.TelegramUserName}, вам доступные команды: /start, /help/, /info, /addttask, /showtasks, /removetask, /completetask, /showalltasks ,/exit");
            else
                botClient.SendMessage(update.Message.Chat, @"Доступные команды: /start, /help/, /info, /exit");
        }
        //Обработка команды /start
        private void StartCommand(ITelegramBotClient botClient, Update update)
        {
            currentUser = userService.RegisterUser(update.Message.From.Id, update.Message.From.Username);
        }
        // Обработка команды /help
        private void HelpCommand(ITelegramBotClient botClient, Update update)
        {
            if (currentUser != null)
                botClient.SendMessage(update.Message.Chat, $"{currentUser.TelegramUserName}, вам доступные команды:");
            else
                botClient.SendMessage(update.Message.Chat, "Вам доступные команды:");
            if (currentUser != null)
            {
                botClient.SendMessage(update.Message.Chat, @"  /addtask : позволяет добавить задачу в список.");
                botClient.SendMessage(update.Message.Chat, @"  /showtasks : выводит список активных задач.");
                botClient.SendMessage(update.Message.Chat, @"  /removetask : позвояет удаить задачу.");
                botClient.SendMessage(update.Message.Chat, @"  /completetask : заверршает задачу.");
                botClient.SendMessage(update.Message.Chat, @"  /showalltasks : выводит все задачи.");
            }
            botClient.SendMessage(update.Message.Chat, @"  /start : позволяет ввести в порграмму Ваше имя, что бы прорграмма Вас узнавала.");
            botClient.SendMessage(update.Message.Chat, @"  /help : отображает краткую справочную информацию о том, как пользоваться программой.");
            botClient.SendMessage(update.Message.Chat, @"  /info : предоставляет информацию о версии программы и дате её создания.");
            botClient.SendMessage(update.Message.Chat, @"  /exit : завершает программу.");
        }
        // Обработка команды /info
        private void InfoCommand(ITelegramBotClient botClient, Update update)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (currentUser != null)
                botClient.SendMessage(update.Message.Chat, $"{currentUser.TelegramUserName}, предоставляю Вам информацию о программе:");
            else
                botClient.SendMessage(update.Message.Chat, "Информация о программе:");
            botClient.SendMessage(update.Message.Chat, $"  Версия: {assembly.GetName().Version}");
            botClient.SendMessage(update.Message.Chat, $"  Дата создания: {System.IO.File.GetLastWriteTime(assembly.Location)}");
        }
        // Обработка команды /addtask
        private void AddTaskCommand(ITelegramBotClient botClient, Update update)
        {
            if (currentUser == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
            }

            if (toDoService.GetCount() >= taskCountLimit)
            {
                throw new TaskCountLimitException(taskCountLimit, botClient, update);
            }

            string str = update.Message.Text.Substring(@"/addtask ".Length);

            if (str.Length > taskLengthLimit)
            {
                throw new TaskLengthLimitException(str.Length, taskLengthLimit, botClient, update);
            }

            if (String.IsNullOrEmpty(str))
                botClient.SendMessage(update.Message.Chat, "Нельзя добавить задачу с пустым описанием");
            else
            {
                var toDoList = toDoService.GetAllByUserId(currentUser.UserId);
                foreach ( var toDoItem in toDoList)
                {
                    if (toDoItem.Name == str)
                        throw new DuplicateTaskException(str, botClient, update);
                }

                var newTask = toDoService.Add(currentUser, str);
                botClient.SendMessage(update.Message.Chat, @$"Задача ""{str}"" добавлена в список");
            }
        }
        // Обработка команды /showtasks
        private void ShowTasksCommand(ITelegramBotClient botClient, Update update)
        {
            if (currentUser == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
            }

            if (toDoService.GetCount() == 0)
                botClient.SendMessage(update.Message.Chat, "Список задач пуст");
            else
                botClient.SendMessage(update.Message.Chat, "Вот ваш список задач:");
            foreach(var task in toDoService.GetActiveByUserId(currentUser.UserId))
            {
                botClient.SendMessage(update.Message.Chat, $"{task.Name} - {task.CreatedAt} - {task.Id}");
            }
        }
        // Обработка команды /removetask
        private void RemoveTaskCommand(ITelegramBotClient botClient, Update update)
        {
            if (currentUser == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
            }

            if (toDoService.GetCount() == 0)
            {
                botClient.SendMessage(update.Message.Chat, "Удалять нечего");
                return;
            }
            string str = update.Message.Text.Substring(@"/removetask ".Length);
            Guid delGuig = new Guid(str);
            toDoService.Delete(delGuig);
        }
    }
}

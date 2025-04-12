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
        IUserService userService;
        IToDoService toDoService;
        public UpdateHandler(IUserService userService, IToDoService toDoService)
        {
            this.userService = userService;
            this.toDoService = toDoService;
        }
        public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
        {
            try
            {
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
                        if (userService.GetUser(update.Message.From.Id) != null)
                            botClient.SendMessage(update.Message.Chat, $"{userService.GetUser(update.Message.From.Id)}, вы ввели неизвестную команду:");
                        else
                            botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (TaskCountLimitException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (TaskLengthLimitException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DuplicateTaskException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ShowAllTasks(ITelegramBotClient botClient, Update update)
        {
            if (userService.GetUser(update.Message.From.Id) == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
            }

            if (toDoService.GetCount() == 0)
                botClient.SendMessage(update.Message.Chat, "Список задач пуст");
            else
                botClient.SendMessage(update.Message.Chat, "Вот ваш список задач:");
            foreach (var task in toDoService.GetAllByUserId(userService.GetUser(update.Message.From.Id).UserId))
            {
                botClient.SendMessage(update.Message.Chat, $"({task.State}) {task.Name} - {task.CreatedAt} - {task.Id}");
            }
        }

        private void CompleteTask(ITelegramBotClient botClient, Update update)
        {
            if (userService.GetUser(update.Message.From.Id) == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
            }
            string str = update.Message.Text.Substring(@"/completetask ".Length);
            Guid delGuig = new Guid(str);
            toDoService.MarkCompleted(delGuig);
        }
        // Вывоодим список доступных комманд
        private void EnableCommand(ITelegramBotClient botClient, Update update)
        {
            if (userService.GetUser(update.Message.From.Id) != null)
                botClient.SendMessage(update.Message.Chat, $"{userService.GetUser(update.Message.From.Id).TelegramUserName}, вам доступные команды: /start, /help/, /info, /addttask, /showtasks, /removetask, /completetask, /showalltasks ,/exit");
            else
                botClient.SendMessage(update.Message.Chat, @"Доступные команды: /start, /help/, /info, /exit");
        }
        //Обработка команды /start
        private void StartCommand(ITelegramBotClient botClient, Update update)
        {
            userService.RegisterUser(update.Message.From.Id, update.Message.From.Username);
        }
        // Обработка команды /help
        private void HelpCommand(ITelegramBotClient botClient, Update update)
        {
            if (userService.GetUser(update.Message.From.Id) != null)
                botClient.SendMessage(update.Message.Chat, $"{userService.GetUser(update.Message.From.Id).TelegramUserName}, вам доступные команды:");
            else
                botClient.SendMessage(update.Message.Chat, "Вам доступные команды:");
            if (userService.GetUser(update.Message.From.Id) != null)
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
            if (userService.GetUser(update.Message.From.Id) != null)
                botClient.SendMessage(update.Message.Chat, $"{userService.GetUser(update.Message.From.Id).TelegramUserName}, предоставляю Вам информацию о программе:");
            else
                botClient.SendMessage(update.Message.Chat, "Информация о программе:");
            botClient.SendMessage(update.Message.Chat, $"  Версия: {assembly.GetName().Version}");
            botClient.SendMessage(update.Message.Chat, $"  Дата создания: {System.IO.File.GetLastWriteTime(assembly.Location)}");
        }
        // Обработка команды /addtask
        private void AddTaskCommand(ITelegramBotClient botClient, Update update)
        {
            if (userService.GetUser(update.Message.From.Id) == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
            }

            string str = update.Message.Text.Substring(@"/addtask ".Length);

            if (String.IsNullOrEmpty(str))
                botClient.SendMessage(update.Message.Chat, "Нельзя добавить задачу с пустым описанием");
            else
            {
                var newTask = toDoService.Add(userService.GetUser(update.Message.From.Id), str);
                botClient.SendMessage(update.Message.Chat, @$"Задача ""{str}"" добавлена в список");
            }
        }
        // Обработка команды /showtasks
        private void ShowTasksCommand(ITelegramBotClient botClient, Update update)
        {
            if (userService.GetUser(update.Message.From.Id) == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:");
            }

            if (toDoService.GetCount() == 0)
                botClient.SendMessage(update.Message.Chat, "Список задач пуст");
            else
                botClient.SendMessage(update.Message.Chat, "Вот ваш список задач:");
            foreach(var task in toDoService.GetActiveByUserId(userService.GetUser(update.Message.From.Id).UserId))
            {
                botClient.SendMessage(update.Message.Chat, $"{task.Name} - {task.CreatedAt} - {task.Id}");
            }
        }
        // Обработка команды /removetask
        private void RemoveTaskCommand(ITelegramBotClient botClient, Update update)
        {
            if (userService.GetUser(update.Message.From.Id) == null)
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

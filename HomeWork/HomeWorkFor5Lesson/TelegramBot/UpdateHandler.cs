using HomeWorkFor5Lesson.Core.DataAccess;
using HomeWorkFor5Lesson.Core.Services;
using HomeWorkFor5Lesson.TelegramBot;
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

namespace HomeWorkFor5Lesson.TelegramBot
{
    public delegate void MessageEventHandler(string message);

    class UpdateHandler : IUpdateHandler
    {

        private readonly IUserService userService;
        private readonly IToDoService toDoService;
        private readonly IToDoReportService toDoReportService;

        public event MessageEventHandler OnHandleUpdateStarted;
        public event MessageEventHandler OnHandleUpdateCompleted;
        public UpdateHandler(IUserService userService, IToDoService toDoService, IToDoReportService toDoReportService)
        {
            this.userService = userService;
            this.toDoService = toDoService;
            this.toDoReportService = toDoReportService;
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
        {
            Console.WriteLine($"HandleError: {exception})");
            return Task.CompletedTask;
        }
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            OnHandleUpdateStarted?.Invoke(update.Message.Text);
            try
            {
                await EnableCommand(botClient, update, ct);

                switch (update.Message.Text.Split(' ')[0])
                {
                    case @"/start":
                        await StartCommand(botClient, update, ct);
                        break;
                    case @"/help":
                        await HelpCommand(botClient, update, ct);
                        break;
                    case @"/info":
                        await InfoCommand(botClient, update, ct);
                        break;
                    case @"/addtask":
                        await AddTaskCommand(botClient, update, ct);
                        break;
                    case @"/showtasks":
                        await ShowTasksCommand(botClient, update, ct);
                        break;
                    case @"/removetask":
                        await RemoveTaskCommand(botClient, update, ct);
                        break;
                    case @"/completetask":
                        await CompleteTask(botClient, update, ct);
                        break;
                    case @"/showalltasks":
                        await ShowAllTasks(botClient, update, ct);
                        break;
                    case @"/report":
                        await ReportStats(botClient, update, ct);
                        break;
                    case @"/find":
                        await FindTask(botClient, update, ct);
                        break;
                    case @"/exit":
                        break;
                    default:
                        var user = await userService.GetUser(update.Message.From.Id, ct);
                        if (user != null)
                            botClient.SendMessage(update.Message.Chat, $"{user.TelegramUserName}, вы ввели неизвестную команду:", ct);
                        else
                            botClient.SendMessage(update.Message.Chat, "Неизвестная команда:", ct);
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
            finally
            {
                OnHandleUpdateCompleted?.Invoke(update.Message.Text);
            }
        }

        private async Task FindTask(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            string str = update.Message.Text.Substring(@"/find ".Length);
            var user = await userService.GetUser(update.Message.From.Id, ct);
            var foundItems = await toDoService.Find(user, str);
            if (foundItems.Count == 0)
            {
                botClient.SendMessage(update.Message.Chat, "Задачи не найдены.", ct);
            }
            else
            {
                foreach (var item in foundItems)
                {
                    botClient.SendMessage(update.Message.Chat, $"{item.Name} - {item.CreatedAt} - {item.Id}", ct);
                }
            }
        }

        private async Task ReportStats(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            var user = await userService.GetUser(update.Message.From.Id, ct);
            var stats = await toDoReportService.GetUserStats(user.UserId);
            botClient.SendMessage(update.Message.Chat, $"Статистика по задачам на {stats.generatedAt}. Всего: {stats.total}; Завершенных: {stats.completed}; Активных: {stats.active};", ct);
        }

        private async Task ShowAllTasks(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            var user = await userService.GetUser(update.Message.From.Id, ct);
            if (user == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:", ct);
            }
            botClient.SendMessage(update.Message.Chat, "Вот ваш список задач:", ct);
            foreach (var task in await toDoService.GetAllByUserId(user.UserId))
            {
                botClient.SendMessage(update.Message.Chat, $"({task.State}) {task.Name} - {task.CreatedAt} - {task.Id}", ct);
            }
        }

        private async Task CompleteTask(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            var user = await userService.GetUser(update.Message.From.Id, ct);
            if (user == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:", ct);
            }
            string str = update.Message.Text.Substring(@"/completetask ".Length);
            Guid delGuig = new Guid(str);
            toDoService.MarkCompleted(delGuig);
        }
        // Вывоодим список доступных комманд
        private async Task EnableCommand(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            var user = await userService.GetUser(update.Message.From.Id, ct);
            if (user != null)
                botClient.SendMessage(update.Message.Chat, $"{user.TelegramUserName}, вам доступные команды: /start, /help/, /info, /addttask, /showtasks, /removetask, /completetask, /showalltasks ,/report, /find, /exit", ct);
            else
                botClient.SendMessage(update.Message.Chat, @"Доступные команды: /start, /help/, /info, /exit", ct);
        }
        //Обработка команды /start
        private async Task StartCommand(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            await userService.RegisterUser(update.Message.From.Id, update.Message.From.Username, ct);
        }
        // Обработка команды /help
        private async Task HelpCommand(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            var user = await userService.GetUser(update.Message.From.Id, ct);
            if (user != null)
                botClient.SendMessage(update.Message.Chat, $"{user.TelegramUserName}, вам доступные команды:", ct);
            else
                botClient.SendMessage(update.Message.Chat, "Вам доступные команды:", ct);
            if (user != null)
            {
                botClient.SendMessage(update.Message.Chat, @"  /addtask : позволяет добавить задачу в список.", ct);
                botClient.SendMessage(update.Message.Chat, @"  /showtasks : выводит список активных задач.", ct);
                botClient.SendMessage(update.Message.Chat, @"  /removetask : позвояет удаить задачу.", ct);
                botClient.SendMessage(update.Message.Chat, @"  /completetask : заверршает задачу.", ct);
                botClient.SendMessage(update.Message.Chat, @"  /showalltasks : выводит все задачи.", ct);
                botClient.SendMessage(update.Message.Chat, @"  /report : выводит статистику по задачам.", ct);
                botClient.SendMessage(update.Message.Chat, @"  /find : выводит все задачи которые начинаются с заданного значения.", ct);
            }
            botClient.SendMessage(update.Message.Chat, @"  /start : позволяет ввести в порграмму Ваше имя, что бы прорграмма Вас узнавала.", ct);
            botClient.SendMessage(update.Message.Chat, @"  /help : отображает краткую справочную информацию о том, как пользоваться программой.", ct);
            botClient.SendMessage(update.Message.Chat, @"  /info : предоставляет информацию о версии программы и дате её создания.", ct);
            botClient.SendMessage(update.Message.Chat, @"  /exit : завершает программу.", ct);
        }
        // Обработка команды /info
        private async Task InfoCommand(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var user = await userService.GetUser(update.Message.From.Id, ct);
            if (user != null)
                botClient.SendMessage(update.Message.Chat, $"{user.TelegramUserName}, предоставляю Вам информацию о программе:", ct);
            else
                botClient.SendMessage(update.Message.Chat, "Информация о программе:", ct);
            botClient.SendMessage(update.Message.Chat, $"  Версия: {assembly.GetName().Version}", ct);
            botClient.SendMessage(update.Message.Chat, $"  Дата создания: {File.GetLastWriteTime(assembly.Location)}", ct);
        }
        // Обработка команды /addtask
        private async Task AddTaskCommand(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            var user = await userService.GetUser(update.Message.From.Id, ct);
            if (user == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:", ct);
            }

            string str = update.Message.Text.Substring(@"/addtask ".Length);

            if (string.IsNullOrEmpty(str))
                botClient.SendMessage(update.Message.Chat, "Нельзя добавить задачу с пустым описанием", ct);
            else
            {
                var newTask = toDoService.Add(user, str);
                botClient.SendMessage(update.Message.Chat, @$"Задача ""{str}"" добавлена в список", ct);
            }
        }
        // Обработка команды /showtasks
        private async Task ShowTasksCommand(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            var user = await userService.GetUser(update.Message.From.Id, ct);
            if (user == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:", ct);
            }
            botClient.SendMessage(update.Message.Chat, "Вот ваш список задач:", ct);
            foreach (var task in await toDoService.GetActiveByUserId(user.UserId))
            {
                botClient.SendMessage(update.Message.Chat, $"{task.Name} - {task.CreatedAt} - {task.Id}", ct);
            }
        }
        // Обработка команды /removetask
        private async Task RemoveTaskCommand(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            var user = await userService.GetUser(update.Message.From.Id, ct);
            if (user == null)
            {
                botClient.SendMessage(update.Message.Chat, "Неизвестная команда:", ct);
            }

            string str = update.Message.Text.Substring(@"/removetask ".Length);
            Guid delGuig = new Guid(str);
            toDoService.Delete(delGuig);
        }
    }
}

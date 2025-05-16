using HomeWorkFor5Lesson.Infrastructure.DataAccess;
using Otus.ToDoList.ConsoleBot;
using HomeWorkFor5Lesson.Core.Services;
using HomeWorkFor5Lesson.TelegramBot;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            using var cts = new CancellationTokenSource();

            InMemoryUserRepository inMemoryUserRepository = new InMemoryUserRepository();
            UserService userService = new UserService(inMemoryUserRepository);

            var taskCountLimit = EnterTaskCount();
            var taskLengthLimit = EnterTaskLengthLimit();
            InMemoryToDoRepository inMemoryToDoRepository = new InMemoryToDoRepository();
            ToDoService toDoService = new ToDoService(taskCountLimit, taskLengthLimit, inMemoryToDoRepository);

            ToDoReportService toDoReportService = new ToDoReportService(inMemoryToDoRepository);

            var handler = new UpdateHandler(userService, toDoService, toDoReportService);
            var botClient = new ConsoleBotClient();

            handler.OnHandleUpdateStarted += (message) => Console.WriteLine($"Началась обработка сообщения '{message}'");
            handler.OnHandleUpdateCompleted += (message) => Console.WriteLine($"Закончилась обработка сообщения '{message}'");

            try
            {
                botClient.StartReceiving(handler, cts.Token);
            }
            finally
            {
                handler.OnHandleUpdateStarted -= (message) => Console.WriteLine($"Началась обработка сообщения '{message}'");
                handler.OnHandleUpdateCompleted -= (message) => Console.WriteLine($"Закончилась обработка сообщения '{message}'");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка: {ex.GetType()}, {ex.Message}, {ex.StackTrace}, {ex.InnerException}");
        }
    }
    // Проверяем, является ли строка null или пустой
    private static void ValidateString(string? str)
    {
        if (string.IsNullOrEmpty(str))
        {
            throw new ArgumentException("Вводимое значение не должно быть пустым.");
        }
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
    // Ввод максимального кол-ва задач
    private static int EnterTaskCount()
    {
        Console.WriteLine("Введите максимально допустимое количество задач");
        string? taskCountInput = Console.ReadLine();
        return ParseAndValidateInt(taskCountInput, 1, 100);
    }
    // Ввод максимальной длины задачи
    private static int EnterTaskLengthLimit()
    {
        Console.WriteLine("Введите максимально допустимую длину задачи");
        string? taskLengthInput = Console.ReadLine();
        return ParseAndValidateInt(taskLengthInput, 1, 100);
    }

}


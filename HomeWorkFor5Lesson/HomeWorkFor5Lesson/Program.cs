using System;
using System.Reflection;
class Program
{
    private static String? name;
    private static List<String> taskList = new List<String>();
    private static int taskCountLimit;
    private static int taskLengthLimit;


    static void Main(string[] args)
    {
        Console.WriteLine("Привет!");
        while (true)
        {
            try
            {
                if (taskCountLimit == 0)
                {
                    EnterTaskCount();
                }

                if (taskLengthLimit == 0)
                {
                    EnterTaskLengthLimit();
                }
                EnableCommand();

                Console.WriteLine("Введите команду:");
                var command = Console.ReadLine();
                switch (command.Split(' ')[0])
                {
                    case @"/start":
                        StartCommand();
                        break;
                    case @"/help":
                        HelpCommand();
                        break;
                    case @"/info":
                        InfoCommand();
                        break;
                    case @"/addtask":
                        AddTaskCommand();
                        break;
                    case @"/showtasks":
                        ShowTasksCommand();
                        break;
                    case @"/removetask":
                        RemoveTaskCommand();
                        break;
                    case @"/echo":
                        EchoCommand(command.Substring(command.IndexOf(' ') + 1));
                        break;
                    case @"/exit":
                        return;
                    default:
                        if (!string.IsNullOrEmpty(name))
                            Console.WriteLine($"{name}, вы ввели неизвестную команду:");
                        else
                            Console.WriteLine("Неизвестная команда:");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
            catch (TaskCountLimitException ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
            catch (TaskLengthLimitException ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
            catch (DuplicateTaskException ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.GetType()}, {ex.Message}, {ex.StackTrace}, {ex.InnerException}");
                continue;
            }
        }
    }
    // Ввод максимального кол-ва задач
    private static void EnterTaskCount()
    {
        Console.WriteLine("Введите максимально допустимое количество задач");
        string? taskCountInput = Console.ReadLine();
        taskCountLimit = ParseAndValidateInt(taskCountInput, 1, 100);

    }
    // Ввод максимальной длины задачи
    private static void EnterTaskLengthLimit()
    {
        Console.WriteLine("Введите максимально допустимую длину задачи");
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
    private static void EnableCommand()
    {
        if (!string.IsNullOrEmpty(name))
            Console.WriteLine($"{name}, вам доступные команды: /start, /help/, /info, /echo, /addttask, /showtasks, /removetask, /exit");
        else
            Console.WriteLine(@"Доступные команды: /start, /help/, /info, /addttask, /showtasks, /removetask, /exit");
    }
    //Обработка команды /start
    private static void StartCommand()
    {
        Console.WriteLine("Введите свое имя:");
        name = Console.ReadLine();
        ValidateString(name);
        Console.WriteLine($"Приятно познакомиться, {name}!");
    }
    // Обработка команды /help
    private static void HelpCommand()
    {
        if (!string.IsNullOrEmpty(name))
            Console.WriteLine($"{name}, вам доступные команды:");
        else
            Console.WriteLine("Вам доступные команды:");
        Console.WriteLine(@"  /start : позволяет ввести в порграмму Ваше имя, что бы прорграмма Вас узнавала.");
        Console.WriteLine(@"  /help : отображает краткую справочную информацию о том, как пользоваться программой.");
        Console.WriteLine(@"  /info : предоставляет информацию о версии программы и дате её создания.");
        if (!string.IsNullOrEmpty(name))
            Console.WriteLine(@"  /echo : возвращает поьзоватеьский текст, введенный после /echo");
        Console.WriteLine(@"  /addtask : позволяет добавить задачу в список.");
        Console.WriteLine(@"  /showtasks : выводит список задач.");
        Console.WriteLine(@"  /removetask : позвояет удаить задачу.");
        Console.WriteLine(@"  /exit : завершает программу.");
    }
    // Обработка команды /info
    private static void InfoCommand()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        if (!string.IsNullOrEmpty(name))
            Console.WriteLine($"{name}, предоставляю Вам информацию о программе:");
        else
            Console.WriteLine("Информация о программе:");
        Console.WriteLine($"  Версия: {assembly.GetName().Version}");
        Console.WriteLine($"  Дата создания: {System.IO.File.GetLastWriteTime(assembly.Location)}");
    }
    // Обработка команды /addtask
    private static void AddTaskCommand()
    {
        if (taskList.Count >= taskCountLimit)
        {
            throw new TaskCountLimitException(taskCountLimit);
        }
        if (!string.IsNullOrEmpty(name))
            Console.WriteLine($"{name}, введите описание задачи:");
        else
            Console.WriteLine("Введите описание задачи:");

        var taskDesc = Console.ReadLine();
        if (taskDesc.Length > taskLengthLimit)
        {
            throw new TaskLengthLimitException(taskDesc.Length, taskLengthLimit);
        }

        if (String.IsNullOrEmpty(taskDesc))
            Console.WriteLine("Нельзя добавить задачу с пустым описанием");
        else
        {
            if (taskList.Contains(taskDesc))
            {
                throw new DuplicateTaskException(taskDesc);
            }
            else
            {
                taskList.Add(taskDesc);
                Console.WriteLine(@$"Задача ""{taskDesc}"" добавлена в список");
            }
        }
    }
    // Обработка команды /showtasks
    private static void ShowTasksCommand()
    {
        if (taskList.Count == 0)
            Console.WriteLine("Список задач пуст");
        else
            Console.WriteLine("Вот ваш список задач:");
        for (int i = 0; i < taskList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {taskList[i]} ");
        }
    }
    // Обработка команды /removetask
    private static void RemoveTaskCommand()
    {
        ShowTasksCommand();
        if (taskList.Count == 0)
        {
            Console.WriteLine("Удалять нечего");
            return;
        }
        while (true)
        {
            Console.WriteLine("Укажите номер задачи, которую вы хотите удалить:");
            var taskNum = Console.ReadLine();
            int intNum;
            if (int.TryParse(taskNum, out intNum))
            {
                if (intNum > 0 && intNum <= taskList.Count)
                {
                    Console.WriteLine(@$"Задача ""{taskList[intNum - 1]}"" удалена");
                    taskList.RemoveAt(intNum - 1);
                    return;
                }
            }
            Console.WriteLine("Введен некорректный номер задачи");
        }
    }
    // Обработка команды /echo
    private static void EchoCommand(String str)
    {
        if (string.IsNullOrEmpty(name))
            if (!string.IsNullOrEmpty(name))
                Console.WriteLine($"{name}, вы ввели неизвестную команду:");
            else
                Console.WriteLine("Неизвестная команда:");
        else
            Console.WriteLine(str);
    }
}


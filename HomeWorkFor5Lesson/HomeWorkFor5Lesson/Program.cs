// See https://aka.ms/new-console-template for more information
/*
Доступ к команде /echo: После ввода имени становится доступной команда /echo. При вводе этой команды с аргументом (например, /echo Hello), программа возвращает введенный текст (в данном примере "Hello").
*/
using System;
using System.Reflection;
class Program
{
    private static String name = "";
    private static List<String> taskList = new List<String>();
    static void Main(string[] args)
    {
        Console.WriteLine("Привет!");
        Console.WriteLine(@"Доступные команды: /start, /help/, /info, /addttask, /showtasks, /removetask, /exit");
        while (true)
        {
            Console.WriteLine("Введите команду:");
            var command = Console.ReadLine();
            switch (command.Split(' ')[0])
            {
                case @"/start":
                    Console.WriteLine("Введите свое имя:");
                    name = Console.ReadLine();
                    Console.WriteLine($"Приятно познакомиться, {name}!");
                    break;
                case @"/help":
                    HelpCommand();
                    break;
                case @"/info":
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    if (!string.IsNullOrEmpty(name))
                        Console.WriteLine($"{name}, предоставляю Вам информацию о программе:");
                    else
                        Console.WriteLine("Информация о программе:");
                    Console.WriteLine($"  Версия: {assembly.GetName().Version}");
                    Console.WriteLine($"  Дата создания: {System.IO.File.GetLastWriteTime(assembly.Location)}");
                    break;
                case @"/addtask":
                    AddTask();
                    break;
                case @"/showtasks":
                    ShowTasks();
                    break;
                case @"/removetask":
                    RemoveTask();
                    break;
                case @"/echo":
                    if (string.IsNullOrEmpty(name))
                        if (!string.IsNullOrEmpty(name))
                            Console.WriteLine($"{name}, вы ввели неизвестную команду:");
                        else
                            Console.WriteLine("Неизвестная команда:");
                    else
                        //Console.WriteLine($"Вы просили называть вас {name}."); // На занятии, вроде, обсуждали такую реализацию
                        Console.WriteLine(command.Substring(command.IndexOf(' ') + 1));
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
    }

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
            //Console.WriteLine(@"  /echo : Возвращает имя полльзователя"); // На занятии, вроде, обсуждали такую реализацию
            Console.WriteLine(@"  /echo : возвращает поьзоватеьский текст, введенный после /echo");
        Console.WriteLine(@"  /addtask : позволяет добавить задачу в список.");
        Console.WriteLine(@"  /showtasks : выводит список задач.");
        Console.WriteLine(@"  /removetask : позвояет удаить задачу.");
        Console.WriteLine(@"  /exit : завершает программу.");
    }

    private static void AddTask()
    {
        if (!string.IsNullOrEmpty(name))
            Console.WriteLine($"{name}, введите описание задачи:");
        else
            Console.WriteLine("Введите описание задачи:");
        var taskDesc = Console.ReadLine();
        if (String.IsNullOrEmpty(taskDesc))
            Console.WriteLine("Нельзя добавить задачу с пустым описанием");
        else
        {
            taskList.Add(taskDesc);
            Console.WriteLine(@$"Задача ""{taskDesc}"" добавлена в список");
        }

    }

    private static void ShowTasks()
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

    private static void RemoveTask()
    {
        ShowTasks();
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
}


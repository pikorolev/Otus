// See https://aka.ms/new-console-template for more information
/*
Доступ к команде /echo: После ввода имени становится доступной команда /echo. При вводе этой команды с аргументом (например, /echo Hello), программа возвращает введенный текст (в данном примере "Hello").
*/
using System;
using System.Reflection;
class Program
{
    static void Main(string[] args)
    {
        String name = "";
        Console.WriteLine("Привет!");
        Console.WriteLine(@"Доступные команды: /start, /help/, /info, /exit");
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
                    if (!string.IsNullOrEmpty(name))
                        Console.WriteLine($"{name}, вам доступные команды:");
                    else
                        Console.WriteLine("Вам доступные команды:");
                    Console.WriteLine(@"  /start : позволяет ввести в порграмму Ваше имя, что бы прорграмма Вас узнавала.");
                    Console.WriteLine(@"  /help : отображает краткую справочную информацию о том, как пользоваться программой.");
                    Console.WriteLine(@"  /info : предоставляет информацию о версии программы и дате её создания.");
                    if (!string.IsNullOrEmpty(name))
                        //Console.WriteLine(@"  /echo : Возвращает имя полльзователя"); // На занятии, вроде, обсуждали такую реализацию
                        Console.WriteLine(@"  /echo : Возвращает поьзоватеьский текст, введенный после /echo");
                    Console.WriteLine(@"  /exit : завершает программу.");
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
}


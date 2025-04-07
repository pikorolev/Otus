using HomeWorkFor5Lesson;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using System;
using System.Reflection;
class Program
{
    static void Main(string[] args)
    {
        var handler = new UpdateHandler();
        var botClient = new ConsoleBotClient();
        botClient.StartReceiving(handler);
        Update update = new Update();

        botClient.SendMessage(update.Message.Chat, "Привет!");
        while (true)
        {
            try
            {
                string str = Console.ReadLine();
                update = new Update () 
                {
                    Message = new Message()
                    {
                        Text = str,
                        From = new User()
                        {
                            Id = 1,
                            Username = "Телерамм пользователь"
                        }
                    }
                };

                handler.HandleUpdateAsync(botClient, update);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.GetType()}, {ex.Message}, {ex.StackTrace}, {ex.InnerException}");
                continue;
            }
        }
    }
    
}


using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using System;

public class TaskCountLimitException : Exception
{
    public TaskCountLimitException(int taskCountLimit, ITelegramBotClient botClient, Update update)
        : base($"Превышено максимальное количество задач равное {taskCountLimit}")
    {
    }
}
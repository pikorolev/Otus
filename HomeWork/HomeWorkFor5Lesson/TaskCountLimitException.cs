using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using System;

public class TaskCountLimitException : Exception
{
    public TaskCountLimitException(int taskCountLimit)
        : base($"Превышено максимальное количество задач равное {taskCountLimit}")
    {
    }
}
using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using System;

public class TaskLengthLimitException : Exception
{
    public TaskLengthLimitException(int taskLength, int taskLengthLimit)
        : base(@$"Длина задачи ""{taskLength}"" превышает максимально допустимое значение {taskLengthLimit}.")
    {
    }
}

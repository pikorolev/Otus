using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using System;

public class DuplicateTaskException : Exception
{
    public DuplicateTaskException(string task)
        : base(@$"Задача ""{task}"" уже существует.")
    {
    }
}

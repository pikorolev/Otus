﻿using HomeWorkFor5Lesson;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using System;
using System.Reflection;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            var taskCountLimit = EnterTaskCount();
            var taskLengthLimit = EnterTaskLengthLimit();

            UserService userService = new UserService();
            ToDoService toDoService = new ToDoService(taskCountLimit, taskLengthLimit);

            var handler = new UpdateHandler(userService, toDoService);
            var botClient = new ConsoleBotClient();

            

            botClient.StartReceiving(handler);
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


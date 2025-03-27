using System;
using System.Reflection;

namespace HomeWorkFor13Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Stack("a", "b", "c");

            // size = 3, Top = 'c'
            Console.WriteLine($"size = {s.Size}, Top = '{s.Top}'");

            // в стеке s теперь элементы - "a", "b", "c", "3", "2", "1" <- верхний
            s.Merge(new Stack("1", "2", "3"));

            // в стеке s теперь элементы -  "c", "b", "a" "3", "2", "1", "В", "Б", "А" <- верхний
            var s2 = Stack.Concat(new Stack("a", "b", "c"), new Stack("1", "2", "3"), new Stack("А", "Б", "В"));

            var deleted = s.Pop();

            // Извлек верхний элемент 'c' Size = 2
            Console.WriteLine($"Извлек верхний элемент '{deleted}' Size = {s.Size}");
            s.Add("d");

            // size = 3, Top = 'd'
            Console.WriteLine($"size = {s.Size}, Top = '{s.Top}'");

            s.Pop();
            s.Pop();
            s.Pop();
            s.Pop();
            s.Pop();
            s.Pop();
            // size = 0, Top = null
            Console.WriteLine($"size = {s.Size}, Top = {(s.Top == null ? "null" : s.Top)}");
            s.Pop();
        }
    }
}
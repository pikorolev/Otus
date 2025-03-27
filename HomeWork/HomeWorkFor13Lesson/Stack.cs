using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkFor13Lesson
{
    public class Stack
    {
        //private readonly List<string> items;
        private StackItem? top = null;
        // Кол-во элементов из Стека
        //public int Size { get { return items.Count; } }
        public int Size;
        // Верхний элемент
        //public string? Top
        //{
        //    get { return items.Count > 0 ? items[items.Count - 1] : null;}
        //}
        public string? Top => top?.Value;
        public Stack(params string[] newItems)
        {
            //items = new List<string>(newItems);
            foreach (string item in newItems)
            {
                Add(item);
            }
        }
        private class StackItem
        {
            public string Value { get; }
            public StackItem? Previous { get; }

            public StackItem(string value, StackItem? previous)
            {
                Value = value;
                Previous = previous;
            }
        }
        // Добавление новой записи
        public void Add(string item)
        {
            //items.Add(item);
            top = new StackItem(item, top);
            Size++;
        }
        // Извлечение вехнего элемента
        public string Pop()
        {
            //if (items.Count == 0)
            //{
            //    throw new InvalidOperationException("Стек пустой");
            //}

            //string topItem = items[items.Count - 1];
            //items.RemoveAt(items.Count - 1);
            //return topItem;
            if (top == null)
            {
                throw new InvalidOperationException("Стек пустой");
            }

            string topValue = top.Value;
            top = top.Previous;
            Size--;
            return topValue;
        }
        public static Stack Concat(params Stack[] stacks)
        {
            Stack resultStack = new Stack();

            // Проходим по каждому стеку
            foreach (var stack in stacks)
            {
                while (stack.Size > 0)
                {
                    resultStack.Add(stack.Pop());
                }
            }

            return resultStack;
        }

    }
}

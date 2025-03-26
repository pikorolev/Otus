using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkFor13Lesson
{
    internal class Stack
    {
        private readonly List<string> items;
        // Кол-во элементов из Стека
        public int Size { get { return items.Count; } }
        // Верхний элемент
        public string? Top
        {
            get { return items.Count > 0 ? items[items.Count - 1] : null;}
        }
        public Stack(params string[] newItems)
        {
            items = new List<string>(newItems);
        }
        // Добавление новой записи
        public void Add(string item)
        {
            items.Add(item);
        }
        // Извлечение вехнего элемента
        public string Pop()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Стек пустой");
            }

            string topItem = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            return topItem;
        }

    }
}

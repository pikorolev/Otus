using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkFor13Lesson
{
    public static class StackExtensions
    {
        // Метод расширения для класса Stack
        public static void Merge(this Stack s1, Stack s2)
        {

            // Извлекаем элементы из s2 и помещаем их во временный стек
            while (s2.Size > 0)
            {   
                s1.Add(s2.Pop());
            }
        }
    }
}

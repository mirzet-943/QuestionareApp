using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTasksProject
{
    class Task1
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            RunTask1();
            Console.ReadLine();
        }

        public static void RunTask1()
        {
            KeyValue[] a = new KeyValue[] { new KeyValue(-1, 3), new KeyValue(1, 3), new KeyValue(2, 5), new KeyValue(3, 8), new KeyValue(9, 1), new KeyValue(5, 4) };

            var repeating = a.Where(s => a.Count(v => v.Value == s.Value) > 1);
            if (repeating != null && repeating.Count() > 0)
            {
                var min = repeating.Min(s => s.Value);
                var nextValue = FindSmallestPositiveHash(a.Select(s => s.Value).ToArray(), min);
                var id = FindSmallestPositiveHash(a.Select(s => s.Id).ToArray(), -1);
                var result = new KeyValue(id, nextValue);
                Console.WriteLine("Next struct is: " + result.Id + "," + result.Value);
            }
        }
        private static int FindSmallestPositiveHash(int[] array, int smallerRepeatingNum)
        {
            List<int> set = new List<int>();
            int maxPositive = 0;
            array.ToList().ForEach(s =>
            {
                if (!set.Contains(s))
                {
                    maxPositive = Math.Max(maxPositive, s);
                    set.Add(s);
                }
            });
            for (int i = 1; i < maxPositive; i++)
            {
                if (!set.Contains(i) && smallerRepeatingNum < i)
                {
                    return i;
                }
            }
            return maxPositive + 1;
        }

        private struct KeyValue
        {
            public int Id { get; set; }
            public int Value { get; set; }
            public KeyValue(int id, int value)
            {
                this.Id = id;
                this.Value = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTasksProject
{
    class Task2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Task 2 solution!");
            RunTask2_LookupPath();
            Console.ReadLine();
        }

        public static void RunTask2_LookupPath()
        {
            object item = new
            {
                a = new { k = "apple", b = new { t = "ass" } }
            };
            var path = "a.l";
            var obj = LookupForProperty(item, path);
            if (obj != null)
                Console.WriteLine("Property found: [" + path + "] = " + obj);
        }

        public static object LookupForProperty(object src, string lookupPath)
        {
            object currentValue = src;
            string RemainingProperty, PreviousProperty = "[source object]";
            string[] Split;
            do
            {
                Split = lookupPath.Split('.');
                RemainingProperty = lookupPath.Contains(".") ? lookupPath.Substring(lookupPath.IndexOf('.') + 1) : "";
                var nextProperty = currentValue.GetType().GetProperty(Split[0]);
                if (nextProperty == null)
                {
                    Console.WriteLine("Sorry we couldn't find property of source: [" + src + "] no property named: \"" + Split[0] + "\" in : " + PreviousProperty);
                    return null;
                }
                currentValue = nextProperty.GetValue(currentValue, null);
                PreviousProperty = Split[0];
                lookupPath = RemainingProperty;
            } while (!string.IsNullOrEmpty(RemainingProperty));
            return currentValue;
        }
    }
}

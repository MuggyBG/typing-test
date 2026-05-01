using System;
using System.Collections.Generic;

namespace TypingTest_Project.Logic
{
    public static class TextHelper
    {
        public static int CountLineErrors(string typed, string target)
        {
            int errors = 0;
            int lengthToCompare = Math.Min(typed.Length, target.Length);

            for (int i = 0; i < lengthToCompare; i++)
            {
                if (typed[i] != target[i])
                {
                    errors++;
                }
            }
            errors += Math.Abs(target.Length - typed.Length);

            return errors;
        }

        public static Queue<string> SplitIntoLines(string content)
        {
            var queue = new Queue<string>();
            var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                queue.Enqueue(line);
            }

            return queue;
        }
    }
}
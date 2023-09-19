using System;
using System.Collections.Generic;
using System.Linq;

namespace PS0
{
    class Program
    {
        /// <summary>
        /// Uses console inputs to build dictionary of alphabetically
        /// sorted strings. Determines how many strings are unique.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string[] inputs = Console.ReadLine().Split(' ');

            int n, k;
            try
            {
                int.TryParse(inputs[0], out n);
                int.TryParse(inputs[1], out k);
            }
            catch (Exception)
            {
                return;
            }

            //Validate inputs
            if (!(1 <= n && n <= 10000))
            {
                return;
            }

            if (!(1 <= k && k <= 1000))
            {
                return;
            }

            Dictionary<string, int> words_dict = new Dictionary<string, int>();

            for (int i = 0; i < n; i++)
            {
                string sorted = StringAlphabetic(Console.ReadLine().Trim().ToLower());

                int count = 0;

                //Key already in dictionary, increment value or insert
                if (words_dict.TryGetValue(sorted, out count)) {
                    words_dict[sorted] = count + 1;
                }
                else
                {
                    words_dict[sorted] = 1;
                }
            }

            int totalUnique = 0;

            //Check for values of 1, as these are unique
            foreach (int val in words_dict.Values)
            {
                if (val == 1)
                {
                    totalUnique++;
                }
            }

            Console.Write(totalUnique);
        }

        /// <summary>
        /// Sorts string alphabetically
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string StringAlphabetic(string s)
        {
            char[] c = s.ToArray();
            Array.Sort(c);
            return new string(c);
        }
    }
}
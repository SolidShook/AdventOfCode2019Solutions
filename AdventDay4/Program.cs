using System;
using System.Text.RegularExpressions;

namespace AdventDay4
{
    class Password
    {
        private string password;

        public bool attempt(string value, int min, int max)
        {
            if (value.Length < 6)
            {
                return false;
            }

            int intValue = int.Parse(value);
            if (intValue < min || intValue > max)
            {
                return false;
            }

            int matchCounts = 0;
            bool decrease = false;
            for (var i = 0; i < value.Length - 1; i++)
            {
                if (value[i] == value[i + 1])
                {
                    matchCounts++;
                }

                if (value[i + 1] < value[i])
                {
                    decrease = true;
                    return false;
                }
            }

            if (matchCounts == 0 || decrease == true)
            {
                return false;
            }

            return true;
        }

        //public bool attempt2(string value, int min, int max)
        //{
        //    if (value.Length < 6)
        //    {
        //        return false;
        //    }

        //    int intValue = int.Parse(value);
        //    if (intValue < min || intValue > max)
        //    {
        //        return false;
        //    }

        //    int matchCounts = 0;
        //    bool decrease = false;
        //    for (var i = 0; i < value.Length - 1; i++)
        //    {
        //        if (value[i + 1] < value[i])
        //        {
        //            decrease = true;
        //            return false;
        //        }

        //        if (i > 0)
        //        {
        //            if (value[i - 1] > value[i])
        //            {
        //                decrease = true;
        //                return false;
        //            }
        //        }

        //        int lookahead = 1;
        //        int match = 0;
        //        int space = value.Length - i;

        //        do
        //        {
        //            int c1 = value[i + lookahead] - '0';
        //            int c2 = value[i] - '0';
        //            if (c1 == c2)
        //            {
        //                match++;
        //                lookahead++;
        //            }
        //            else
        //            {
        //                space = 0;
        //            }
        //        } while (lookahead < space && match > 0);

        //        if (match == 1)
        //        {
        //            matchCounts++;
        //        }

        //        if (match > 1)
        //        {
        //            i = i + lookahead - 1;
        //        }
        //    }

        //    if (matchCounts == 0 || decrease)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public bool part2(string value)
        {
            int matchCount = 0;

            Regex regex = new Regex(@"(1+|2+|3+|4+|5+|6+|7+|8+|9+|0+)");
            foreach (Match match in regex.Matches(value))
            {
                if (match.Length == 2)
                {
                    matchCount++;
                }
            };

            return matchCount > 0;
        }
        public Password()
        {

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Password password = new Password();

            int min = 109165;
            int max = 576723;
            int iterator = min;
            int finds = 0;

            while (iterator <= max)
            {
                string attempt = iterator.ToString();

                if (password.attempt(attempt, min, max))
                {
                    if (password.part2(attempt))
                    {
                        Console.WriteLine("Passed: {0}", iterator.ToString());
                        finds++;
                    }
                }

                iterator++;
            }

            Console.WriteLine("Found passwords count: {0}", finds);

            System.Console.ReadLine();
        }
    }
}

using System;

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
                }
            }

            if (matchCounts == 0 || decrease == true)
            {
                return false;
            }

            return true;
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
                string attempt = min.ToString();

                if (password.attempt(iterator.ToString(), min, max))
                {
                    finds++;
                }

                iterator++;
            }

            Console.WriteLine("Found passwords count: {0}", finds);
            System.Console.ReadLine();
        }
    }
}

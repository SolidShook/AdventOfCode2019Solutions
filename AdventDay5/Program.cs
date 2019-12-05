using System;

namespace AdventDay5
{
    class Program
    {
        public static int loops = 0;
        static int ProcessIntCode(int[] intCode, int cursor)
        {
            loops++;
            int opCode = intCode[cursor];
            int position1 = intCode[cursor + 1];
            int position2 = intCode[cursor + 2];
            int position3 = intCode[cursor + 3];

            if (opCode == 1)
            {
                intCode[position3] = intCode[position1] + intCode[position2];
            }
            else if (opCode == 2)
            {
                intCode[position3] = intCode[position1] * intCode[position2];
            }
            else
            {
                //you fucked up
                return -999;
            }

            if (intCode[cursor + 4] != 99)
            {
                return ProcessIntCode(intCode, cursor + 4);
            }
            else
            {
                return intCode[0];
            }
        }

        private static int[] SearchAnswers(int[] intCode)
        {

            int target = 19690720;
            int answer1 = 0;
            int answer2 = 0;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    int[] intCodeCopy = new int[intCode.Length];
                    intCode.CopyTo(intCodeCopy, 0);

                    intCodeCopy[1] = i;
                    intCodeCopy[2] = j;

                    if (ProcessIntCode(intCodeCopy, 0) == target)
                    {
                        answer1 = i;
                        answer2 = j;
                    }
                }
            }

            return new int[] { answer1, answer2 };
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int counter = 0;
            string line;

            System.IO.StreamReader file =
                new System.IO.StreamReader("../../../data.txt");

            string str = file.ReadToEnd();
            file.Close();

            int[] intCode = Array.ConvertAll(str.Split(','), int.Parse);

            ////question 1 answer
            int result = ProcessIntCode(intCode, 0);

            System.Console.WriteLine("The answer is {0}", result);


            //question 2;
            int[] answers = SearchAnswers(Array.ConvertAll(str.Split(','), int.Parse));

            System.Console.WriteLine("The answer is {0}, {1}", answers[0], answers[1]);

            // Suspend the screen.  
            System.Console.ReadLine();
        }
    }
}

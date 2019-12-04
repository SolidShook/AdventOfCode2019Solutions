using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AdventDay2
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
            } else if (opCode == 2)
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

            int result = ProcessIntCode(intCode, 0);

            System.Console.WriteLine("The answer is {0}", result);
            // Suspend the screen.  
            System.Console.ReadLine();
        }
    }
}

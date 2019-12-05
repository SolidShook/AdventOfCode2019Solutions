using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AdventDay5
{
    enum Modes
    {
        Param,
        Immed
    }

    class Parameter
    {
        private Modes Mode;
        public int Value;

        public int GetResult(int[] intCode)
        {
            if (Mode == Modes.Param)
            {
                return intCode[Value];
            }

            if (Mode == Modes.Immed)
            {
                return Value;
            }

            return -999;
        }

        public Parameter(Modes mode, int value)
        {
            Mode = mode;
            Value = value;
        }
    }

    class Program
    {
        public static int loops = 0;
        static int ProcessIntCode(int[] intCode, int cursor)
        {
            loops++;
            int opCode = intCode[cursor];

            int steps = 0;
            int op = 0;

            string opInstruction = opCode.ToString();
            List<Parameter> pars = new List<Parameter>();

            if (opInstruction.Length > 1)
            {
                op = opInstruction[^1] - '0';

                if (op == 1 || op == 2)
                {
                    steps = 3;
                }

                if (op == 3 || op == 4)
                {
                    steps = 1;
                }

                for (int i = 0; i < steps; i++)
                {
                    int index = opInstruction.Length - 3 - i;
                    int modeValue = index < 0 ? 0 : opInstruction[index] - '0';

                    Modes mode = (Modes)(modeValue);
                    Parameter param = new Parameter(mode, intCode[cursor + i + 1]);
                    pars.Add(param);
                }

                int x = 0;
            }
            else
            {
                op = opCode;
            }

            if (op == 1)
            {
                intCode[pars[2].Value] = pars[0].GetResult(intCode) + pars[1].GetResult(intCode);
            }
            else if (op == 2)
            {
                intCode[pars[2].Value] = pars[0].GetResult(intCode) * pars[1].GetResult(intCode);
            } 
            else if (op == 3)
            {
            }
            else if (op == 4)
            {
            }
            else
            {
                //you fucked up
                return -999;
            }

            if (intCode[cursor + 4] != 99)
            {
                return ProcessIntCode(intCode, cursor + steps + 1);
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

        static void Main()
        {
            Console.WriteLine("Hello World!");

            System.IO.StreamReader file =
                new System.IO.StreamReader("../../../data.txt");

            string str = file.ReadToEnd();
            file.Close();

            int[] intCode = Array.ConvertAll(str.Split(','), int.Parse);

            ////question 1 answer
            int result = ProcessIntCode(intCode, 0);

            System.Console.WriteLine("The answer is {0}", result);

            // Suspend the screen.  
            System.Console.ReadLine();
        }
    }
}

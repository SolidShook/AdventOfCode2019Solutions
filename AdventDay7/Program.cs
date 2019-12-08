using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AdventDay7
{
    public enum Modes
    {
        Param,
        Immed
    }

    enum Instructions
    {
        ADD,
        MULT,
        INPUT,
        OUTPUT,
        JUMP_IF_TRUE,
        JUMP_IF_FALSE,
        LESS_THAN,
        EQUALS,
        HALT,
        ERROR
    }

    class Operator
    {
        public int ParamCount;
        public Instructions Instruction;

        public Operator(Instructions instruction, int paramCount, opMethod operation)
        {
            ParamCount = paramCount;
            Instruction = instruction;
        }
    }

    public class Parameter
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

        public void SetMode(int mode)
        {
            Mode = (Modes)mode;
        }

        public Parameter(Modes mode, int value)
        {
            Mode = mode;
            Value = value;
        }

        public Parameter(int value)
        {
            Mode = Modes.Param;
            Value = value;
        }
    }

    //int returns new cursor position
    public delegate int opMethod(int[] intCode, List<Parameter> pars, int cursor);

    class Operators
    {
        static int Sum(int[] intCode, List<Parameter> pars, int cursor)
        {
            intCode[pars[2].Value] = pars[0].GetResult(intCode) + pars[1].GetResult(intCode);

            return cursor + pars.Count;
        }

        static int Mult(int[] intCode, List<Parameter> pars, int cursor)
        {
            intCode[pars[2].Value] = pars[0].GetResult(intCode) * pars[1].GetResult(intCode);

            return cursor + pars.Count + 1;
        }

        static int Input(int[] intCode, List<Parameter> pars, int cursor)
        {
            System.Console.WriteLine("INPUT");
            string line = Console.ReadLine();
            int a = int.Parse(line);
            System.Console.WriteLine("YOU GAVE {0}", a);
            intCode[pars[0].Value] = a;

            return cursor + 2; 
        }

        public static readonly Dictionary<int, Operator> Ops
            = new Dictionary<int, Operator>
        {
            { 1, new Operator(Instructions.ADD, 3, new opMethod(Sum)) },
            { 2, new Operator(Instructions.MULT, 3, new opMethod(Mult)) },
            { 3, new Operator(Instructions.INPUT, 1) },
            { 4, new Operator(Instructions.OUTPUT, 1) },
            { 5, new Operator(Instructions.JUMP_IF_TRUE, 2) },
            { 6, new Operator(Instructions.JUMP_IF_FALSE, 2) },
            { 7, new Operator(Instructions.LESS_THAN, 3) },
            { 8, new Operator(Instructions.EQUALS, 3) },
            { 99, new Operator(Instructions.HALT, 0) }
        }; 
    }

    class IntCodeParser
    {
        private List<Parameter> GetParameters(int[] intCode, int cursor, Operator oper, string modes)
        {
            List<Parameter> pars = new List<Parameter>();

            for (int i = 0; i < oper.ParamCount; i++)
            {
                pars.Add(new Parameter(intCode[cursor + i + 1]));
            }

            if (modes != "")
            {
                int it = 0;
                for (int x = modes.Length - 1; x >= 0; x--)
                {
                    pars[it].SetMode(modes[x] - '0');
                    it++;
                }
            }

            return pars;
        }
        public int ProcessIntCode(int[] intCode, int cursor)
        {
            int opCode = intCode[cursor];

            string opInstruction = opCode.ToString();
            int instr;
            string modes = "";

            if (opInstruction.Length == 1)
            {
                instr = int.Parse(opInstruction);
            }
            else
            {
                instr = int.Parse(opInstruction.Substring(opInstruction.Length - 2));
                modes = opInstruction.Substring(0, opInstruction.Length - 2);
            }

            Operator oper = Operators.Ops[instr];

            List<Parameter> pars = GetParameters(intCode, cursor, oper, modes);

            int newCursorPos = cursor + oper.ParamCount + 1;

            switch (instruction)
            {
                case Instructions.ADD:
                    intCode[pars[2].Value] = pars[0].GetResult(intCode) + pars[1].GetResult(intCode);
                    break;
                case Instructions.MULT:
                    intCode[pars[2].Value] = pars[0].GetResult(intCode) * pars[1].GetResult(intCode);
                    break;
                case Instructions.INPUT:
                    System.Console.WriteLine("INPUT");
                    string line = Console.ReadLine();
                    int a = int.Parse(line);
                    System.Console.WriteLine("YOU GAVE {0}", a);
                    intCode[pars[0].Value] = a;
                    break;
                case Instructions.OUTPUT:
                    System.Console.WriteLine("OUTPUT {0}", pars[0].GetResult(intCode));
                    break;
                case Instructions.JUMP_IF_TRUE:
                    if (pars[0].GetResult(intCode) != 0)
                    {
                        newCursorPos = pars[1].GetResult(intCode);
                    }
                    break;
                case Instructions.JUMP_IF_FALSE:
                    if (pars[0].GetResult(intCode) == 0)
                    {
                        newCursorPos = pars[1].GetResult(intCode);
                    }
                    break;
                case Instructions.LESS_THAN:
                    if (pars[0].GetResult(intCode) < pars[1].GetResult(intCode))
                    {
                        intCode[pars[2].Value] = 1;
                    }
                    else
                    {
                        int test = pars[2].GetResult(intCode);
                        intCode[pars[2].Value] = 0;
                    }
                    break;
                case Instructions.EQUALS:
                    if (pars[0].GetResult(intCode) == pars[1].GetResult(intCode))
                    {
                        intCode[pars[2].Value] = 1;
                    }
                    else
                    {
                        intCode[pars[2].Value] = 0;
                    }
                    break;
                case Instructions.HALT:
                    System.Console.WriteLine("HALT");
                    return intCode[0];
                    break;
                case Instructions.ERROR:
                    return -999;
            }

            return ProcessIntCode(intCode, newCursorPos);
        }
    }

    class Amplifier
    {
        int PhaseSetting;

        public Amplifier(int phaseSetting)
        {
            PhaseSetting = phaseSetting;
        }
    }
    class Program
    {
        static void Main()
        {
            System.Console.WriteLine("BEGIN");
            System.IO.StreamReader file =
                new System.IO.StreamReader("../../../data.txt");

            string str = file.ReadToEnd();
            file.Close();

            IntCodeParser parser;
            List<Amplifier> amplifiers;

            while (true)
            {
                int[] intCode = Array.ConvertAll(str.Split(','), int.Parse);
                parser = new IntCodeParser();
                amplifiers = new List<Amplifier>();

                string settings = "012345";

                foreach(char setting in settings)
                {
                    amplifiers.Add(new Amplifier(int.Parse(setting.ToString())));
                }

                ////question 1 answer
                int result = parser.ProcessIntCode(intCode, 0);

                //Console.WriteLine("[{0}]", string.Join(", ", intCode));

            }
        }
    }
}

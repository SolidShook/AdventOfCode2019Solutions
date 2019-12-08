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

        public Operator(Instructions instruction, int paramCount)
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

    class Operators
    {
        public static readonly Dictionary<int, Operator> Ops = new Dictionary<int, Operator>
        {
            { 1, new Operator(Instructions.ADD, 3) },
            { 2, new Operator(Instructions.MULT, 3) },
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
        public int LastOutput;
        private List<Parameter> GetParameters(int[] intCode, int cursor, Operator oper, string opCode)
        {
            List<Parameter> pars = new List<Parameter>();

            string modes = "";

            if (intCode[cursor].ToString().Length > 2)
            {
                modes = opCode.Substring(0, opCode.Length - 2);
            }

            for (int i = 0; i < oper.ParamCount; i++)
            {
                pars.Add(new Parameter(intCode[cursor + i + 1]));
            }

            if (modes != "")
            {
                int it = 0;
                for (int x = modes.Length - 1; x >= 0; x--)
                {
                    var test = modes[x] -'0';
                    pars[it].SetMode(modes[x] - '0');
                    it++;
                }
            }

            return pars;
        }

        private Operator GetOperator(string opCode)
        {
            if (opCode.Length > 2)
            {
                return Operators.Ops[int.Parse(opCode.Substring(opCode.Length - 2))];
            }

            return Operators.Ops[int.Parse(opCode)];
        }

        protected virtual void InputCommand(int[] intCode, List<Parameter> pars)
        {
            System.Console.WriteLine("INPUT");
            string line = Console.ReadLine();
            int a = int.Parse(line);
            System.Console.WriteLine("YOU GAVE {0}", a);
            intCode[pars[0].Value] = a;
        }

        public int ProcessIntCode(int[] intCode, int cursor)
        {
            string opCode = intCode[cursor].ToString();
            Operator oper = GetOperator(opCode);
            List<Parameter> pars = GetParameters(intCode, cursor, oper, opCode);

            int newCursorPos = cursor + oper.ParamCount + 1;

            switch (oper.Instruction)
            {
                case Instructions.ADD:
                    intCode[pars[2].Value] = pars[0].GetResult(intCode) + pars[1].GetResult(intCode);
                    break;
                case Instructions.MULT:
                    intCode[pars[2].Value] = pars[0].GetResult(intCode) * pars[1].GetResult(intCode);
                    break;
                case Instructions.INPUT:
                    InputCommand(intCode, pars);
                    break;
                case Instructions.OUTPUT:
                    LastOutput = pars[0].GetResult(intCode);
                    //System.Console.WriteLine("OUTPUT {0}", LastOutput);
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
                    //System.Console.WriteLine("HALT");
                    return LastOutput;
            }

            return ProcessIntCode(intCode, newCursorPos);
        }
    }

    class IntCodeParserSetInput : IntCodeParser
    {
        int inputAddress;
        List<int> Inputs;
        protected override void InputCommand(int[] intCode, List<Parameter> pars)
        {
            intCode[pars[0].Value] = Inputs[inputAddress];
            inputAddress++;
        }

        public int process(int[] intCode, List<int> inputs)
        {
            inputAddress = 0;
            Inputs = inputs;
            return ProcessIntCode(intCode, 0);
        }
    }
    class Amplifier
    {
        public int PhaseSetting;
        public int Input;

        public List<int> GetInputs ()
        {
            return new List<int> { PhaseSetting, Input };
        }
        public Amplifier(int phaseSetting, int input)
        {
            PhaseSetting = phaseSetting;
            Input = input;
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

            while (true)
            {
                int[] intCode = Array.ConvertAll(str.Split(','), int.Parse);


                IntCodeParser defaultParser = new IntCodeParser();
                IntCodeParserSetInput parser = new IntCodeParserSetInput();

                parser = new IntCodeParserSetInput();
                List<Amplifier> amplifiers = new List<Amplifier>();

                string settings = "31240";

                int input = 0;
                foreach (char setting in settings)
                {
                    Amplifier amp = new Amplifier(int.Parse(setting.ToString()), input);
                    
                    input = parser.process(intCode, amp.GetInputs());
                    int x = 0;
                }

                Console.ReadLine();
            }
        }
    }
}

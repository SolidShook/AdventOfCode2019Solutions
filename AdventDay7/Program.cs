using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected int[] _intCode;
        public int Cursor;

        private List<Parameter> GetParameters(Operator oper, string opCode)
        {
            List<Parameter> pars = new List<Parameter>();

            string modes = "";

            if (_intCode[Cursor].ToString().Length > 2)
            {
                modes = opCode.Substring(0, opCode.Length - 2);
            }

            for (int i = 0; i < oper.ParamCount; i++)
            {
                pars.Add(new Parameter(_intCode[Cursor + i + 1]));
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

        private Operator GetOperator(string opCode)
        {
            if (opCode.Length > 2)
            {
                return Operators.Ops[int.Parse(opCode.Substring(opCode.Length - 2))];
            }

            return Operators.Ops[int.Parse(opCode)];
        }

        protected virtual void InputCommand(List<Parameter> pars)
        {
            System.Console.WriteLine("INPUT");
            string line = Console.ReadLine();
            int a = int.Parse(line);
            System.Console.WriteLine("YOU GAVE {0}", a);
            _intCode[pars[0].Value] = a;
        }

        protected virtual void OutputCommand(List<Parameter> pars)
        {
            LastOutput = pars[0].GetResult(_intCode);
        }

        public int ProcessIntCode()
        {
            string opCode = _intCode[Cursor].ToString();
            Operator oper = GetOperator(opCode);
            List<Parameter> pars = GetParameters(oper, opCode);
            int cursorDifference = oper.ParamCount + 1;
            Cursor += cursorDifference;

            switch (oper.Instruction)
            {
                case Instructions.ADD:
                    _intCode[pars[2].Value] = pars[0].GetResult(_intCode) + pars[1].GetResult(_intCode);
                    break;
                case Instructions.MULT:
                    _intCode[pars[2].Value] = pars[0].GetResult(_intCode) * pars[1].GetResult(_intCode);
                    break;
                case Instructions.INPUT:
                    InputCommand(pars);
                    break;
                case Instructions.OUTPUT:
                    OutputCommand(pars);
                    LastOutput = pars[0].GetResult(_intCode);
                    break;
                case Instructions.JUMP_IF_TRUE:
                    if (pars[0].GetResult(_intCode) != 0)
                    {
                        Cursor = pars[1].GetResult(_intCode);
                    }
                    break;
                case Instructions.JUMP_IF_FALSE:
                    if (pars[0].GetResult(_intCode) == 0)
                    {
                        Cursor = pars[1].GetResult(_intCode);
                    }
                    break;
                case Instructions.LESS_THAN:
                    if (pars[0].GetResult(_intCode) < pars[1].GetResult(_intCode))
                    {
                        _intCode[pars[2].Value] = 1;
                    }
                    else
                    {
                        _intCode[pars[2].Value] = 0;
                    }
                    break;
                case Instructions.EQUALS:
                    if (pars[0].GetResult(_intCode) == pars[1].GetResult(_intCode))
                    {
                        _intCode[pars[2].Value] = 1;
                    }
                    else
                    {
                        _intCode[pars[2].Value] = 0;
                    }
                    break;
                case Instructions.HALT:
                    return LastOutput;
            }

            return ProcessIntCode();
        }

        public IntCodeParser(int[] intCode)
        {
            Cursor = 0;
            _intCode = intCode;
        }
    }

    class IntCodeParserSetInput : IntCodeParser 
    {
        public Queue<int> Inputs;
        protected override void InputCommand(List<Parameter> pars)
        {
            int input = Inputs.Dequeue();
            _intCode[pars[0].Value] = input;
        }

        public int Process(Queue<int> inputs)
        {
            Inputs = inputs;
            return ProcessIntCode();
        }

        public IntCodeParserSetInput(int[] intCode) : base(intCode)
        {
        }
    }

    class IntCodeParserLooper : IntCodeParserSetInput
    {
        public IntCodeParserLooper(int[] intCode) : base(intCode)
        {
        }

        protected override void OutputCommand(List<Parameter> pars)
        {
            LastOutput = pars[0].GetResult(_intCode);
            Inputs.Enqueue(LastOutput);
        }
    }

    class Amplifier
    {
        public Queue<int> Inputs;

        public Queue<int> GetInputs ()
        {
            return Inputs;
        }
        public Amplifier(Queue<int> inputs)
        {
            Inputs = inputs;
        }
    }

    class NoLooper
    {
        public void Process(int[] intCode)
        {
            int? highest = null;
            string combo = "";

            List<string> settingsCollection = Permutations.GetPermutations("01234");
            IntCodeParserSetInput parser = new IntCodeParserSetInput(intCode);

            foreach (string settings in settingsCollection)
            {
                List<Amplifier> amplifiers = new List<Amplifier>();

                int input = 0;
                foreach (char setting in settings)
                {
                    Queue<int> inputs = new Queue<int>();
                    inputs.Enqueue(int.Parse(setting.ToString()));
                    inputs.Enqueue(input);
                    Amplifier amp = new Amplifier(inputs);
                    input = parser.Process(amp.GetInputs());
                }

                if (highest == null || input > highest)
                {
                    highest = input;
                    combo = settings;
                }
            }

            Console.WriteLine("{0} made {1}", combo, highest);
        }
    }

    class Looper
    {
        public void Process(int[] intCode)
        {
            int? highest = null;
            string combo = "";

            List<string> settingsCollection = Permutations.GetPermutations("56789");
            IntCodeParserLooper parser = new IntCodeParserLooper(intCode);

            foreach (string settings in settingsCollection)
            {
                List<Amplifier> amplifiers = new List<Amplifier>();

                int input = 0;
                foreach (char setting in settings)
                {
                    Queue<int> inputs = new Queue<int>();
                    inputs.Enqueue(input);
                    inputs.Enqueue(int.Parse(setting.ToString()));
                    Amplifier amp = new Amplifier(inputs);
                    input = parser.Process(amp.GetInputs());
                }

                if (highest == null || input > highest)
                {
                    highest = input;
                    combo = settings;
                }
            }

            Console.WriteLine("{0} made {1}", combo, highest);
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
                NoLooper noLooper = new NoLooper();
                noLooper.Process(Array.ConvertAll(str.Split(','), int.Parse));

                //Looper looper = new Looper();
                //looper.Process(Array.ConvertAll(str.Split(','), int.Parse));

                Console.ReadLine();
            }
        }
    }
}

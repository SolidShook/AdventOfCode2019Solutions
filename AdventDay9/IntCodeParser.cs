using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Numerics;
namespace AdventDay9
{
    #region constants
    public enum Modes
    {
        Param = 0,
        Immed = 1,
        Rel = 2
    }

    enum Instructions
    {
        Add,
        Mult,
        Input,
        Output,
        JumpIfTrue,
        JumpIfFalse,
        LessThan,
        Equals,
        SetRel,
        Halt,
        Error
    }

    class Operators
    {
        public static readonly Dictionary<int, Operator> Ops = new Dictionary<int, Operator>
        {
            { 1, new Operator(Instructions.Add, 3) },
            { 2, new Operator(Instructions.Mult, 3) },
            { 3, new Operator(Instructions.Input, 1) },
            { 4, new Operator(Instructions.Output, 1) },
            { 5, new Operator(Instructions.JumpIfTrue, 2) },
            { 6, new Operator(Instructions.JumpIfFalse, 2) },
            { 7, new Operator(Instructions.LessThan, 3) },
            { 8, new Operator(Instructions.Equals, 3) },
            { 9, new Operator(Instructions.SetRel, 1) },
            { 99, new Operator(Instructions.Halt, 0) }
        };
    }
    #endregion


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
        public BigInteger Value;
        public int RelativeBase;

        public BigInteger GetResult(IntCode intCode)
        {
            if (Mode == Modes.Param)
            {
                return intCode.GetAddress(Value);
            }

            if (Mode == Modes.Immed)
            {
                return Value;
            }

            if (Mode == Modes.Rel)
            {
                int newAddress = RelativeBase + (int)Value;
                return intCode.GetAddress(RelativeBase + Value);
            }

            return -999;
        }

        public BigInteger GetLiteral(IntCode intCode)
        {
            if (Mode == Modes.Param || Mode == Modes.Immed)
            {
                return Value;
            }
            else
            {
                return RelativeBase + Value;
            }
        }

        public void SetMode(int mode)
        {
            Mode = (Modes)mode;
        }

        public Parameter(Modes mode, BigInteger value, int relativeBase)
        {
            Mode = mode;
            Value = value;
            RelativeBase = relativeBase;
        }

        public Parameter(Modes mode, int value)
        {
            Mode = mode;
            Value = value;
        }

        public Parameter(BigInteger value, int relativeBase)
        {
            Mode = Modes.Param;
            Value = value;
            RelativeBase = relativeBase;
        }
    }

    public class IntCode
    {
        private List<BigInteger> _intCode;

        private void FillToAddress(BigInteger address)
        {
            if (_intCode.Count < address + 1)
            {
                while (_intCode.Count < address + 1)
                {
                    _intCode.Add(0);
                }
            }
        }

        public BigInteger GetAddress(BigInteger address)
        {
            FillToAddress(address);

            return _intCode[(int)address];
        }

        public void SetAddress(Parameter target, BigInteger value)
        {
            int address = (int)target.GetLiteral(this);
            FillToAddress(address);

            _intCode[address] = value;
        }

        public IntCode(List<BigInteger> intCode)
        {
            _intCode = intCode;
        }
    }


    #region intCodeParser
    class IntCodeParser
    {
        public BigInteger LastOutput;
        protected IntCode IntC;
        public BigInteger Cursor;
        public int RelativeBase;

        private List<Parameter> GetParameters(Operator oper, string opCode)
        {
            List<Parameter> pars = new List<Parameter>();

            string modes = "";

            if (IntC.GetAddress(Cursor).ToString().Length > 2)
            {
                modes = opCode.Substring(0, opCode.Length - 2);
            }

            for (int i = 0; i < oper.ParamCount; i++)
            {
                pars.Add(new Parameter(IntC.GetAddress(Cursor + i + 1), RelativeBase));
            }

            if (modes != "")
            {
                int it = 0;
                for (int x = modes.Length - 1; x >= 0; x--)
                {
                    int mode = modes[x];

                    if (oper.Instruction == Instructions.Equals)
                    {
                        mode = 0;
                    }
                    
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
            System.Console.WriteLine("Input");
            string line = Console.ReadLine();
            int a = int.Parse(line);
            System.Console.WriteLine("YOU GAVE {0}", a);
            IntC.SetAddress(pars[0], a);
        }

        protected virtual void OutputCommand(List<Parameter> pars)
        {
            Console.WriteLine(pars[0].GetResult(IntC));
            LastOutput = pars[0].GetResult(IntC);
        }

        public Instructions ProcessIntCode(bool loop)
        {
            string opCode = IntC.GetAddress(Cursor).ToString();

            Operator oper = GetOperator(opCode);
            List<Parameter> pars = GetParameters(oper, opCode);
            int cursorDifference = oper.ParamCount + 1;
            Cursor += cursorDifference;

            switch (oper.Instruction)
            {
                case Instructions.Add:
                    IntC.SetAddress(pars[2], pars[0].GetResult(IntC) + pars[1].GetResult(IntC));
                    break;
                case Instructions.Mult:
                    IntC.SetAddress(pars[2], pars[0].GetResult(IntC) * pars[1].GetResult(IntC));
                    break;
                case Instructions.Input:
                    InputCommand(pars);
                    break;
                case Instructions.Output:
                    OutputCommand(pars);
                    break;
                case Instructions.JumpIfTrue:
                    if (pars[0].GetResult(IntC) != 0)
                    {
                        Cursor = pars[1].GetResult(IntC);
                    }
                    break;
                case Instructions.JumpIfFalse:
                    if (pars[0].GetResult(IntC) == 0)
                    {
                        Cursor = pars[1].GetResult(IntC);
                    }
                    break;
                case Instructions.LessThan:
                    if (pars[0].GetResult(IntC) < pars[1].GetResult(IntC))
                    {
                        IntC.SetAddress(pars[2], 1);
                    }
                    else
                    {
                        IntC.SetAddress(pars[2], 0);
                    }
                    break;
                case Instructions.Equals:
                    if (pars[0].GetResult(IntC) == pars[1].GetResult(IntC))
                    {
                        IntC.SetAddress(pars[2], 1);
                    }
                    else
                    {
                        IntC.SetAddress(pars[2], 0);
                    }
                    break;
                case Instructions.SetRel:
                    RelativeBase += (int)pars[0].GetResult(IntC);
                    break;
                case Instructions.Halt:
                    return oper.Instruction;
            }

            if (loop)
            {
                return ProcessIntCode(loop);
            }

            else return oper.Instruction;
        }

        public IntCodeParser(List<BigInteger> intCode)
        {
            Cursor = 0;
            IntC = new IntCode(new List<BigInteger>(intCode));
        }
    }

    class IntCodeParserSetInput : IntCodeParser
    {
        public Queue<int> Inputs;
        protected override void InputCommand(List<Parameter> pars)
        {
            int input = Inputs.Dequeue();
            IntC.SetAddress(pars[0], input);
        }

        public void Process(Queue<int> inputs)
        {
            Inputs = inputs;
            ProcessIntCode(true);
        }

        public IntCodeParserSetInput(List<BigInteger> intC) : base(intC)
        {
        }
    }

    class IntCodeParserLooper : IntCodeParserSetInput
    {
        public bool Paused = false;
        public IntCodeParserLooper(List<BigInteger> intC) : base(intC)
        {
        }

        protected override void OutputCommand(List<Parameter> pars)
        {
            LastOutput = pars[0].GetResult(IntC);

            Paused = true;
        }
    }

    #endregion
}

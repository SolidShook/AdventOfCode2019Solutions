using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic.CompilerServices;

namespace AdventDay11
{
    class Panel
    {
        public readonly Point Pos;

        //cba parsing, just keep everything a big int for compatibility lol
        public BigInteger Colour;

        public void Paint(BigInteger colour)
        {
            Colour = colour;
        }

        public Panel(int x, int y)
        {
            Pos = new Point(x, y);
            Colour = 0;
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Panel p = (Panel)obj;
                return (Pos.X == p.Pos.X) && (Pos.Y == p.Pos.Y);
            }
        }

        public override int GetHashCode()
        {
            return (Pos.X << 2) ^ Pos.Y;
        }
    }

    class G
    {
        public static readonly Dictionary<string, Point> Directions = new Dictionary<string, Point>
        {
            { "UP", new Point(0, -1)},
            { "RIGHT", new Point(1, 0)},
            { "DOWN", new Point(0, 1)},
            { "LEFT", new Point(-1, 0)},
        };
    }
    class Robot
    {
        private IntCodeProcessorLooper _cpu;
        private Dictionary<Point, Panel> _panels;
        private Point Pos;

        private void AddPanel(int x, int y)
        {
            _panels.Add(new Point(x, y), new Panel(x, y));
        }

        public BigInteger GetCurrentPanelColour()
        {
            BigInteger colour = 55;
            if (_panels.ContainsKey(new Point(Pos.X, Pos.Y)))
            {
                colour = _panels[new Point(Pos.X, Pos.Y)].Colour;
            }

            return colour;
        }

        public void Execute()
        {
            Instructions lastInstruction = new Instructions();

            while (lastInstruction != Instructions.Halt)
            {
                while (!_cpu.Paused)
                {
                    lastInstruction = _cpu.Step();
                }

                int test = 0;
            }

            int x = 0;
        }

        public Robot(List<BigInteger> intCode)
        {
            _panels = new Dictionary<Point, Panel>();
            Pos = new Point(0,0);
            AddPanel(0, 0);

            _cpu = new IntCodeProcessorLooper(intCode);
            _cpu.AddInput(GetCurrentPanelColour());
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Robot rob = new Robot(IntCode.GetBigInts("../../../data.txt"));

            Console.WriteLine("Hello World!");

            rob.Execute();
        }
    }
}

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
            Colour = 1;
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
        public enum Directions
        {
            Up,
            Right,
            Down,
            Left
        }

        public static readonly Dictionary<Directions, Point> DirectionValues = new Dictionary<Directions, Point>
        {
            { Directions.Up, new Point(0, -1)},
            { Directions.Right, new Point(1, 0)},
            { Directions.Down, new Point(0, 1)},
            { Directions.Left, new Point(-1, 0)},
        };
    }
    class Robot
    {
        private IntCodeProcessorLooper _cpu;
        private Dictionary<(int, int), Panel> _panels;
        private Point Pos;
        private G.Directions Direction;

        private void AddPanel(int x, int y)
        {
            _panels.Add((x, y), new Panel(x, y));
        }

        private void Move()
        {
            Pos.X += G.DirectionValues[Direction].X;
            Pos.Y += G.DirectionValues[Direction].Y;
        }

        private void TurnRight()
        {
            if ((int) Direction >= G.DirectionValues.Count - 1)
            {
                Direction = (G.Directions) 0;
            }
            else
            {
                Direction = (G.Directions) (int) Direction + 1;
            }

            Move();
        }

        private void TurnLeft()
        {
            if ((int)Direction  == 0)
            {
                Direction = (G.Directions)(int)G.DirectionValues.Count - 1;
            }
            else
            {
                Direction = (G.Directions)(int)Direction - 1;
            }

            Move();
        }

        public BigInteger GetCurrentPanelColour()
        {
            if (!_panels.ContainsKey((Pos.X, Pos.Y)))
            {
                AddPanel(Pos.X, Pos.Y);
            }

            return _panels[(Pos.X, Pos.Y)].Colour;
        }

        public void Paint(BigInteger value)
        {
            if (!_panels.ContainsKey((Pos.X, Pos.Y)))
            {
                AddPanel(Pos.X, Pos.Y);
            }

            _panels[(Pos.X, Pos.Y)].Colour = value;
        }

        public void Execute()
        {
            Instructions lastInstruction = new Instructions();
            bool painted = false;

            while (lastInstruction != Instructions.Halt)
            {
                while (!_cpu.Paused && lastInstruction != Instructions.Halt)
                {
                    lastInstruction = _cpu.Step();
                }

                if (!painted)
                {
                    Paint(_cpu.LastOutput);
                    painted = true;
                }
                else
                {
                    if (_cpu.LastOutput == 1)
                    {
                        TurnRight();
                    }
                    if (_cpu.LastOutput == 0)
                    {
                        TurnLeft();
                    }
                    _cpu.AddInput(GetCurrentPanelColour());
                    painted = false;
                }

                _cpu.Paused = false;
            }

            Console.WriteLine("Number of Panels {0}", _panels.Count);

            int x = 0;
        }

        public Robot(List<BigInteger> intCode)
        {
            _panels = new Dictionary<(int, int), Panel>();
            Pos = new Point(0,0);
            AddPanel(0, 0);
            Direction = G.Directions.Up;
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

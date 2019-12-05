using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventDay3
{
    class Point
    {
        public int x;
        public int y;

        public int getManhattanDistance()
        {
            return Math.Abs(x) + Math.Abs(y);
        }

        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    class Line
    {
        public char dir;
        public char dir2d;

        public int steps;
        public Point p1;
        public Point p2;

        private Point calculateEndPoint()
        {
            int x = 0;
            int y = 0;

            if (dir == 'R')
            {
                x = p1.x + steps;
                y = p1.y;
            }
            if (dir == 'L')
            {
                x = p1.x - steps;
                y = p1.y;
            }
            if (dir == 'U')
            {
                x = p1.x;
                y = p1.y + steps;
            }
            if (dir == 'D')
            {
                x = p1.x;
                y = p1.y - steps;
            }

            return new Point(x, y);
        }

        public Line(char _dir, int _steps, int _originX, int _originY)
        {
            dir = _dir;
            steps = _steps;
            p1 = new Point(_originX, _originY);

            if (dir == 'R' || dir == 'L')
            {
                dir2d = 'X';
            }
            else
            {
                dir2d = 'Y';
            }

            p2 = calculateEndPoint();
        }
    }

    class Wire
    {
        public readonly List<Line> lines = new List<Line>();

        public Wire(string[] instructions)
        {
            int x = 0;
            int y = 0;

            foreach (string instruction in instructions)
            {
                char dir = (char)Regex.Match(instruction, @"\D+").Value[0];
                int steps = int.Parse(Regex.Match(instruction, @"\d+").Value);
                Line newLine = new Line(dir, steps, x, y);
                lines.Add(newLine);
                x = newLine.p2.x;
                y = newLine.p2.y;
            }
        }
    }
}

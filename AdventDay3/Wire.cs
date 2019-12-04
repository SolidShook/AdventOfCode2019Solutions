using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventDay3
{
    class Line
    {
        public Vector2 a;
        public Vector2 b;

        public Line(Vector2 pointA, Vector2 pointB)
        {
            a = pointA;
            b = pointB;
        }
    }

    class Movement
    {
        public char dir;
        public int steps;

        public Movement(char _dir, int _steps)
        {
            dir = _dir;
            steps = _steps;
        }
    }

    class Wire
    {
        public List<Line> lines = new List<Line>();
        private List<Movement> movements = new List<Movement>();
        private Vector2 currentPosition;
        private void CreateLines()
        {
            foreach (Movement move in movements)
            {
                Vector2 startPoint = new Vector2(currentPosition.X, currentPosition.Y);

                int xScale = 0;
                int yScale = 0;

                if (move.dir == 'R')
                {
                    xScale = 1;
                } else if (move.dir == 'L')
                {
                    xScale = -1;
                } else if (move.dir == 'U')
                {
                    yScale = 1;
                } else if (move.dir == 'D')
                {
                    yScale = -1;
                }

                Vector2 endPoint = new Vector2(currentPosition.X + (move.steps * xScale), currentPosition.Y + (move.steps * yScale));

                Line line = new Line(startPoint, endPoint);
                lines.Add(line);

                currentPosition.X = endPoint.X;
                currentPosition.Y = endPoint.Y;
            }
        }

        private Vector2? CheckIntersection(Line line1, Line line2)
        {
            float x1 = line1.a.X;
            float x2 = line1.b.X;
            float x3 = line2.a.X;
            float x4 = line2.b.X;
            float y1 = line1.a.Y;
            float y2 = line1.b.Y;
            float y3 = line2.a.Y;
            float y4 = line2.b.Y;


            float det12 = x1 * y2 - y1 * x2;
            float det34 = x3 * y4 - y3 * x4;
            float den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (den == 0)
            {
                return null;
            }

            float x = (det12 * (x3 - x4) - (x1 - x2) * det34) / den;
            float y = (det12 * (y3 - x4) - (y1 - y2) * det34) / den;

            return new Vector2(x, y);
        }

        public List<Vector2> CalculateIntersections(List<Line> otherLines)
        {
            List<Vector2> intersections = new List<Vector2>();

            foreach (Line line in lines)
            {
                foreach (Line otherLine in lines)
                {
                    Vector2? intersection = CheckIntersection(line, otherLine);

                    if (intersection != null)
                    {
                        intersections.Add((Vector2)intersection);
                    }
                }
            }
            return intersections;
        }
        public Wire(string[] instructions)
        {
            currentPosition = new Vector2();

            foreach (string instruction in instructions)
            {
                char dir = (char)Regex.Match(instruction, @"\D+").Value[0];
                int steps = int.Parse(Regex.Match(instruction, @"\d+").Value);
                movements.Add(new Movement(dir, steps));
            }

            CreateLines();
        }
    }
}

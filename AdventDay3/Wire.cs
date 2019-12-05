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
    class Point
    {
        public int x;
        public int y;

        public static bool operator ==(Point a, Point b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return a != b;
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
        public int steps;
        public Point originPoint;

        public Line(char _dir, int _steps, int _originX, int _originY)
        {
            dir = _dir;
            steps = _steps;
            originPoint = new Point(_originX, _originY);
        }
    }

    class Wire
    {
        public readonly List<Line> lines = new List<Line>();

        private List<Point> calculatePoints(List<Line> otherLines)
        {
            List<Point> points = new List<Point>();

            foreach (Line line in otherLines)
            {
                for (int i = 1; i <= line.steps; i++)
                {
                    int x = 0;
                    int y = 0;
                    if (line.dir == 'R')
                    {
                        x = line.originPoint.x + i;
                        y = line.originPoint.y;
                    }
                    if (line.dir == 'L')
                    {
                        x = line.originPoint.x - i;
                        y = line.originPoint.y;
                    }
                    if (line.dir == 'U')
                    {
                        x = line.originPoint.x;
                        y = line.originPoint.y + i;
                    }
                    if (line.dir == 'D')
                    {
                        x = line.originPoint.x;
                        y = line.originPoint.y - i;
                    }

                    points.Add(new Point(x, y));
                }
            }

            return points;
        }

        private int getLowestManhattanDistance(List<Point> intersections)
        {
            int? shortestDistance = null;

            foreach (Point point in intersections)
            {
                int manhattanDistance = Math.Abs(point.x) + Math.Abs(point.y);

                if (manhattanDistance > 0 && (shortestDistance == null || shortestDistance > manhattanDistance))
                {
                    shortestDistance = manhattanDistance;
                }
            }

            return (int) shortestDistance;
        }

        public int CalculateShortestDistance(List<Line> otherLines)
        {
            List<Point> points = calculatePoints(lines);
            List<Point> otherPoints = calculatePoints(otherLines);
            List<Point> intersections = new List<Point>();

            foreach (Point point1 in points)
            {
                foreach (Point point2 in otherPoints)
                {
                    if (point1 == point2)
                    {
                        intersections.Add(point1);
                    }
                }
            }

            return getLowestManhattanDistance(intersections);
        }

        public Wire(string[] instructions)
        {
            int x = 0;
            int y = 0;

            foreach (string instruction in instructions)
            {
                char dir = (char)Regex.Match(instruction, @"\D+").Value[0];
                int steps = int.Parse(Regex.Match(instruction, @"\d+").Value);
                lines.Add(new Line(dir, steps, x, y));

                if (dir == 'R')
                {
                    x += steps;
                } else if (dir == 'L')
                {
                    x -= steps;
                } else if (dir == 'U')
                {
                    y += steps;
                } else if (dir == 'D')
                {
                    y -= steps;
                }
            }
        }
    }
}

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

    class Intersection
    {
        public Point point;
        private int distance1;
        private int distance2;
        public int totalDistance;

        public Intersection(Point _point, int _distance1, int _distance2)
        {
            point = _point;
            distance1 = _distance1;
            distance2 = _distance2;
            totalDistance = distance1 + distance2;
        }
    }

    class Line
    {
        public char dir;
        public char dir2d;

        public int steps;
        public Point originPoint;
        public Point endPoint;
        public Point intersects(Line line2)
        {
            if (dir2d == line2.dir2d)
            {
                return null;
            }

            if (dir2d == 'X')
            {
                System.Console.WriteLine("Checking Collision ({0},{1}) - ({2}, {3}) and ({4}, {5}) - ({6}, {7})", originPoint.x, originPoint.y, endPoint.x, endPoint.y, line2.originPoint.x, line2.originPoint.y, line2.endPoint.x, line2.endPoint.y);

                if (originPoint.x <= line2.originPoint.x && endPoint.x >= line2.originPoint.x)
                {
                    if (line2.originPoint.y <= originPoint.y && line2.endPoint.y >= originPoint.y)
                    {
                        System.Console.WriteLine("found, created intersection ({0}, {1})", line2.originPoint.x, originPoint.y);
                        return new Point(line2.originPoint.x, originPoint.y);
                    }
                }
            } else if (dir2d == 'Y')
            {
                return line2.intersects(this);
            }

            return null;
        }

        private Point calculateEndPoint()
        {
            int x = 0;
            int y = 0;

            if (dir == 'R')
            {
                x = originPoint.x + steps;
                y = originPoint.y;
            }
            if (dir == 'L')
            {
                x = originPoint.x - steps;
                y = originPoint.y;
            }
            if (dir == 'U')
            {
                x = originPoint.x;
                y = originPoint.y + steps;
            }
            if (dir == 'D')
            {
                x = originPoint.x;
                y = originPoint.y - steps;
            }

            return new Point(x, y);
        }

        public Line(char _dir, int _steps, int _originX, int _originY)
        {
            dir = _dir;
            steps = _steps;
            originPoint = new Point(_originX, _originY);
            endPoint = new Point(_originX, _originY);

            if (dir == 'R' || dir == 'L')
            {
                dir2d = 'X';
                
            }
            else
            {
                dir2d = 'Y';
            }

            endPoint = calculateEndPoint();
        }
    }

    class Wire
    {
        public readonly List<Line> lines = new List<Line>();

        public List<Point> getIntersections(Wire wire2)
        {
            List<Point> intersections = new List<Point>();

            foreach (Line line in lines)
            {
                foreach(Line line2 in wire2.lines)
                {
                    Point intersection = line.intersects(line2);

                    if (intersection != null)
                    {
                        intersections.Add(intersection);
                    }
                }
            }

            return intersections;
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

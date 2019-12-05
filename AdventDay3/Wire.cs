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

        //public static bool operator ==(Point a, Point b)
        //{
        //    return a.x == b.x && a.y == b.y;
        //}

        //public static bool operator !=(Point a, Point b)
        //{
        //    return a != b;
        //}

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
                if (originPoint.x < line2.originPoint.x && endPoint.x > line2.originPoint.x)
                {
                    if (line2.originPoint.y < originPoint.y && line2.endPoint.y > originPoint.y)
                    {
                        return new Point(line2.originPoint.x, originPoint.y);
                    }
                }
            } else if (dir2d == 'Y')
            {
                if (line2.dir2d == 'X')
                {
                    return line2.intersects(this);
                }
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
        public List<Point> intersections = new List<Point>();

        //private int getLowestManhattanDistance(List<Point> intersections)
        //{
        //    int? shortestDistance = null;

        //    foreach (Point point in intersections)
        //    {
        //        int manhattanDistance = Math.Abs(point.x) + Math.Abs(point.y);

        //        if (manhattanDistance > 0 && (shortestDistance == null || shortestDistance > manhattanDistance))
        //        {
        //            shortestDistance = manhattanDistance;
        //        }
        //    }

        //    return (int) shortestDistance;
        //}

        //private int getLowestLength(List<Intersection> intersections)
        //{
        //    int? shortestDistance = null;

        //    foreach (Intersection intersection in intersections)
        //    {
        //        int manhattanDistance = intersection.totalDistance;

        //        if (manhattanDistance > 0 && (shortestDistance == null || shortestDistance > manhattanDistance))
        //        {
        //            shortestDistance = manhattanDistance;
        //        }
        //    }

        //    return (int)shortestDistance;
        //}

        //public int CalculateShortestDistance(List<Line> otherLines)
        //{
        //    List<Point> intersections = new List<Point>();

        //    foreach (Line line in lines)
        //    {
        //        foreach (Line line2 in otherLines)
        //        {
        //            if ((line.dir == 'U' || line.dir == 'D') && (line2.dir == 'R' || line2.dir == 'L') ||
        //                (line2.dir == 'U' || line2.dir == 'D') && (line.dir == 'R' || line.dir == 'L'))
        //            {
        //                foreach (Point point1 in line.points)
        //                {
        //                    foreach (Point point2 in line2.points)
        //                    {
        //                        if (point1 == point2)
        //                        {
        //                            intersections.Add(point1);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return getLowestManhattanDistance(intersections);
        //}

        //public int CalculateShortestLength(List<Line> otherLines)
        //{
        //    List<Intersection> intersections = new List<Intersection>();

        //    int distance1 = 0;
        //    for (int i = 0; i < lines.Count; i ++)
        //    {
        //        int distance2 = 0;

        //        for (int j = 0; j < otherLines.Count; j++)
        //        {

        //            if ((lines[i].dir == 'U' || lines[i].dir == 'D') && (otherLines[j].dir == 'R' || otherLines[j].dir == 'L') ||
        //                (otherLines[j].dir == 'U' || otherLines[j].dir == 'D') && (lines[i].dir == 'R' || otherLines[j].dir == 'L'))
        //            {
        //                foreach (Point point1 in lines[i].points)
        //                {
        //                    foreach (Point point2 in otherLines[j].points)
        //                    {
        //                        if (point1 == point2)
        //                        {
        //                            Point lineOrigin = lines[i].originPoint;
        //                            Point otherLineOrigin = otherLines[j].originPoint;

        //                            int difference1 = Math.Abs(lineOrigin.x - point1.x) + Math.Abs(lineOrigin.y - point1.y);
        //                            int difference2 = Math.Abs(otherLineOrigin.x - point1.x) + Math.Abs(otherLineOrigin.y - point1.y);

        //                            Intersection found = new Intersection(point1, distance1 + difference1, distance2 + difference2);
        //                            intersections.Add(found);
        //                        }
        //                    }
        //                }
        //            }
        //            distance2 += otherLines[j].steps;
        //        }
        //        distance1 += lines[i].steps;
        //    }

        //    return getLowestLength(intersections);
        //}


        public void getIntersections(Wire wire2)
        {
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

            return;
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

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace AdventDay3
{
    class Intersection
    {
        public Point pos;
        public int dist1;
        public int dist2;

        public int totalDist;

        public Intersection(Point _pos, Point p1, Point p2, int _dist1, int _dist2)
        {
            pos = _pos;

            int dif1 = Math.Abs(p1.x - pos.x);

            if (dif1 == 0)
            {
                dif1 = Math.Abs(p1.y - pos.y);
            }

            int dif2 = Math.Abs(p2.y - pos.y);
            if (dif2 == 0)
            {
                dif2 = Math.Abs(p2.x - pos.x);
            }

            dist1 = _dist1 + dif1;
            dist2 = _dist2 + dif2;
            totalDist = dist1 + dist2;
        }
    }
    class Program
    {
        static int getShortestManhattan(List<Point> points)
        {
            int? shortest = null;

            if (points.Count == 0)
            {
                return 0;
            }

            foreach (Point point in points)
            {
                int distance = point.getManhattanDistance();

                if (distance > 0)
                {
                    if (distance < shortest || shortest == null)
                    {
                        shortest = distance;
                    }
                }
            }

            return (int)shortest;
        }

        static int getShortestLengths(List<int> lengths)
        {
            int? shortest = null;

            if (lengths.Count == 0)
            {
                return 0;
            }

            foreach (int distance in lengths)
            {
                if (distance > 0)
                {
                    if (distance < shortest || shortest == null)
                    {
                        shortest = distance;
                    }
                }
            }

            return (int)shortest;
        }

        static Point checkIntersection(Line line1, Line line2)
        {
            if (line1.dir2d == line2.dir2d)
            {
                return null;
            }

            if (line1.dir2d == 'X')
            {
                //System.Console.WriteLine("Checking Collision ({0},{1}) - ({2}, {3}) and ({4}, {5}) - ({6}, {7})", line1.p1.x, line1.p1.y, line1.p2.x, line1.p2.y, line2.p1.x, line2.p1.y, line2.p2.x, line2.p2.y);

                int x1 = Math.Min(line1.p1.x, line1.p2.x);
                int x2 = Math.Max(line1.p1.x, line1.p2.x);
                int y1 = Math.Min(line2.p1.y, line2.p2.y);
                int y2 = Math.Max(line2.p1.y, line2.p2.y);

                if (line2.p1.x > x1 && line2.p1.x < x2)
                {
                    if (line1.p1.y > y1 && line1.p1.y < y2)
                    {
                        //System.Console.WriteLine("found, created intersection ({0}, {1})", line2.p1.x, line1.p1.y);
                        return new Point(line2.p1.x, line1.p1.y);
                    }
                }
            }
            else
            {
                return checkIntersection(line2, line1);
            }

            return null;
        }

        static List<Point> getIntersections(List<Wire> wires)
        {
            List<Point> intersections = new List<Point>();

            foreach (Line line1 in wires[0].lines)
            {
                foreach (Line line2 in wires[1].lines)
                {
                    Point intersection = checkIntersection(line1, line2);
                    if (intersection != null)
                    {
                        intersections.Add(intersection);
                    }
                }
            }

            return intersections;
        }

        static List<Intersection> getIntersectionsPart2(List<Wire> wires)
        {
            List<Intersection> intersections = new List<Intersection>();

            int distance1 = 0;
            foreach (Line line1 in wires[0].lines)
            {
                int distance2 = 0;
                foreach (Line line2 in wires[1].lines)
                {
                    Point intPoint = checkIntersection(line1, line2);
                    if (intPoint != null)
                    {

                        intersections.Add(new Intersection(intPoint, line1.p1, line2.p1, distance1, distance2));
                    }

                    distance2 += line2.steps;
                }

                distance1 += line1.steps;
            }

            return intersections;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            System.IO.StreamReader file =
                new System.IO.StreamReader("../../../data.txt");

            string str = file.ReadToEnd();
            file.Close();

            List<Wire> wires = new List<Wire>();
            foreach (string wire in str.Split('\n'))
            {
                wires.Add(new Wire(wire.Split(',')));
            }

            List<Point> intersections = getIntersections(wires);
            List<Intersection> intersections2 = getIntersectionsPart2(wires);

            List<int> lengths = new List<int>();
            foreach (Intersection intersection in intersections2)
            {
                lengths.Add(intersection.totalDist);
            }

            int answer = getShortestManhattan(intersections);
            int answer2 = getShortestLengths(lengths);
            System.Console.WriteLine("The answer is {0}", answer);
            System.Console.WriteLine("The answer is {0}", answer2);
            System.Console.ReadLine();
        }
    }
}

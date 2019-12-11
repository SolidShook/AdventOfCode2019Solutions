using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace AdventDay10
{
    class Point
    {
        public int X;
        public int Y;

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Point p = (Point)obj;
                return (X == p.X) && (Y == p.Y);
            }
        }

        public override int GetHashCode()
        {
            return (X << 2) ^ Y;
        }

        public Point()
        {

        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    class Asteroid
    {
        public Point Pos;
        public Point GlobalPos;
        public double AngleX;
        public double AngleY;
        public double Angle;

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Asteroid p = (Asteroid)obj;

                return AngleX == p.AngleX && AngleY == p.AngleY;
            }
        }

        public static bool operator ==(Asteroid lhs, Asteroid rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Asteroid lhs, Asteroid rhs)
        {
            return !(lhs.Equals(rhs));
        }

        public override int GetHashCode()
        {
            return (int) AngleY ^ (int)AngleX;
        }

        public Asteroid(int x, int y, int globalX, int globalY)
        {
            Pos = new Point(x, y);
            GlobalPos = new Point(globalX, globalY);

            double m = Math.Sqrt(x * x + y * y);

            AngleX = Math.Round(x / m, 3);
            AngleY = Math.Round(y / m, 3);

            Angle = Math.Atan2(AngleX, -AngleY);
            if (Angle < 0)
            {
                Angle += Math.PI * 2;
            }
        }
    }
    class Map
    {

        private List<string> _map = new List<string>();

        private void CompareAsteroids()
        {

        }

        private SortedDictionary<double, Queue<Asteroid>> ScanAroundAsteroid(Point centre)
        {
            bool leftReached = false, rightReached = false, topReached = false, bottomReached = false;

            int dist = 1;

            //queues of asteroids, ordered by angle, and then distance;
            SortedDictionary<double, Queue<Asteroid>> astCollection = new SortedDictionary<double, Queue<Asteroid>>();

            void AnalyseAsteroid(int localX, int localY, int globalX, int globalY)
            {
                Asteroid ast = new Asteroid(localX, localY, globalX, globalY);

                if (astCollection.ContainsKey(ast.Angle))
                {
                    astCollection[ast.Angle].Enqueue(ast);
                }
                else
                {
                    Queue<Asteroid> astQ = new Queue<Asteroid>();
                    astCollection.Add(ast.Angle, astQ);
                }
            }

            void CheckPoints()
            {
                for (int y = -dist; y <= dist; y++)
                {
                    int globalY = centre.Y + y;
                    if (globalY < 0)
                    {
                        topReached = true;
                        continue;
                    }
                    else if (globalY > _map.Count - 1)
                    {
                        bottomReached = true;
                        continue;
                    }

                    for (int x = -dist; x <= dist; x++)
                    {
                        int globalX = centre.X + x;

                        if (globalX == centre.X && globalY == centre.Y)
                        {
                            continue;
                        }

                        if (globalX < 0)
                        {
                            leftReached = true;
                            continue;
                        }

                        if (globalX > _map[globalY].Length - 1)
                        {
                            rightReached = true;
                            continue;
                        }

                        if (_map[globalY][globalX] == '#')
                        {
                            AnalyseAsteroid(x, y, globalX, globalY);
                        }
                    }
                }
            }

            while (!leftReached || !rightReached || !topReached || !bottomReached)
            {
                CheckPoints();
                dist++;
            }

            return astCollection;
        }

        public Asteroid SpinLaser(SortedDictionary<double, Queue<Asteroid>> asts, int max)
        {
            int counter = 0;
            Asteroid lastBlast = new Asteroid(0, 0, 0, 0);
            while (counter < max && asts.Count != 0)
            {
                foreach (KeyValuePair<double, Queue<Asteroid>> astP in asts)
                {
                    if (counter >= max)
                    {
                        break;
                    }
                    lastBlast = astP.Value.Dequeue();
                    counter++;
                }

                //remove empties
                foreach (KeyValuePair<double, Queue<Asteroid>> astD in asts.ToList())
                {
                    if (astD.Value.Count == 0)
                        asts.Remove(astD.Key);
                }
            }


            Console.WriteLine("asteroid {0} destroid at postion {1},{2}", counter, lastBlast.GlobalPos.X, lastBlast.GlobalPos.Y);

            return lastBlast;
        }

        public void FindBestAsteroid()
        {
            int mostFound = 0;
            int foundX = 0;
            int foundY = 0;
            SortedDictionary<double, Queue<Asteroid>> foundDict = new SortedDictionary<double, Queue<Asteroid>>();

            for (int y = 0; y < _map.Count; y++)
            {
                for (int x = 0; x < _map[y].Length; x++)
                {
                    if (_map[y][x] == '#')
                    {
                        SortedDictionary<double, Queue<Asteroid>> foundDictBuf = ScanAroundAsteroid(new Point(x, y));
                        
                        if (foundDictBuf.Count > mostFound)
                        {
                            mostFound = foundDictBuf.Count;
                            foundX = x;
                            foundY = y;
                            foundDict = foundDictBuf;
                        }
                    }
                }
            }

            Console.WriteLine("found {0} at postion {1},{2}", mostFound, foundX, foundY);

            Asteroid answer2 = SpinLaser(foundDict, 200);
        }

        public Map(string fileAddress)
        {
            System.IO.StreamReader file =
                new System.IO.StreamReader(fileAddress);

            string line;

            while ((line = file.ReadLine()) != null)
            {
                _map.Add(line);
            }

            file.Close();

            FindBestAsteroid();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Map testmap1 = new Map("../../../test1.txt");
            Map testmap2 = new Map("../../../test2.txt");
            Map testmap3 = new Map("../../../test3.txt");
            Map testmap4 = new Map("../../../test4.txt");
            Map testmap5 = new Map("../../../test5.txt");

            Map answerMap = new Map("../../../data.txt");

            Console.ReadLine();
        }
    }
}

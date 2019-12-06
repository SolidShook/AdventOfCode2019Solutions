using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventDay6
{
    static class Commands
    {
        public const char ORBITED_BY = ')';
    }

    class SpaceObject
    {
        public string Target;

        public SpaceObject()
        {
        }
    }

    class Sorter
    {
        private Dictionary<string, SpaceObject> SpaceObjects;

        private void CreateOrbiters(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (!SpaceObjects.ContainsKey(names[i]))
                {
                    SpaceObject sObj = new SpaceObject();
                    SpaceObjects.Add(names[i], sObj);
                }

                if (i > 0)
                {
                    SpaceObject sObj = SpaceObjects[names[i]];
                    sObj.Target = names[i - 1];
                }
            }
        }

        private int CountOrbits(SpaceObject obj, int count)
        {
            if (obj.Target != null)
            {
                count = CountOrbits(SpaceObjects[obj.Target], count + 1);
            }

            return count;
        }

        private HashSet<string> GetPathToCenter(SpaceObject obj)
        {
            HashSet<string> path = new HashSet<string>();
            string nextObj = obj.Target;

            while (nextObj != null)
            {
                path.Add(nextObj);

                nextObj = SpaceObjects[nextObj].Target;
            }

            return path;
        }

        private int CountAllOrbits()
        {
            int orbitCount = 0;

            foreach (KeyValuePair<string, SpaceObject> obj in SpaceObjects)
            {
                int result = CountOrbits(obj.Value, 0);

                Console.WriteLine("Obj {0} has {1} orbits", obj.Key, result);
                orbitCount += result;
                // Do something here
            }

            return orbitCount;
        }

        private int GetStepsToSanta()
        {
            SpaceObject you = SpaceObjects["YOU"];
            SpaceObject santa = SpaceObjects["SAN"];


            return 0;
        }

        public Sorter(string[] instrs)
        {
            SpaceObjects = new Dictionary<string, SpaceObject>();

            foreach (string instr in instrs)
            {
                string[] names = instr.Split(Commands.ORBITED_BY);

                CreateOrbiters(names);
            }


            int answer = CountAllOrbits();

            Console.WriteLine("the answer is {0}", answer);
            Console.ReadLine();
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
            System.IO.StreamReader file =
                new System.IO.StreamReader("../../../data.txt");

            string str = file.ReadToEnd();
            file.Dispose();

            List<string> instrs = new List<string>();

            instrs.AddRange(str.Split("\r\n"));

            new Sorter(instrs.ToArray());
        }
    }
}

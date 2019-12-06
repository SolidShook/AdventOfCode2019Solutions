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
        public int Steps;
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

        private int CountOrbits(SpaceObject obj)
        {
            int count = 0;
            string nextObj = obj.Target;

            while (nextObj != null)
            {
                count++;

                nextObj = SpaceObjects[nextObj].Target;
            }

            obj.Steps = count;

            return count;
        }

        public HashSet<string> GetPathToCentre(SpaceObject obj)
        {
            //Could have done in previous function, but it would have wasted a lot of memory.
            //It's like a memory vs performance thing. Not knowing the amount of spaceObjects makes might make this safer;

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
                int result = CountOrbits(obj.Value);

                orbitCount += result;
            }

            return orbitCount;
        }

        private int GetStepsToSanta()
        {
            //Answer2

            SpaceObject you = SpaceObjects["YOU"];
            SpaceObject santa = SpaceObjects["SAN"];

            HashSet<string> youPath = GetPathToCentre(you);
            HashSet<string> santaPath = GetPathToCentre(santa);
            HashSet<string> common = new HashSet<string>(youPath);

            common.IntersectWith(santaPath);
            youPath.ExceptWith(common);
            santaPath.ExceptWith(common);

            return youPath.Count + santaPath.Count;
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

            int answer2 = GetStepsToSanta();

            Console.WriteLine("the answer is {0}", answer2);
            
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
            while (true)
            {
                System.IO.StreamReader file =
                new System.IO.StreamReader("../../../data.txt");

                string str = file.ReadToEnd();
                file.Dispose();

                List<string> instrs = new List<string>();

                instrs.AddRange(str.Split("\r\n"));

                new Sorter(instrs.ToArray());

                Console.ReadLine();
            }
        }
    }
}

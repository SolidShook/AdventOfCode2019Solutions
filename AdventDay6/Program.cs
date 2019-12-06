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
        public HashSet<string> Path;
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
            HashSet<string> path = new HashSet<string>();
            string nextObj = obj.Target;

            while (nextObj != null)
            {
                count++;

                //not efficient but saves making a new loop
                path.Add(nextObj);

                nextObj = SpaceObjects[nextObj].Target;
            }

            obj.Steps = count;
            obj.Path = path;

            return count;
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

            HashSet<string> youPath = new HashSet<string>(you.Path);
            HashSet<string> santaPath = new HashSet<string>(santa.Path);
            HashSet<string> ans = new HashSet<string>(you.Path);

            ans.IntersectWith(santa.Path);
            youPath.ExceptWith(ans);
            santaPath.ExceptWith(ans);


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

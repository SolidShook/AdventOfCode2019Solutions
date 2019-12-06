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
        public string Target { get; set; }

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
                    if (i > 0)
                    {
                        sObj.Target = names[i - 1];
                    }
                    SpaceObjects.Add(names[i], sObj);
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

        private int CountAllOrbits()
        {
            int orbitCount = 0;

            foreach (KeyValuePair<string, SpaceObject> obj in SpaceObjects)
            {
                orbitCount += CountOrbits(obj.Value, 0);
                // Do something here
            }

            return orbitCount;
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

using System;
using System.Collections.Generic;
using System.Numerics;

namespace AdventDay3
{
    class Program
    {
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

            wires[0].getIntersections(wires[1]);

            //int answer = wires[0].CalculateShortestDistance(wires[1].lines);

            //int answer = wires[0].CalculateShortestLength(wires[1].lines);
            System.Console.WriteLine("The answer is {0}", 1);

            System.Console.ReadLine();
        }
    }
}

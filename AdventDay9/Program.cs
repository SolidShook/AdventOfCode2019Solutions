using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventDay9
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
            string[] rawIntCode = str.Split(',');
            List<BigInteger> bigInts = rawIntCode.Select(val => BigInteger.Parse(val)).ToList();

            IntCodeParser parser = new IntCodeParser(bigInts);

            parser.ProcessIntCode(true);

            Console.ReadLine();
        }
    }
}

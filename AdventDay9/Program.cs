using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventDay9
{
    class Program
    {
        static List<BigInteger> GetBigInts(string address)
        {
            System.IO.StreamReader file =
                new System.IO.StreamReader(address);

            string str = file.ReadToEnd();
            file.Close();

            string[] rawIntCode = str.Split(',');
            return rawIntCode.Select(val => BigInteger.Parse(val)).ToList();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            IntCodeParser test1 = new IntCodeParser(GetBigInts("../../../test1.txt"));
            IntCodeParser test2 = new IntCodeParser(GetBigInts("../../../test2.txt"));
            IntCodeParser test3 = new IntCodeParser(GetBigInts("../../../test3.txt"));

            IntCodeParser parser = new IntCodeParser(GetBigInts("../../../data.txt"));


            test1.ProcessIntCode(true);
            test2.ProcessIntCode(true);
            test3.ProcessIntCode(true);
            Instructions result =  Instructions.Error;

            while (result != Instructions.Halt)
            {
                result = parser.ProcessIntCode(false);

            }

            Console.ReadLine();
        }
    }
}

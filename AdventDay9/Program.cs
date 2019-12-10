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


            IntCodeProcessor test1 = new IntCodeProcessor(GetBigInts("../../../test1.txt"));
            IntCodeProcessor test2 = new IntCodeProcessor(GetBigInts("../../../test2.txt"));
            IntCodeProcessor test3 = new IntCodeProcessor(GetBigInts("../../../test3.txt"));

            IntCodeProcessor processor = new IntCodeProcessor(GetBigInts("../../../data.txt"));


            test1.ProcessIntCode(true);
            test2.ProcessIntCode(true);
            test3.ProcessIntCode(true);
            Instructions result =  Instructions.Error;

            while (result != Instructions.Halt)
            {
                result = processor.ProcessIntCode(false);

            }

            Console.ReadLine();
        }
    }
}

using System;

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

            int[] intCode = Array.ConvertAll(str.Split(','), int.Parse);
            IntCodeParser parser = new IntCodeParser(intCode);

            parser.ProcessIntCode(true);

            Console.ReadLine();
        }
    }
}

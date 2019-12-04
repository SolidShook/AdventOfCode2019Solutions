using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventDay2
{
    class Program
    {
        private static void ProcessRow(int[] row)
        {

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int counter = 0;
            string line;

            System.IO.StreamReader file =
                new System.IO.StreamReader("../../../data.txt");

            string str = file.ReadToEnd();

            List<int[]> rows = new List<int[]> ();
            string[] strArray = str.Split(',');

            for (int i = 0; i < strArray.Length; i+=5)
            {
                int[] row = new int[5];
                for (int j = 0; j < 5; j++)
                {
                    row[j] = Int32.Parse(strArray[i+j]);
                }

                rows.Add(row);
            }

            foreach (int[] row in rows)
            {
                
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            System.Console.ReadLine();
        }
    }
}

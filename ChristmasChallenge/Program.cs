using System;

namespace ChristmasChallenge
{
    class Program
    {
        private static int CalcThing(int mass)
        {
            int fuel = 0;
            fuel = (int)Math.Floor((float)(mass / 3));
            fuel -= 2;

            return fuel;
        }
        private static int CalculateFuel(int mass)
        {
            int fuel = CalcThing(mass);

            if (CalcThing(fuel) > 0)
            {
                fuel += CalculateFuel(fuel);
            }

            return fuel;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int counter = 0;
            string line;

            int fuel = 0;

            System.IO.StreamReader file =
                new System.IO.StreamReader("../../../data.txt");
            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                fuel += CalculateFuel(Int32.Parse(line));
                counter++;
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            System.Console.WriteLine("You need {0} fuel", fuel);
            // Suspend the screen.  
            System.Console.ReadLine();
        }
    }
}

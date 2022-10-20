using System;

namespace RollReturn
{
    class Program
    {
        static int CalculateRoll(int diename)
        {

            Random random = new Random();
            int Roll = 0;

            if (diename == 20)
            {
                Roll += random.Next(1,11) * 2;
            }
            if (diename == 12) {
                Roll += random.Next(1,13);
            }
            if (diename == 10)
            {
                Roll += random.Next(1, 101);
            }
            if (diename == 8)
            {
                Roll += random.Next(1, 9);
            }
            if (diename == 6)
            {
                Roll += random.Next(1, 7);
            }
            if (diename == 4)
            {
                Roll += random.Next(1, 5);
            }

            return Roll;
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Enter die value as a number (e.g. 20):");
            int DieValueInput = int.Parse(Console.ReadLine());
            int[] DieValuesArray = { 4, 6, 8, 10, 12, 20 };

            if (DieValuesArray.Contains(DieValueInput))
            {
                int Roll = CalculateRoll(DieValueInput);
                Console.WriteLine(Roll);
            }
            else
            {
                Console.WriteLine("Wrong die number! Possibilities are 4, 6, 8, 10, 12 and 20. Try again!");
            }
        }
    }
}
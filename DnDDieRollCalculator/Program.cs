using System;
namespace RollReturn
{
    class Program
    {
        static int CalculateRoll(int dievalue)
        {

            Random random = new Random();
            int Roll = 0;

            Roll += random.Next(1, dievalue);

            return Roll;
        }
        
        static void Main(string[] args)
        {
        }
    }
}
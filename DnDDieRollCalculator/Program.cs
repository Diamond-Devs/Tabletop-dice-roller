using System;
namespace RollReturn
{
    class Program
    {
        static int CalculateRoll(int dieValue)
        {

            Random random = new Random();
            int roll = 0;

            roll += random.Next(1, dieValue);

            return roll;
        }
        
        static void Main(string[] args)
        {
        }
    }
}
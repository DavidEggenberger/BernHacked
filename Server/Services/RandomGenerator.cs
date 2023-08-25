using System;

namespace Server.Services
{
    public class RandomGenerator
    {
        public int GenerateRandomInt()
        {
            return new Random().Next(int.MaxValue);
        }
    }
}

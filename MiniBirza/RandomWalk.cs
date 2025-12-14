using System;

namespace MiniBirza
{
    //Random Walk modelis - kaina juda atsitiktinai į viršų/apačią.
    public class RandomWalk : IPriceModel
    {
        private readonly Random _random;
        public string Name => "Random Walk";

        public RandomWalk(Random random)
        {
            _random = random;
        }

        public decimal Next(decimal currentPrice)
        {
            // Atsitiktinis pokytis tarp -3% ir +3%
            double change = _random.NextDouble() * 0.06 - 0.03;
            decimal factor = 1m + (decimal)change;
            decimal newPrice = currentPrice * factor;
            if (newPrice < 0.01m) newPrice = 0.01m;
            return decimal.Round(newPrice, 2);
        }
    }
}
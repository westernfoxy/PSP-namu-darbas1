using System;

namespace MiniBirza
{
    //Mean Reversion modelis - kaina linkusi grįžti prie ilgalaikio vidurkio.
    public class MeanReversion : IPriceModel
    {
        private readonly Random _random;
        private readonly decimal _anchorPrice;

        public string Name => "Mean Reversion";

        public MeanReversion(Random random, decimal anchorPrice)
        {
            _random = random;
            _anchorPrice = anchorPrice;
        }

        public decimal Next(decimal currentPrice)
        {
            // Atsitiktinis pokytis tarp -2% ir +2%
            double change = _random.NextDouble() * 0.04 - 0.02;
            decimal randomPart = currentPrice * (1m + (decimal)change);

            decimal drift = (_anchorPrice - currentPrice) * 0.02m;

            decimal newPrice = randomPart + drift;
            if (newPrice < 0.01m) newPrice = 0.01m;
            return decimal.Round(newPrice, 2);
        }
    }
}

using System;
using System.Globalization;

namespace MiniBirza
{
    // parenka modeli ir paleidžia žaidima.
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            const decimal startingCash = 10_000m;
            const decimal startPrice = 100m;
            const decimal feeRate = 0.001m; // 0.1% mokestis
            const int maxRounds = 50;
            const int smaPeriod = 5;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== Biržos mini-simuliatorius ===");
            Console.ResetColor();
            Console.WriteLine($"Pradinis balansas: {startingCash:F2}");
            Console.WriteLine($"Pradinė aktyvo kaina: {startPrice:F2}");
            Console.WriteLine();
            Console.WriteLine("Pasirinkite kainos modelį:");
            Console.WriteLine("1 - Random Walk");
            Console.WriteLine("2 - Mean Reversion");
            Console.Write("Jūsų pasirinkimas (1/2): ");

            IPriceModel priceModel;
            var random = new Random();

            string choice = Console.ReadLine()?.Trim() ?? "";
            if (choice == "2")
            {
                priceModel = new MeanReversion(random, startPrice);
            }
            else
            {
                priceModel = new RandomWalk(random);
            }

            var market = new Market(startPrice, priceModel);
            var portfolio = new Portfolio(startingCash);
            var sma = new SMAIndicator(smaPeriod);

            var game = new GameEngine(market, portfolio, sma, feeRate, maxRounds);
            game.Run();
        }
    }

}

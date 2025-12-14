using System;

namespace MiniBirza
{
    // valdo ėjimus, būseną ir vartotojo įvestį.
    public class GameEngine
    {
        private readonly Market _market;
        private readonly Portfolio _portfolio;
        private readonly SMAIndicator _sma;
        private readonly decimal _feeRate;
        private readonly int _maxRounds;

        public GameEngine(Market market, Portfolio portfolio, SMAIndicator sma, decimal feeRate, int maxRounds)
        {
            _market = market;
            _portfolio = portfolio;
            _sma = sma;
            _feeRate = feeRate;
            _maxRounds = maxRounds;
        }

        public void Run()
        {
            bool exitRequested = false;

            for (int round = 1; round <= _maxRounds; round++)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=== Biržos simuliatorius ===");
                Console.ResetColor();
                Console.WriteLine($"Modelis: {_market.ModelName}");
                Console.WriteLine($"Eiga: {round}/{_maxRounds}");
                Console.WriteLine();

                decimal price = _market.NextPrice();
                _sma.AddPrice(price);

                decimal totalValue = _portfolio.GetTotalValue(price);
                decimal pnl = totalValue - _portfolio.StartingCash;

                Console.WriteLine($"Dabartinė kaina: {price:F2}");
                var smaValue = _sma.CurrentValue;
                if (smaValue.HasValue)
                {
                    Console.WriteLine($"SMA ({_sma.Period}): {smaValue.Value:F2}");
                }
                else
                {
                    Console.WriteLine($"SMA ({_sma.Period}): nepakanka duomenų");
                }

                Console.WriteLine();
                Console.WriteLine($"Grynieji pinigai:  {_portfolio.Cash:F2}");
                Console.WriteLine($"Turimas kiekis:    {_portfolio.Quantity} vnt.");
                Console.WriteLine($"Portfelio vertė:   {totalValue:F2}");
                Console.WriteLine($"Pelnas/nuostolis:  {pnl:F2}");
                Console.WriteLine();

                if (_portfolio.IsBankrupt(price))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("BANKROTAS! Jūsų portfelio vertė nukrito iki nulio.");
                    Console.ResetColor();
                    break;
                }

                while (true)
                {
                    Console.WriteLine("Pasirinkite veiksmą:");
                    Console.WriteLine("[B]UY  - pirkti");
                    Console.WriteLine("[S]ELL - parduoti");
                    Console.WriteLine("[H]OLD - nieko nedaryti");
                    Console.WriteLine("[Q]UIT - baigti žaidimą");
                    Console.Write("Veiksmas: ");

                    string action = Console.ReadLine()?.Trim().ToUpperInvariant() ?? "";

                    if (string.IsNullOrWhiteSpace(action))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Veiksmas negali būti tuščias. Spauskite Enter ir bandykite dar kartą.");
                        Console.ResetColor();
                        Console.ReadLine();
                        continue;
                    }

                    if (action == "Q")
                    {
                        Console.WriteLine("Žaidimas baigtas žaidėjo pasirinkimu.");
                        exitRequested = true;
                        break;
                    }

                    if (action == "H")
                    {
                        break;
                    }

                    if (action == "B" || action == "S")
                    {
                        int quantity = AskQuantity();
                        if (quantity <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Įvestas neteisingas kiekis. Spauskite Enter ir bandykite įvesti dar kartą.");
                            Console.ResetColor();
                            Console.ReadLine();
                            continue;
                        }

                        bool success;
                        string message;

                        if (action == "B")
                        {
                            success = _portfolio.Buy(quantity, price, _feeRate, out message);
                        }
                        else
                        {
                            success = _portfolio.Sell(quantity, price, _feeRate, out message);
                        }

                        Console.WriteLine(message);
                        Console.WriteLine("Spauskite Enter kad tęsti...");
                        Console.ReadLine();

                        break;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Neteisingai įvestas veiksmas. Spauskite Enter ir bandykite dar kartą.");
                    Console.ResetColor();
                    Console.ReadLine();
                }

                if (exitRequested)
                    break;

                if (round == _maxRounds)
                {
                    Console.WriteLine();
                    Console.WriteLine("Laikas baigėsi – pasiektas maksimalus ėjimų skaičius.");
                }
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Galutiniai rezultatai ===");
            Console.ResetColor();
            Console.WriteLine($"Galutinė aktyvo kaina: {_market.CurrentPrice:F2}");
            Console.WriteLine($"Grynieji pinigai:      {_portfolio.Cash:F2}");
            Console.WriteLine($"Turimas kiekis:        {_portfolio.Quantity} vnt.");
            Console.WriteLine($"Portfelio vertė:       {_portfolio.GetTotalValue(_market.CurrentPrice):F2}");
            Console.WriteLine($"Pelnas/nuostolis:      {_portfolio.GetTotalValue(_market.CurrentPrice) - _portfolio.StartingCash:F2}");
            Console.WriteLine();
            Console.WriteLine("Spauskite Enter, kad išeitumėte...");
            Console.ReadLine();
        }

        private int AskQuantity()
        {
            Console.Write("Įveskite kiekį (vnt.): ");
            string input = Console.ReadLine() ?? "";
            if (int.TryParse(input, out int quantity) && quantity > 0)
            {
                return quantity;
            }
            return 0;
        }
    }
}
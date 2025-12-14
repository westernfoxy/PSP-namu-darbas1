using System;

namespace MiniBirza
{
    //pinigai, jų kiekis ir operacijos BUY/SELL.
    public class Portfolio
    {
        public decimal Cash { get; private set; }
        public int Quantity { get; private set; }
        public decimal StartingCash { get; }

        public Portfolio(decimal startingCash)
        {
            StartingCash = startingCash;
            Cash = startingCash;
            Quantity = 0;
        }

        public decimal GetTotalValue(decimal currentPrice)
        {
            return Cash + Quantity * currentPrice;
        }

        public bool Buy(int quantity, decimal price, decimal feeRate, out string message)
        {
            if (quantity <= 0)
            {
                message = "Kiekis turi būti teigiamas.";
                return false;
            }

            decimal tradeValue = quantity * price;
            decimal fee = tradeValue * feeRate;
            decimal totalCost = tradeValue + fee;

            if (totalCost > Cash)
            {
                message = "Nepakanka lėšų šiam pirkimui.";
                return false;
            }

            Cash -= totalCost;
            Quantity += quantity;
            message = $"Nupirkta {quantity} vnt. už {tradeValue:F2}, mokestis {fee:F2}.";
            return true;
        }

        public bool Sell(int quantity, decimal price, decimal feeRate, out string message)
        {
            if (quantity <= 0)
            {
                message = "Kiekis turi būti teigiamas.";
                return false;
            }

            if (quantity > Quantity)
            {
                message = "Neturite tiek vienetų, kad parduotumėte.";
                return false;
            }

            decimal tradeValue = quantity * price;
            decimal fee = tradeValue * feeRate;
            decimal totalReceived = tradeValue - fee;

            Cash += totalReceived;
            Quantity -= quantity;
            message = $"Parduota {quantity} vnt. už {tradeValue:F2}, mokestis {fee:F2}.";
            return true;
        }

        public bool IsBankrupt(decimal currentPrice)
        {
            return GetTotalValue(currentPrice) <= 0.01m;
        }
    }
}

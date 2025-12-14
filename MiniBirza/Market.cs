using System;

namespace MiniBirza
{
    //saugo einamą kainą ir modelį, generuoja naują kainą.
    public class Market
    {
        public decimal CurrentPrice { get; private set; }
        private readonly IPriceModel _priceModel;

        public Market(decimal startPrice, IPriceModel priceModel)
        {
            CurrentPrice = startPrice;
            _priceModel = priceModel;
        }

        public decimal NextPrice()
        {
            CurrentPrice = _priceModel.Next(CurrentPrice);
            return CurrentPrice;
        }

        public string ModelName => _priceModel.Name;
    }
}

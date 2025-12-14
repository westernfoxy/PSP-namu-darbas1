using System;
using System.Collections.Generic;

namespace MiniBirza
{
    //SMA (Simple Moving Average) - n kainų aritmetinis vidurkis
    public class SMAIndicator
    {
        private readonly int _period;
        private readonly Queue<decimal> _values;
        private decimal _sum;

        public int Period => _period;

        public SMAIndicator(int period)
        {
            if (period <= 0) throw new ArgumentException("Periodas turi būti teigiamas.");
            _period = period;
            _values = new Queue<decimal>();
            _sum = 0m;
        }

        public void AddPrice(decimal price)
        {
            _values.Enqueue(price);
            _sum += price;

            if (_values.Count > _period)
            {
                decimal removed = _values.Dequeue();
                _sum -= removed;
            }
        }

        public decimal? CurrentValue
        {
            get
            {
                if (_values.Count < _period)
                    return null;

                return _sum / _period;
            }
        }
    }
}

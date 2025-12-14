using System;

namespace MiniBirza
{
    public interface IPriceModel
    {
        string Name { get; }
        decimal Next(decimal currentPrice);
    }
}

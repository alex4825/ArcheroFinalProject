using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet
{
    public class WalletService
    {
        private readonly Dictionary<CurrencyTypes, ReactiveVariable<int>> _currencies;

        public WalletService(Dictionary<CurrencyTypes, ReactiveVariable<int>> currencies)
        {
            _currencies = new(currencies);
        }

        public List<CurrencyTypes> AvailableCurrencies => _currencies.Keys.ToList();

        public IReadonlyVariable<int> GetCurrency(CurrencyTypes currencyType) => _currencies[currencyType];

        public bool Enough(CurrencyTypes currencyType, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            return _currencies[currencyType].Value >= amount;
        }

        public void Add(CurrencyTypes currencyType, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _currencies[currencyType].Value += amount;
        }

        public void Spend(CurrencyTypes currencyType, int amount)
        {
            if (Enough(currencyType, amount) == false)
                throw new InvalidOperationException($"Not enough {currencyType} in wallet");

            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _currencies[currencyType].Value -= amount;
        }
    }
}

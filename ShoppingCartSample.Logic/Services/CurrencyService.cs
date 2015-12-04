using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Data.Helpers;
using ShoppingCartSample.Data.Repositories;
using ShoppingCartSample.Domain.Exceptions;
using ShoppingCartSample.Domain.Models;

namespace ShoppingCartSample.Logic.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public Currency GetBaseCurrency()
        {
            return _currencyRepository.GetBaseCurrency();
        }

        public IEnumerable<Currency> GetAvailableCurrencies()
        {
            return _currencyRepository.GetAvailableCurrencies();
        }

        public string GetCurrencySymbol(string currencyCode)
        {
            return _currencyRepository.GetCurrencySymbol(currencyCode);
        }

        public decimal GetNewPriceModifier(string sourceCurrencyCode, string targetCurrencyCode)
        {                        
            if (string.IsNullOrWhiteSpace(sourceCurrencyCode) || string.IsNullOrWhiteSpace(targetCurrencyCode))
            {
                throw new ArgumentNullException();
            }
            return _currencyRepository.GetNewPriceModifier(sourceCurrencyCode, targetCurrencyCode);
        }
    }
}

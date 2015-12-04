using System;
using System.Collections.Generic;
using System.Configuration;
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

        public string BaseCurrencyCode { get; set; }

        private Currency _baseCurrency;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public Currency GetBaseCurrency()
        {
            if (_baseCurrency != null)
            {
                return _baseCurrency;
            }

            if (string.IsNullOrWhiteSpace(BaseCurrencyCode))
            {
                throw new ConfigurationErrorsException("Base currency is not defined in web.config.");
            }

            var currency = _currencyRepository.GetByCode(BaseCurrencyCode);

            if (currency == null)
            {
                throw new CurrencyNotFoundException();
            }

            _currencyRepository.SetAsDefault(currency);

            _baseCurrency = currency;
            return currency;
        }

        public IEnumerable<Currency> GetAvailableCurrencies()
        {
            if (CurrencyHelper.AvailableCurrencies != null)
            {
                return CurrencyHelper.AvailableCurrencies;
            }

            CurrencyHelper.AvailableCurrencies = _currencyRepository.GetAvailableCurrencies();

            return CurrencyHelper.AvailableCurrencies;            
        }

        public string GetCurrencySymbol(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                throw new ArgumentNullException();
            }

            if (currencyCode == BaseCurrencyCode)
            {
                return _baseCurrency.Symbol;
            }

            var currency = _currencyRepository.GetByCode(currencyCode);

            if (currency == null)
            {
                throw new CurrencyNotFoundException();
            }

            return currency.Symbol;
        }

        public decimal GetNewPriceModifier(string sourceCurrencyCode, string targetCurrencyCode)
        {                        
            if (string.IsNullOrWhiteSpace(sourceCurrencyCode) || string.IsNullOrWhiteSpace(targetCurrencyCode))
            {
                throw new ArgumentNullException();
            }

            //if it's the same currency, return 1.
            if (sourceCurrencyCode.Equals(targetCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return 1;
            }

            var bothCurrenciesExist = _currencyRepository.Exists(sourceCurrencyCode) && _currencyRepository.Exists(targetCurrencyCode);

            if (!bothCurrenciesExist)
            {
                throw new CurrencyNotFoundException();
            }

            var modifier = 1m;

            /*
            * If source currency code is not the base, then convert the source into the base first.
            * There might be additional factors that influence the conversion (eg. added charges, taxes, supply chain costs, etc.)
            */
            if (!sourceCurrencyCode.Equals(BaseCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                modifier = CurrencyHelper.GetPriceModifier(sourceCurrencyCode, BaseCurrencyCode);
                sourceCurrencyCode = BaseCurrencyCode;
            }

            return (modifier *= CurrencyHelper.GetPriceModifier(sourceCurrencyCode, targetCurrencyCode));
        }
    }
}

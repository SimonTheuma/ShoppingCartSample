using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Models;

namespace ShoppingCartSample.Logic.Services
{
    public interface ICurrencyService
    {
        Currency GetBaseCurrency();

        IEnumerable<Currency> GetAvailableCurrencies();

        string GetCurrencySymbol(string currencyCode);

        decimal GetNewPriceModifier(string sourceCurrencyCode, string targetCurrencyCode);        
    }
}

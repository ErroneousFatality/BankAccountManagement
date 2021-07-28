using Domain.Entities.Banks;
using Domain.Services.External;
using System;
using System.Collections.Generic;

namespace Domain.Services.Internal
{
    public class BankServiceFactory
    {
        // Fields
        private readonly IDictionary<BankType, IBankService> BankServiceDictionary;

        // Constructors
        public BankServiceFactory(IDictionary<BankType, IBankService> bankServiceDictionary)
        {
            BankServiceDictionary = new Dictionary<BankType, IBankService>(bankServiceDictionary);
        }

        // Methods

        /// <exception cref="ArgumentOutOfRangeException">
        ///     bankType
        /// </exception>
        /// <exception cref="NotImplementedException">
        ///     No bank service defined for bank type value.
        /// </exception>
        public IBankService GetBankService(BankType bankType)
        {
            if (!Enum.IsDefined(bankType))
            {
                throw new ArgumentOutOfRangeException(nameof(bankType));
            }
            if (!BankServiceDictionary.TryGetValue(bankType, out IBankService bankService))
            {
                throw new NotImplementedException($"No bank service defined for {nameof(BankType)} value = {bankType}.");
            }
            return bankService;
        }
    }
}

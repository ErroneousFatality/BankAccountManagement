using Domain.Entities.Banks;
using Domain.Entities.BankTransactions;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Domain.Entities.BankAccounts
{
    public class BankAccount
    {
        public Guid Id { get; private set; }

        public Guid BankId { get; private set; }
        public Bank Bank { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public string Number { get; private set; }
        public string Pin { get; private set; }

        public ICollection<BankTransaction> Transactions { get; private set; }

        // Constructors
        public BankAccount(
            Guid bankId,
            Guid userId,
            string bankAccountNumber,
            string pin
        )
        {
            if (!Regex.IsMatch(bankAccountNumber, SerbianBankAccountNumberRegex))
            {
                throw new ArgumentException($"Invalid bank account number. The correct format is: \"{SerbianBankAccountNumberRegex}\".", nameof(bankAccountNumber));
            }
            BankId = bankId;
            UserId = userId;
            Number = bankAccountNumber;
            SetPin(pin);
        }

        private BankAccount() { }

        // Methods
        public void SetPin(string pin)
        {
            if(string.IsNullOrWhiteSpace(pin) || pin.Length != PinLength)
            {
                throw new ArgumentException($"{nameof(pin)} length must be {PinLength}.", nameof(pin));
            }
            Pin = pin;
        }

        // Constants
        public const int PinLength = 6;
        public const int NumberMaxLength = 20;
        public const string SerbianBankAccountNumberRegex = "^\\d{3}\\-\\d{13}\\-\\d{2}";
    }
}

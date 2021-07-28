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

        public BankType BankId { get; private set; }
        public Bank Bank { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public string Number { get; private set; }
        public string Pin { get; private set; }

        public ICollection<BankTransaction> Transactions { get; private set; }

        // Constructors

        /// <exception cref="ArgumentException">
        ///     Invalid bank account number.
        ///     Pin length.
        /// </exception>
        public BankAccount(
            BankType bankId,
            Guid userId,
            string bankAccountNumber,
            string pin
        )
        {
            ValidateNumber(bankAccountNumber);
            BankId = bankId;
            UserId = userId;
            Number = bankAccountNumber;
            SetPin(pin);
        }

        private BankAccount() { }

        // Methods

        /// <exception cref="ArgumentException">
        ///     Pin length.
        /// </exception>
        public void SetPin(string pin)
        {
            ValidatePin(pin);
            Pin = pin;
        }

        /// <exception cref="ArgumentException">
        ///     Invalid bank account number.
        /// </exception>
        public static void ValidateNumber(string bankAccountNumber)
        {
            if (!Regex.IsMatch(bankAccountNumber, SerbianBankAccountNumberRegex))
            {
                throw new ArgumentException($"Invalid bank account number. The correct format is: \"{SerbianBankAccountNumberRegex}\".", nameof(bankAccountNumber));
            }
        }

        /// <exception cref="ArgumentException">
        ///     Invalid bank account number.
        /// </exception>
        public static void ValidatePin(string pin)
        {
            if (!Regex.IsMatch(pin, PinRegex))
            {
                throw new ArgumentException($"Invalid {nameof(pin)}. The correct format is four digits", nameof(pin));
            }
        }

        // Constants
        public const string SerbianBankAccountNumberRegex = "^\\d{3}\\-\\d{13}\\-\\d{2}$";
        public const int NumberLength = 20;

        public const string PinRegex = "^\\d{4}$";
        public const int PinLength = 4;
    }
}
